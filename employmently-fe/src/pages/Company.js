import { useParams } from "react-router-dom";
import { useState } from "react";
import React, { useEffect } from "react";
import axios from 'axios';
import { Nav } from "react-bootstrap";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';




function Company() {
    const [user, setUser] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);


    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");


    let parameters = useParams();
    var id = parameters.id;;

    // Checking account and getting user data
    useEffect(() => {
        if (!getRequest) {
            axios.get(`${process.env.REACT_APP_BACKEND}/Company/getCertainCompany/${id}`)
                .then(response => {
                    setUser(response.data);
                    console.log(response.data);
                })
                .catch(error => {
                    setErrorMessage(error.response.data.Error[0]);
                })
            setGetRequest(true);
        }
    }, [id, getRequest]);


    return (
        < div className={`my-profile-comp`}>
            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="sucMessage"> Success: {successMessage} </div>}
            {ExpiredTokenCheck()}
            <h1 className="heading">Company     {user.name}</h1>

            <br></br>
            <div className="profile-header">
                <img src={user.profilePicture} className="profile-picture" alt="Profile pic"></img>

            </div>
            <div className="profile-details">

                <div style={{ display: 'flex', flexWrap: 'nowrap', gap: '150px' }}>
                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <p><strong>USERNAME &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>    {user.name}</p>
                        <p><strong>YEAR CREATED &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong>    {user.yearCreated}</p>

                    </div>
                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <p><strong>EIK &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>    {user.uniqueIdentifier}</p>
                        <p><strong>EMPLOYEES &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.employees ? user.employees : "0"} employees</p>
                    </div>
                </div>

                <p><strong>DESCRIPTION &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.description}</p>

            </div>
            <Nav.Link className="back-but-comp" href={`${process.env.REACT_APP_SERVER_PAGE}/`}>Back</Nav.Link>
        </div>
    )
}

export default Company;