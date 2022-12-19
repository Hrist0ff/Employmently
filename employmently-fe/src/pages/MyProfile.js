import React, { useEffect } from "react";
import jwt from 'jwt-decode';
import axios from 'axios';
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import { Nav } from "react-bootstrap";

function MyProfile() {
    const token = localStorage.getItem("token");
    const [user, setUser] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState("");

    useEffect(() => {
        if (token) {
            if (!getRequest) {
                const decodedToken = jwt(token);
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                axios.get(`${process.env.REACT_APP_BACKEND}/User/${userId}`)
                    .then(response => {
                        setUser(response.data);
                        console.log(response.data);
                    })
                    .catch(error => {
                        setErrorMessage(error.response.data.Error[0]);
                    })
                setGetRequest(true);
            }
        }
    }, [token, getRequest]);


    return (
        <div>
            {ExpiredTokenCheck()}
            <h1>My profile</h1>
            <p>Id : {user.id}</p>
            <p>Username : {user.userName}</p>
            <p>Email : {user.email}</p>
            <p>PhoneNumber : {user.phoneNumber}</p>
            <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/`}>Back</Nav.Link>
        </div>
    )



}

export default MyProfile;