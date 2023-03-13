import React, { useEffect, useState } from "react";
import jwt from 'jwt-decode';
import axios from 'axios';
import '../styles/myprofile.css';
import { FileUploader } from "react-drag-drop-files";
import FormInput from '../components/FormInput';
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Navbar from "../components/Navbar";
import { Link } from "react-router-dom";
import Logo from "../images/employment.png";
import Remote from "../images/remote.png";
import format from 'date-fns/format';
import { isToday, isYesterday, isTomorrow } from 'date-fns';
import "react-notifications/lib/notifications.css";
import { NotificationManager } from 'react-notifications';

function MyProfile() {
    const token = localStorage.getItem("accessToken");

    const [user, setUser] = React.useState({});
    const [applications, setApplications] = React.useState([]);
    const [performed, setPerformed] = React.useState(false);

    const [photoInput, setPhotoInput] = React.useState(false);
    const [phoneInput, setPhoneInput] = React.useState(false);
    const [descriptionInput, setDescriptionInput] = React.useState(false);

    const [file, setFile] = useState(null);

    const showSuccessMessage = (message) => {
        NotificationManager.success(message, 'Success');
    }

    const showErrorMessage = (message) => {
        NotificationManager.error(message, 'Error');
    }


    const [values, setValues] = useState({
        phoneNumber: "",
        description: "",
    });

    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    const isCandidate = () => {
        const decodedToken = jwt(token);
        return decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Candidate";
    }
    const inputs = [
        {
            id: 1,
            name: "phoneNumber",
            type: "text",
            placeholder: "Enter your phone number",
            errorMessage: "It should be a valid phone number!",
            label: "Phone number",
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
                showSuccessMessage("Profile picture changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 4000);
            })
            .catch(error => {
                showErrorMessage("Couldn't change picture. ");
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
                showSuccessMessage("Phone number changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 3000);
            })
            .catch(error => {
                showErrorMessage("Couldn't change phone number. ");
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
                showSuccessMessage("Description changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyProfile`;
                }, 3000);
            })
            .catch(error => {
                showErrorMessage("Couldn't change description. ");
            })
    }


    useEffect(() => {
        if (token) {
            if (!performed) {
                const decodedToken = jwt(token);
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                axios.get(`${process.env.REACT_APP_BACKEND}/User/${userId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                })
                    .then(response => {
                        setUser(response.data);
                    })
                    .catch(error => {
                        showErrorMessage(error.response.data);
                        if (error.response.status === 401) {
                            ExpiredTokenCheck()
                        }
                    })
                if (role === "Candidate") {
                    axios.get(`${process.env.REACT_APP_BACKEND}/User/getMyApplications`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    })
                        .then(response => {
                            const applications = response.data.map(application => {
                                application.applicationTime = new Date(application.applicationTime);
                                application.suggestedInterviewDate = new Date(application.suggestedInterviewDate);

                                if (isToday(application.suggestedInterviewDate)) {
                                    application.suggestedInterviewDate = 'Today at' + format(application.suggestedInterviewDate, " HH:mm");
                                } else if (isTomorrow(application.suggestedInterviewDate)) {
                                    application.suggestedInterviewDate = 'Tommorow at' + format(application.suggestedInterviewDate, " HH:mm");
                                } else {
                                    application.suggestedInterviewDate = format(application.suggestedInterviewDate, "dd/MM/yyyy HH:mm");
                                }

                                if (isToday(application.applicationTime)) {
                                    application.applicationTime = 'Today at' + format(application.applicationTime, " HH:mm");
                                } else if (isYesterday(application.applicationTime)) {
                                    application.applicationTime = 'Yesterday at' + format(application.applicationTime, " HH:mm");
                                } else {
                                    application.applicationTime = format(application.applicationTime, "dd/MM/yyyy HH:mm");
                                }
                                return application;
                            });
                            setApplications(applications);
                        })
                        .catch(error => {
                            showErrorMessage(error.response.data);
                            if (error.response.status === 401) {
                                ExpiredTokenCheck()
                            }
                        })
                }
                setPerformed(true);
            }
        }
        else {
            setTimeout(() => {
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Login`;
            }, 1000);
        }
    }, [token, performed]);

    const fileTypes = ["PNG"];
    const handleChange = (file) => {
        setFile(file);
    };


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
            </div>


            <div className="company-container">
                <div className="user-info">
                    <img src={user.profilePicture} alt="User profile" className="user-pic"></img>
                    <div className="user-main-info">
                        <div className="user-name-and-divider">
                            <span className="company-divider" />
                            <p className="company-heading">{user.userName}</p>
                        </div>

                        {!photoInput && <button className="user-edit-btn" onClick={() => setPhotoInput(true)}>Edit Photo</button>}
                        {photoInput && (
                            <div className="profile-header">
                                <FileUploader
                                    handleChange={handleChange}
                                    name="file"
                                    types={fileTypes}
                                />
                                <button onClick={onPhotoUpload} className="user-upload-btn">Upload photo</button>
                            </div>
                        )}
                    </div>
                </div>
                <div className="company-for-and-technologies">
                    <div className="user-for">
                        <div className="company-for-heading">
                            <p className="company-for-text">About me</p>
                        </div>
                        <div>
                            <p>{user.description}</p>
                            {!descriptionInput && <button className="company-edit-button" onClick={() => setDescriptionInput(true)}>Edit Description</button>}
                            {descriptionInput && (
                                <div className="profile-header">
                                    <form className='company-form' style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>

                                        <textarea
                                            rows={8}
                                            cols={50}
                                            key={inputs[1].id}
                                            {...inputs[1]}
                                            style={{
                                                padding: "10px",
                                            }}
                                            value={values[inputs[1].name]}
                                            onChange={onChange}
                                            placeholder="Enter your description here..."
                                        />

                                        <button onClick={onDescriptionUpload} className="company-button">Update description</button>
                                    </form>

                                </div>
                            )}
                        </div>
                        <div className="technologies-list-mycomp">

                            <div className="company-row">
                                <p className="tag">Phone number: {user.phoneNumber}</p>
                                {!phoneInput && <button className="company-edit-button-emp" onClick={() => setPhoneInput(true)}>Edit Phone</button>}
                                {phoneInput && (
                                    <div className="profile-header">
                                        <form className="phone-form" style={{ display: 'flex', alignItems: 'center' }}>
                                            <FormInput
                                                key={inputs[0].id}
                                                {...inputs[0]}
                                                value={values[inputs[0].name]}
                                                onChange={onChange}
                                            />
                                            <button disabled={!values[inputs[0].name].match(inputs[0].pattern)} onClick={onPhoneUpload} className="user-button-ph">Update phone number</button>
                                        </form>

                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
                {isCandidate() ?
                    <div className="company-listings">
                        <div className="company-for-heading">
                            <p className="company-for-text">My applications</p>
                        </div>
                        {user && applications ?
                            <div>
                                {applications.map((application) => {
                                    return (
                                        <div style={{ paddingTop: '20px' }}>
                                            <div className="listing-section-home">
                                                <div className="listing-container">
                                                    <div className="listing-item-user">
                                                        <a href={`${process.env.REACT_APP_SERVER_PAGE}/Company/${application.companyId}`} className="listing-a-div-myp" >
                                                            <div>
                                                                <img className="listing-company-logo" src={application.companyPic} alt="Company logo"></img>
                                                                <p className="listing-company-name-text">{application.listingCompany}</p>
                                                            </div>
                                                        </a>
                                                        <a href={`${process.env.REACT_APP_SERVER_PAGE}/Listing/${application.listingId}`} className="listing-a-application">
                                                            <div>
                                                                <div className="listing-header">
                                                                    <p className="listing-title-text">{application.listingName}</p>
                                                                    <p className="listing-date">📅{application.applicationTime}</p>
                                                                </div>
                                                                <div className="listing-categories-tags-div">
                                                                    <p className="listing-tag">📍{application.listingLocation}</p>
                                                                    {application.arrangement && application.arrangement === "Remote" ?
                                                                        <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}><img src={Remote} alt="Remote job" className="listing-tag-image"></img> &nbsp;{application.arrangement}</p>
                                                                        : null}
                                                                    {application.arrangement && application.arrangement === "On-site" ?
                                                                        <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💼 {application.arrangement}</p>
                                                                        : null}
                                                                    {application.arrangement && application.arrangement === "Hybrid" ?
                                                                        <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%', alignItems: 'center' }}><img src={Remote} alt="Remote job" className="listing-tag-image-hybrid"></img> &nbsp; / 💼 {application.arrangement}</p>
                                                                        : null}
                                                                    {application.listingSalary ? <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💰 {application.listingSalary} lv.</p> : null}

                                                                </div>
                                                            </div>
                                                        </a>
                                                        <div className="status-and-files" style={{
                                                            background: (application.listingStatus === "Pending" ? '#FFFF99' :
                                                                (application.listingStatus === "Accepted" ? 'aquamarine' :
                                                                    (application.listingStatus === "Rejected" ? '#FF9999' : 'inherit')))
                                                        }}>
                                                            {application.listingStatus === "Pending" ?
                                                                <div style={{ display: 'flex', flexDirection: 'column' }}>
                                                                    <p className="listing-tag">Status: ⌛</p>
                                                                </div> : null}
                                                            {application.listingStatus === "Accepted" ?
                                                                <div style={{ display: 'flex', flexDirection: 'column' }}>
                                                                    <p className="listing-tag">Status: ✅</p>
                                                                    <p className="listing-tag">Suggested date interview:<br></br> {application.suggestedInterviewDate}</p>
                                                                </div>
                                                                : null}
                                                            {application.listingStatus === "Rejected" ?
                                                                <div style={{ display: 'flex', flexDirection: 'column' }}>
                                                                    <p className="listing-tag">Status: ❌</p>
                                                                    <p className="listing-tag">Purpose: {application.rejectionPurpose}</p>
                                                                </div>
                                                                : null}
                                                            <div>
                                                                <a className="listing-tag-a" target="_blank" rel="noreferrer" href={application.cv}>See my CV</a>
                                                                <a className="listing-tag-a" target="_blank" rel="noreferrer" href={application.ml}>See my motivational letter</a>
                                                            </div>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    )
                                })}
                            </div>
                            : null}

                    </div> : null}
            </div>
        </div>
    )

}

export default MyProfile;