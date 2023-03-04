import { useParams } from "react-router-dom";
import { useState } from "react";
import React, { useEffect } from "react";
import axios from 'axios';
import Logo from "../images/employment.png";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import Navbar from "../components/Navbar";
import '../styles/company.css';
import CompanyListings from "../components/CompanyListings";
import { Link } from 'react-router-dom';



function Company() {

    const [user, setUser] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);


    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");


    let parameters = useParams();
    var id = parameters.id;

    // Checking account and getting user data
    useEffect(() => {
        if (!getRequest) {
            axios.get(`${process.env.REACT_APP_BACKEND}/Information/getCertainCompany/${id}`)
                .then(response => {
                    setUser(response.data);
                })
                .catch(error => {
                    setErrorMessage(error.response.data.Error[0]);
                })
            setGetRequest(true);
        }
    }, [id, getRequest]);


    return (
        <div className="background">
            <div>
                <div className="nav">
                    <Link to="/">
                        <img src={Logo} className='log' alt='Employmently logo'></img>
                    </Link>
                    {/* Aligned items to the right */}
                    <div className="justify-end">
                        <label htmlFor="toggle">&#9776;</label>
                        <input type="checkbox" id="toggle" />
                        <div className="menu">
                            {ExpiredTokenCheck()}
                            {Navbar()}
                        </div>
                    </div>
                </div>

                <div className="company-container">
                    <div className="company-info">
                        <img src={user.profilePicture} alt="User profile" className="company-pic"></img>
                        <span className="company-divider" />
                        <p className="company-heading">{user.name}</p>
                    </div>
                    <div className="company-for-and-technologies">
                        <div className="company-for">
                            <div className="company-for-heading">
                                <p className="company-for-text">About the company</p>
                            </div>
                            <div>
                                <p>{user.description}</p>
                            </div>
                            <div className="technologies-list">
                                <p className="tag">Year created: {user.yearCreated}</p>
                                <p className="tag">Number of employees: {user.employees}</p>
                                <p className="tag">Unique Identifier: {user.uniqueIdentifier}</p>
                                <p className="tag">Phone number: {user.phoneNumber}</p>
                            </div>
                        </div>
                        <div className="company-technologies">
                            <div className="company-for-heading">
                                <p className="company-for-text">Categories</p>
                            </div>
                            <p className="company-technologies-text">Categories that we are working with:</p>
                            <div className="technologies-list">
                                {user && user.technologies ? user.technologies.map((technology, index) => {
                                    return <p key={index} className="tag">{technology}</p>;
                                }) : null}
                            </div>
                        </div>
                    </div>
                    <div className="company-listings">
                        <div className="company-for-heading">
                            <p className="company-for-text">Company listings</p>
                        </div>
                        {id ? <CompanyListings companyId={id} isMyCompany={false} /> : null}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Company;