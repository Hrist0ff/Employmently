import React, { useEffect, useState } from "react";
import jwt from 'jwt-decode';
import axios from 'axios';
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import { Nav } from "react-bootstrap";
import '../styles/myprofile.css';
import { FileUploader } from "react-drag-drop-files";


function MyProfile() {
    const token = localStorage.getItem("token");
    
    const [user, setUser] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");
    const [file, setFile] = useState(null);



    const onPhotoUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        const formData = new FormData();
        formData.append('image', file);


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

    const fileTypes = ["PNG"];
    const handleChange = (file) => {
        setFile(file);
    };

    return (
        < div className="my-profile" >
            <FileUploader handleChange={handleChange} name="file" types={fileTypes} />
            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="suc"> Success: {successMessage} </div>}
            {ExpiredTokenCheck()}
            <h1 className="heading">My profile</h1>
            <br></br>
            <img src={user.profilePicture} className="profile-picture" alt="Profile pic"></img>
            <FileUploader
                handleChange={handleChange}
                name="file"
                types={fileTypes}
            />
            <button onClick={onPhotoUpload}>Upload photo</button>
            {/* <p>Id : {user.id}</p> */}
            <p>Username : {user.userName}</p>
            <p>Email : {user.email}</p>
            <p>PhoneNumber : {user.phoneNumber}</p>

            {/* <input type="file" accept="image/png" onChange={onPhotoChange} /> */}
            {/* <button onClick={onPhotoUpload}>Change profile picture</button> */}
            <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/`}>Back</Nav.Link>
        </div>
    )

    // < div className = "my-profile" >
    //     <div className="profile-header">
    //         <h1>My Profile</h1>
    //     </div>
    //     <div className="profile-info">
    //         <div className="profile-picture">
    //             <img src={user.profilePicture} alt="Profile Picture" />
    //         </div>
    //         <div className="personal-info">
    //             <h2>Personal Information</h2>
    //             <p>Name: John Doe</p>
    //             <p>Email: john.doe@example.com</p>
    //             <p>Location: New York, NY</p>
    //         </div>
    //     </div>
    // </div >



}

export default MyProfile;