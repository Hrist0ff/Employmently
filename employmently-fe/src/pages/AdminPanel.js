import { useParams } from "react-router-dom";
import { useState } from "react";
import React, { useEffect } from "react";
import axios from 'axios';
import { Nav } from "react-bootstrap";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import jwt from 'jwt-decode';
import { Link } from 'react-router-dom';
import '../styles/adminpanel.css';
import Remote from '../images/remote.png';
import Logo from "../images/employment.png";
import Navbar from "../components/Navbar";




function AdminPanel() {

    const [listings, setListings] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    function objectToArray(obj) {
        return Object.values(obj)
    }

    // For date time
    function formatDateTime(dateTimeString) {
        const options = {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            hour12: false
        };

        return new Intl.DateTimeFormat('en-US', options).format(new Date(dateTimeString));
    }

    const acceptListing = (id) => {
        const token = localStorage.getItem("accessToken");

        axios.post(`${process.env.REACT_APP_BACKEND}/Admin/AcceptListing/${id}`,
            {},
            {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            .then(response => {
                setSuccessMessage("Listing accepted!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/AdminPanel`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't accept listing.");
            })
    }

    const rejectListing = (id) => {
        const token = localStorage.getItem("accessToken");

        axios.post(`${process.env.REACT_APP_BACKEND}/Admin/RejectListing/${id}`,
            {},
            {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            .then(response => {
                setSuccessMessage("Listing rejected!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/AdminPanel`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't reject listing.");
            })
    }




    useEffect(() => {
        if (!getRequest) {
            const token = localStorage.getItem("accessToken");
            if (token) {
                const decodedToken = jwt(token);
                let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
                if (role !== 'Administrator') {
                    setErrorMessage("You don't have permission to view this page!");
                    setTimeout(() => {
                        window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
                    }, 3000);
                }
            }
            axios.get(`${process.env.REACT_APP_BACKEND}/Admin/getListings`, {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
                .then(response => {
                    setListings(response.data);

                })
                .catch(error => {
                    setErrorMessage(error.response.data);
                })
            setGetRequest(true);
        }
    }, [getRequest]);




    const listingsArray = objectToArray(listings);


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
            <div >
                {errorMessage && <div className="err" style={{ width: '93%', marginLeft: '0%' }}> Error: {errorMessage} </div>}
                {successMessage && <div className="suc" style={{ width: '93%', marginLeft: '0%' }}> Success: {successMessage} </div>}
                {listingsArray.map((listing, idx) => {
                    return (
                        <div style={{ paddingTop: '20px' }}>
                            <div className="listing-section-home">
                                <div className="listing-container">

                                    <div className="listing-item">
                                        <a href={`${process.env.REACT_APP_SERVER_PAGE}/Company/${listing.companyId}`} className="listing-a-div" >
                                            <div>
                                                <img className="listing-company-logo" src={listing.authorPic} alt="Company logo"></img>
                                                <p className="listing-company-name-text">{listing.authorName}</p>
                                            </div>
                                        </a>
                                        <div className="listing-a">
                                            <div>
                                                <div className="listing-header">
                                                    <p className="listing-title-text">{listing.name}</p>
                                                    <p className="listing-date">📅{formatDateTime(listing.createdDate)}</p>
                                                </div>
                                                <div className="listing-description-adminpanel">
                                                    <p className="listing-description-text-adminpanel">Description</p>
                                                    <br></br>
                                                    <p className="listing-description-text-adminpanel">{listing.description}</p>
                                                </div>
                                                <div className="listing-categories-tags-div-ap">
                                                    <div style={{ display: "flex", flexDirection: "row", width: "50%" }}>
                                                        {listing.location ? <p className="listing-tag">📍{listing.location}</p> : null}
                                                        {listing.arrangement && listing.arrangement === "Remote" ?
                                                            <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}><img src={Remote} className="listing-tag-image"></img> &nbsp;{listing.arrangement}</p>
                                                            : null}
                                                        {listing.arrangement && listing.arrangement === "On-site" ?
                                                            <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💼 {listing.arrangement}</p>
                                                            : null}
                                                        {listing.arrangement && listing.arrangement === "Hybrid" ?
                                                            <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%', alignItems: 'center' }}><img src={Remote} className="listing-tag-image-hybrid"></img> &nbsp; / 💼 {listing.arrangement}</p>
                                                            : null}
                                                        {listing.salary ? <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💰 {listing.salary} lv.</p> : null}
                                                    </div>
                                                    <div className="admin-panel-buttons">
                                                        <button className="listing-tag-accept" onClick={() => acceptListing(listing.id)}>Accept</button>
                                                        <button className="listing-tag-reject" onClick={() => rejectListing(listing.id)}>Reject</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )
                }
                )}
            </div >
        </div>
    )

}
export default AdminPanel;