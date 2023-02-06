import axios from "axios";
import { useEffect } from "react";
import { useState } from "react";
import '../styles/listings.css';
import format from 'date-fns/format';
import ru from 'date-fns/locale/bg';
import { isToday, isYesterday } from 'date-fns';
import React from "react";
import jwt from 'jwt-decode';


function CompanyListings({ companyId, isMyCompany }) {
    const token = localStorage.getItem("accessToken");
    const [listings, setListings] = useState([]);
    const [performed, setPerformed] = useState(false);

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    const [isThisCompany, setIsThisCompany] = useState(false);

    const [deleteListing, setDeleteListing] = useState(0);

    const deleteAlisting = (id) => {

        axios.delete(`${process.env.REACT_APP_BACKEND}/Company/DeleteListing/${id}`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                console.log(response.data);
            })
            .catch(error => {
                setErrorMessage(error.response.data);
            })
    }

    useEffect(() => {
        axios.get(`${process.env.REACT_APP_BACKEND}/Information/getCompanyListings/${companyId}`)
            .then(response => {
                const listings = response.data.map(listing => {
                    listing.createdDate = new Date(listing.createdDate);
                    if (isToday(listing.createdDate)) {
                        listing.createdDate = 'Днес';
                    } else if (isYesterday(listing.createdDate)) {
                        listing.createdDate = 'Вчера';
                    } else {
                        listing.createdDate = format(listing.createdDate, "dd MMMM", { locale: ru });
                    }
                    return listing;
                });
                setListings(listings);
            })
            .catch(error => {
                console.log(error);
            })
        setPerformed(true);
    }, [companyId, performed]);

    return (
        <div>
            {listings.map((listing, idx) => {
                return (
                    <div className="company-listing-card" key={idx}>
                        <div className="listing-company">
                            <img src={listing.authorPic} alt="Company" className="img-company"></img>
                            <p className="company-creator">{listing.authorName}</p>
                        </div>
                        <div className="listing-left-side" key={idx}>
                            <a className="listing-name" href={`${process.env.REACT_APP_SERVER_PAGE}/Listing/${listing.id}`}>{listing.name}
                                {listing.location ? <p key={listing.location} className="paragraph-details">🏢{listing.location}</p> : null}
                                {listing.salary ? <p key={listing.salary} className="paragraph-details">💰Salary from {listing.salary} lv.</p> : null}
                                {listing.categoryNames ? <p className="paragraph-details">📁{listing.categoryNames.map((category, index) => {
                                    return (
                                        <span key={index}>{category} &nbsp;</span>
                                    )
                                })}</p> : null}
                                {listing.arrangement ? <p key={listing.arrangement} className="paragraph-details">💼{listing.arrangement}</p> : null}
                            </a>
                        </div>
                        <div className="listing-right-side-company">
                            <p className="listing-date">📅{listing.createdDate}</p>
                            {isMyCompany ?
                                !(deleteListing === listing.id) ?
                                    <button className="listing-tag-reject" onClick={() => setDeleteListing(listing.id)}>Delete listing</button>
                                    :
                                    <div style={{ display: "flex", flexDirection: "column" }}>
                                        <p className="listing-date">Are you sure you <br></br>want to delete this <br></br>listing?</p>
                                        <div style={{ display: "flex", flexDirection: "row" }}>
                                            <button className="listing-tag-accept" onClick={() => deleteAlisting(listing.id)}>Yes</button>
                                            <button className="listing-tag-reject" onClick={() => setDeleteListing(0)}>No</button>
                                        </div>
                                    </div>
                                : null}
                        </div>
                    </div>
                )
            }
            )}
        </div >
    );
};

export default CompanyListings;