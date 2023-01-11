import React, { useEffect, useState } from "react";
import jwt from 'jwt-decode';
import axios from 'axios';
import { Nav } from "react-bootstrap";
import '../styles/myprofile.css';
import { FileUploader } from "react-drag-drop-files";
import FormInput from '../components/FormInput';
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";

function MyProfile() {
    const token = localStorage.getItem("accessToken");

    const [user, setUser] = React.useState({});
    const [getRequest, setGetRequest] = React.useState(false);

    const [photoInput, setPhotoInput] = React.useState(false);
    const [phoneInput, setPhoneInput] = React.useState(false);
    const [descriptionInput, setDescriptionInput] = React.useState(false);

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");
    const [file, setFile] = useState(null);


    const [values, setValues] = useState({
        phoneNumber: "",
        description: "",
    });

    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    const inputs = [
        {
            id: 1,
            name: "phoneNumber",
            type: "text",
            placeholder: "Enter your phone number",
            errorMessage: "It should be a valid phone number!",
            label: "Email",
            pattern: "^[0-9]{10}$",
            required: true,
        },
        {
            id: 2,
            name: "description",
            type: "text",
            placeholder: "Enter description",
            errorMessage: "It should be a valid description!",
            label: "Description",
            required: true,
        }
    ];

    const onPhotoUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        const formData = new FormData();
        formData.append('image', file);


        axios.post(`${process.env.REACT_APP_BACKEND}/Profile/uploadPic/${id}`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                "Authorization": `Bearer ${token}`,
                "Expires": "0"
            }
        })
            .then(response => {
                setSuccessMessage("Profile picture changed successfully!");

                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 4000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change picture. ");
            })
    }

    const onPhoneUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const phoneNumber = values.phoneNumber;


        axios.put(`${process.env.REACT_APP_BACKEND}/Profile/changePhoneNumber/${id}`, phoneNumber, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setPhoneInput(false);
                setSuccessMessage("Phone number changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change phone number. ");
            })
    }

    const onDescriptionUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const description = values.description;


        axios.put(`${process.env.REACT_APP_BACKEND}/Profile/changeDescription/${id}`, description, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setDescriptionInput(false);
                setSuccessMessage("Description changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change description. ");
            })
    }


    useEffect(() => {
        if (token) {
            if (!getRequest) {
                const decodedToken = jwt(token);
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                axios.get(`${process.env.REACT_APP_BACKEND}/User/${userId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                })
                    .then(response => {
                        setUser(response.data);
                        console.log(token);
                    })
                    .catch(error => {
                        setErrorMessage(error.response.data);
                        if (error.response.status === 401) {
                            ExpiredTokenCheck()
                        }
                    })
                setGetRequest(true);
            }
        }
        else {
            setTimeout(() => {
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Login`;
            }, 1000);
        }
    }, [token, getRequest]);

    const fileTypes = ["PNG"];
    const handleChange = (file) => {
        setFile(file);
    };

    return (
        < div className="my-profile" >
            {ExpiredTokenCheck()}

            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="sucMessage"> Success: {successMessage} </div>}

            <h1 className="heading">My profile</h1>

            <br></br>
            <div className="profile-header">
                <img src={user.profilePicture} className="profile-picture" alt="Profile pic"></img>
                {!photoInput && <button className="upload-btn" onClick={() => setPhotoInput(true)}>Edit Photo</button>}
                {photoInput && (
                    <div className="profile-header">
                        <FileUploader
                            handleChange={handleChange}
                            name="file"
                            types={fileTypes}
                        />
                        <button onClick={onPhotoUpload} className="upload-btn">Upload photo</button>
                    </div>
                )}
            </div>
            <div className="profile-details">
                <p><strong>USERNAME &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>    {user.userName}</p>
                <p><strong>EMAIL &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong>    {user.email}</p>
                <div>
                    <p><strong>PHONE NUMBER &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.phoneNumber}</p>
                    {!phoneInput && <button className="edit-btn" onClick={() => setPhoneInput(true)}>Edit Phone</button>}
                    {phoneInput && (
                        <div className="profile-header">
                            <form>
                                <FormInput
                                    key={inputs[0].id}
                                    {...inputs[0]}
                                    value={values[inputs[0].name]}
                                    onChange={onChange}
                                />
                                <button onClick={onPhoneUpload} className="upload-ph-btn">Update phone number</button>
                            </form>

                        </div>
                    )}
                </div>
                <p><strong>DESCRIPTION &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.description}</p>
                {!descriptionInput && <button className="edit-btn" onClick={() => setDescriptionInput(true)}>Edit Description</button>}
                {descriptionInput && (
                    <div className="profile-header">
                        <form>
                            <FormInput
                                key={inputs[1].id}
                                {...inputs[1]}
                                value={values[inputs[1].name]}
                                onChange={onChange}
                            />

                            <button onClick={onDescriptionUpload} className="upload-ph-btn">Update description</button>
                        </form>

                    </div>
                )}
            </div>
            <Nav.Link className="back-but" href={`${process.env.REACT_APP_SERVER_PAGE}/`}>Back</Nav.Link>
        </div>
    )

}

export default MyProfile;