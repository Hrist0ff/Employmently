import { useParams } from "react-router-dom";
import { useState } from "react";
import React, { useEffect } from "react";
import axios from 'axios';
import { Nav } from "react-bootstrap";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import jwt from 'jwt-decode';
import '../styles/adminpanel.css';




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
            second: 'numeric',
            timeZone: 'UTC'
        };

        return new Intl.DateTimeFormat('en-US', options).format(new Date(dateTimeString));
    }

    const acceptListing = (id) => {
        const token = localStorage.getItem("token");

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
                    window.location.href(`${process.env.REACT_APP_SERVER_PAGE}/AdminPanel`);
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't accept listing.");
            })
    }

    const rejectListing = (id) => {
        const token = localStorage.getItem("token");

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
                    window.location.href(`${process.env.REACT_APP_SERVER_PAGE}/AdminPanel`);
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't reject listing.");
            })
    }




    useEffect(() => {
        if (!getRequest) {
            const token = localStorage.getItem("token");
            if (token) {
                const decodedToken = jwt(token);
                let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
                if (role !== 'Administrator') {
                    setErrorMessage("You don't have permission to view this page!");
                    setTimeout(() => {
                        window.location.href = `${process.env.REACT_APP_SERVER_PAGE} / `;
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
                    setErrorMessage(error.response.data.Error[0]);
                })
            setGetRequest(true);
        }
    }, [getRequest]);




    const listingsArray = objectToArray(listings);


    return (
        <div className="admin-panel">
            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="sucMessage"> Success: {successMessage} </div>}
            {ExpiredTokenCheck()}
            <h1 className="heading-p">Admin Panel</h1>

            <div className="listing-headers">
                <p>Name</p>
                <p>Description</p>
                <p>Created on</p>
                <p>Author</p>
                <p>Categories</p>
                <p>Option</p>
            </div>
            {listingsArray.length > 0 ? listingsArray.map(listing => (
                <div className="listing">
                    <div key={listing.id}>{listing.name}</div>
                    <div className="p-listing">{listing.description}</div>
                    <div>{listing.createdDate ? formatDateTime(listing.createdDate) : ""}</div>
                    <div>{listing.authorName}</div>
                    <ul>
                        {listing.categoryNames ? listing.categoryNames.map(category => (
                            <li key={category}>{category}</li>
                        )) : null}
                    </ul>
                    <div>
                        <button className="accept-button" onClick={() => acceptListing(listing.id)}></button>
                        <button className="reject-button" onClick={() => rejectListing(listing.id)}></button>
                    </div>
                </div>
            )) : <p className="paragraph-center">There aren't any pending listings.</p>}
        </div>
    )

}
export default AdminPanel;