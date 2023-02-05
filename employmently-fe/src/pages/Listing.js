import React, { useState, useEffect } from "react";
import axios from "axios";
import { Link, useParams } from 'react-router-dom';
import Logo from "../images/employment.png";
import Calendar from "../images/calendar.png";
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Navbar from "../components/Navbar";
import format from 'date-fns/format';
import Remote from "../images/remote.png";
import { isToday, isYesterday } from 'date-fns';
import "../styles/listingview.css";
import jwt from 'jwt-decode';
import UploadCloud from "../images/upload-cloud.png";



function Listing() {
    const token = localStorage.getItem("accessToken");

    const [listing, setListing] = useState([]);

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    const [performed, setPerformed] = useState(false);

    const [clickedButton, setClickedButton] = useState(false);

    const id = useParams().id;


    const [cvv, setCv] = useState(null);
    const [ml, setMl] = useState(null);


    const cvChange = (file) => {
        console.log(file);
        setCv(file);
    };

    const mlChange = (file) => {
        setMl(file);
    };

    const applicate = (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append('cv', cvv.target.files[0]);
        formData.append('motivationalLetter', ml.target.files[0]);

        axios.post(`${process.env.REACT_APP_BACKEND}/User/ApplicateForJob/${id}`, formData, {
            headers: {
                "Authorization": `Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            }
        }).then(response => {
            setSuccessMessage(response.data);
            setTimeout(() => {
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
            }, 3000);

        })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })
    }

    const userCheck = () => {
        if (token) {
            const decodedToken = jwt(token);
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            console.log(role);
            if (role === 'Candidate') {
                return true;
            }
        }
        else {
            setTimeout(() => {
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Login`;
            }, 500);
        }
    }

    useEffect(() => {
        axios.get(`${process.env.REACT_APP_BACKEND}/Information/getCertainListing/${id}`)
            .then(response => {
                var listing = response.data
                listing.createdDate = new Date(listing.createdDate);
                if (isToday(listing.createdDate)) {
                    listing.createdDate = 'today';
                } else if (isYesterday(listing.createdDate)) {
                    listing.createdDate = 'yesterday';
                } else {
                    listing.createdDate = format(listing.createdDate, "dd MMMM").toLocaleLowerCase();
                }
                setListing(listing);
                console.log(listing)
            })
            .catch(error => {
                console.log(error);
            })
        setPerformed(true);
    }, [id, performed]);

    return (
        <div className="background">
            <div className='home-page'>
                <div className="nav">
                    <Link to="/">
                        <img src={Logo} className='log' alt='Employmently logo'></img>
                    </Link>
                    {/* Aligned items to the right */}
                    <div className="justify-end">
                        <label htmlFor="toggle">&#9776;</label>
                        <input type="checkbox" id="toggle" />
                        <div className="menu" >
                            {ExpiredTokenCheck()}
                            {Navbar()}
                        </div>
                    </div>
                </div>
            </div>
            <div className="listing-section">

                <div className="listing-container">
                    {errorMessage && <div className="err"> Error: {errorMessage} </div>}
                    {successMessage && <div className="sucMessage-cl"> Success: {successMessage} </div>}
                    <div className="listing-item">
                        <div className="listing-company-name">
                            <img className="listing-company-logo" src={listing.authorPic} alt="Company logo"></img>
                            <p className="listing-company-name-text">{listing.authorName}</p>
                        </div>
                        <div className="listing-title">
                            <p className="listing-title-text">{listing.name}</p>
                            {!clickedButton ?
                                <button onClick={() => {
                                    setClickedButton(!clickedButton);
                                    userCheck();
                                }} className="applicate-button">Applicate for this job</button>
                                : null}
                            {clickedButton && userCheck() ?
                                <div className="cv-ml-div">
                                    <label className="custom-file-upload">
                                        <img src={UploadCloud} alt="Upload CV" style={{ width: "30px", height: "30px", marginRight: "5px" }}></img>
                                        {cvv ? cvv.target.files[0].name : "CV"}
                                        <input onChange={cvChange} style={{ display: "none" }} type="file" id="cv" name="cv" />
                                    </label>
                                    <label className="custom-file-upload">
                                        <img src={UploadCloud} alt="Upload Motivation Letter" style={{ width: "30px", height: "30px", marginRight: "5px" }}></img>
                                        {ml ? ml.target.files[0].name : "Motivation Letter"}
                                        <input onChange={mlChange} style={{ display: "none" }} type="file" id="ml" name="ml" />
                                    </label>
                                    <button onClick={applicate} className="submit-button-listings">Submit</button>
                                </div>

                                : null}
                            <div className="listing-categories-tags-div">
                                <p className="listing-tag">📍{listing.location}</p>
                                {listing.arrangement && listing.arrangement === "Remote" ?
                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}><img src={Remote} alt="Remote" className="listing-tag-image"></img> &nbsp;{listing.arrangement}</p>
                                    : null}
                                {listing.arrangement && listing.arrangement === "On-site" ?
                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💼 {listing.arrangement}</p>
                                    : null}
                                {listing.arrangement && listing.arrangement === "Hybrid" ?
                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%', alignItems: 'center' }}><img src={Remote} alt="Remote" className="listing-tag-image-hybrid"></img> &nbsp; / 💼 {listing.arrangement}</p>
                                    : null}
                                {listing.salary ? <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💰 {listing.salary} lv.</p> : null}
                            </div>
                        </div>
                    </div>
                </div>

                <div className="listing-container">
                    <div className="listing-categories-div">
                        <p className="listing-categories-text">THIS LISTING IS PUBLISHED IN THE FOLLOWING CATEGORIES</p>
                        <div className="listing-categories-map">
                            {listing.categoryNames && listing.categoryNames.map((category, index) => {
                                return (
                                    <p className="listing-category-view" key={index}>{category}</p>
                                )
                            })}
                        </div>
                    </div>
                </div>

                <div className="listing-container">
                    <div className="listing-description-div">
                        <div className="published-info">
                            <img src={Calendar} alt="Calendar" className="calendar-photo"></img>
                            {listing.createdDate === "today" || listing.createdDate === "yesterday" ? <p className="listing-date-text">Published {listing.createdDate}</p> : <p className="listing-date-text">Published on {listing.createdDate}</p>}
                        </div>
                        <div className="listing-description" style={{ marginTop: "15px" }}><pre className="pre-text">{listing.description}</pre></div>
                    </div>
                </div>

            </div>
        </div >

    );
}

export default Listing;