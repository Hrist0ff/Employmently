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


function Listing() {
    const [listing, setListing] = useState([]);

    const [performed, setPerformed] = useState(false);

    const id = useParams().id;


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

                    <div className="listing-item">
                        <div className="listing-company-name">
                            <img className="listing-company-logo" src={listing.authorPic} alt="Company logo"></img>
                            <p className="listing-company-name-text">{listing.authorName}</p>
                        </div>
                        <div className="listing-title">
                            <p className="listing-title-text">{listing.name}</p>
                            <button className="applicate-button">Applicate for this job</button>
                            <div className="listing-categories-tags-div">
                                <p className="listing-tag">📍{listing.location}</p>
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
                            <img src={Calendar} className="calendar-photo"></img>
                            {listing.createdDate === "today" || listing.createdDate === "yesterday" ? <p className="listing-date-text">Published {listing.createdDate}</p> : <p className="listing-date-text">Published on {listing.createdDate}</p>}
                        </div>
                        <p className="listing-description"><pre className="pre-text">{listing.description}</pre></p>
                    </div>
                </div>

            </div>
        </div>

    );
}

export default Listing;