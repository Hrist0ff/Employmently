import React, { useEffect, useState } from "react";
import jwt from 'jwt-decode';
import axios from 'axios';
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import { Nav } from "react-bootstrap";

function MyProfile() {
    const token = localStorage.getItem("token");
    const [user, setUser] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    const [picture, setPicture] = useState({
        selectedFile: null
    });



    const onPhotoChange = event => {
        setPicture(event.target.files[0]);
        // this.setState({ selectedFile: event.target.files[0] });
    };

    const onPhotoUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        const formData = new FormData();
        formData.append('image', picture);


        axios.post(`${process.env.REACT_APP_BACKEND}/Profile/uploadPic/${id}`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setSuccessMessage("Profile picture changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change picture. ");
            })
    }



    useEffect(() => {
        if (token) {
            if (!getRequest) {
                const decodedToken = jwt(token);
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                axios.get(`${process.env.REACT_APP_BACKEND}/User/${userId}`)
                    .then(response => {
                        setUser(response.data);
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
            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="suc"> Success: {successMessage} </div>}
            {ExpiredTokenCheck()}
            <h1>My profile</h1>
            <p>Id : {user.id}</p>
            <p>Username : {user.userName}</p>
            <p>Email : {user.email}</p>
            <p>PhoneNumber : {user.phoneNumber}</p>
            <img src={user.profilePicture} alt="Profile pic"></img>
            <input type="file" accept="image/png" onChange={onPhotoChange} />
            <button onClick={onPhotoUpload}>Change profile picture</button>
            <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/`}>Back</Nav.Link>
        </div>
    )



}

export default MyProfile;