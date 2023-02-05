import React, { useEffect, useState } from "react";
import jwt from 'jwt-decode';
import axios from 'axios';
import '../styles/myprofile.css';
import { FileUploader } from "react-drag-drop-files";
import FormInput from '../components/FormInput';
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Navbar from "../components/Navbar";
import { Link, useParams } from "react-router-dom";
import Logo from "../images/employment.png";
import Remote from "../images/remote.png";
import format from 'date-fns/format';
import { isToday, isYesterday, isTomorrow } from 'date-fns';

function Profile() {
    const [performed, setPerformed] = React.useState(false);

    const userId = useParams().id;
    const [user, setUser] = React.useState({});


    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    useEffect(() => {
        if (!performed) {
            axios.get(`${process.env.REACT_APP_BACKEND}/Information/getProfile/${userId}`)
                .then(response => {
                    console.log(response.data);
                    setUser(response.data);

                })
                .catch(error => {
                    setErrorMessage(error.response.data);
                    if (error.response.status === 401) {
                        ExpiredTokenCheck()
                    }
                })

            setPerformed(true);
        }
    }, [performed]);


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
                {errorMessage && <div className="err"> Error: {errorMessage} </div>}
                {successMessage && <div className="sucMessage"> Success: {successMessage} </div>}
                <div className="user-info">
                    <img src={user.profilePicture} alt="User profile" className="user-pic"></img>
                    <div className="user-main-info">
                        <div className="user-name-and-divider">
                            <span className="company-divider" />
                            <p className="company-heading">{user.name}</p>
                        </div>

                    </div>
                </div>
                <div className="company-for-and-technologies">
                    <div className="user-for">
                        <div className="company-for-heading">
                            <p className="company-for-text">About me</p>
                        </div>
                        <div>
                            <p>{user.description}</p>

                        </div>
                        <div className="technologies-list-mycomp">

                            <div className="company-row">
                                <p className="tag">Phone number: {user.phoneNumber}</p>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )

}

export default Profile;