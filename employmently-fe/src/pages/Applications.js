import React, { useEffect } from "react";
import jwt from "jwt-decode";
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import axios from "axios";
import Navbar from "../components/Navbar";
import { Link } from "react-router-dom";
import Logo from "../images/employment.png";
import Remote from "../images/remote.png";
import format from 'date-fns/format';
import { isToday, isYesterday } from 'date-fns';
import "../styles/applications.css";
import { NotificationManager } from 'react-notifications';

function Applications() {
    const token = localStorage.getItem("accessToken");
    const [performed, setPerformed] = React.useState(false);


    const [applications, setApplications] = React.useState([]);

    const showSuccessMessage = (message) => {
        NotificationManager.success(message, 'Success');
    }

    const showErrorMessage = (message) => {
        NotificationManager.error(message, 'Error');
    }


    const [isAccepted, setIsAccepted] = React.useState(0);
    const [isRejected, setIsRejected] = React.useState(0);

    const [values, setValues] = React.useState({
        rejectionPurpose: "",
        suggestedInterviewDate: new Date().toISOString().substr(0, 16)
    });


    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };


    useEffect(() => {
        if (token) {
            if (!performed) {
                const decodedToken = jwt(token);
                const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                ExpiredTokenCheck();
                if (role !== "Company") {
                    showErrorMessage("You do not have permission to watch the applications!");
                    setTimeout(() => {
                        window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
                    }, 0);
                }

                axios.get(`${process.env.REACT_APP_BACKEND}/Company/getApplicationsToCompany`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'accept': 'application/json'
                    }

                }).then(response => {
                    const applications = response.data.map(application => {
                        application.applicationTime = new Date(application.applicationTime);

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
                        showErrorMessage(error.response.data.Error[0]);
                    });



                setPerformed(true);
            }
        }
        else {
            setTimeout(() => {
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
            }, 0);
        }
    }, [token, performed]);

    const rejectListing = (id) => {
        const token = localStorage.getItem("accessToken");

        const rejectionPurpose = values.rejectionPurpose;

        axios.post(`${process.env.REACT_APP_BACKEND}/Company/rejectApplication/${id}`, rejectionPurpose,
            {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'

                }
            })
            .then(response => {
                showSuccessMessage("Listing rejected!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Applications`;
                }, 2000);


            })
            .catch(error => {
                showErrorMessage("Couldn't reject listing.");
            })
    }

    const acceptListing = (id) => {
        const token = localStorage.getItem("accessToken");

        let date = new Date(values.suggestedInterviewDate);
        let DateForInterview = date.toISOString();



        axios.post(`${process.env.REACT_APP_BACKEND}/Company/acceptApplication/${id}`, DateForInterview,
            {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
            .then(response => {
                showSuccessMessage("Listing accepted!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Applications`;
                }, 3000);
            })
            .catch(error => {
                showErrorMessage("Couldn't accept listing.");
            })
    }

    return (
        <div className="background">
            <div className='home-page'>
                <div className="nav">
                    <Link to="/">
                        <img src={Logo} className='log' alt='Employmently logo'></img>
                    </Link>
                    {/* Aligned items to the right */}
                    <div className="justify-end">
                        <label htmlFor="toggle">&#9776;</label>
                        <input type="checkbox" id="toggle" />
                        <div className="menu" >
                            {ExpiredTokenCheck()}
                            {Navbar()}
                        </div>
                    </div>
                </div>
            </div>
            <div >
                {applications ? applications.map((listing, idx) => {
                    return (
                        <div style={{ paddingTop: '20px' }}>
                            <div className="listing-section-home">
                                <div className="listing-container">
                                    <div className="listing-item">
                                        <a className="listing-a-div" href={`${process.env.REACT_APP_SERVER_PAGE}/Profile/${listing.userId}`}>
                                            <div>
                                                <img className="listing-company-logo" src={listing.userPic} alt="Company logo"></img>
                                                <p className="listing-company-name-text">{listing.userName}</p>
                                            </div>
                                        </a>
                                        <div className="applications-listing-a">
                                            <div className="applications-listing-header">
                                                <p className="listing-title-text">{listing.listingName}</p>
                                                <p className="applications-listing-date">📅{listing.applicationTime}</p>
                                            </div>
                                            <div className="listing-categories-tags-div">
                                                {listing.listingArrangement && listing.listingArrangement === "Remote" ?
                                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}><img src={Remote} alt="Remote" className="listing-tag-image"></img> &nbsp;{listing.listingArrangement}</p>
                                                    : null}
                                                {listing.listingArrangement && listing.listingArrangement === "On-site" ?
                                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💼 {listing.listingArrangement}</p>
                                                    : null}
                                                {listing.listingArrangement && listing.listingArrangement === "Hybrid" ?
                                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%', alignItems: 'center' }}><img src={Remote} alt="Remote" className="listing-tag-image-hybrid"></img> &nbsp; / 💼 {listing.listingArrangement}</p>
                                                    : null}
                                                {listing.salary ? <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💰 {listing.salary} lv.</p> : null}
                                                <div className="listing-categories-map">
                                                    {listing.listingCategories && listing.listingCategories.map((category, index) => {
                                                        return (
                                                            <p className="listing-category-view" key={index}>{category}</p>
                                                        )
                                                    })}
                                                </div>

                                            </div>
                                        </div>
                                        <div className="applications-buttons-and-info">
                                            <a className="listing-tag-a" target="_blank" rel="noreferrer" href={`${process.env.REACT_APP_SERVER_PAGE}/Profile/${listing.userId}`}   >See Profile</a>
                                            <a className="listing-tag-a" target="_blank" rel="noreferrer" href={listing.cv}>See CV</a>
                                            <a className="listing-tag-a" target="_blank" rel="noreferrer" href={listing.motivationalLetter}>See motivational letter</a>
                                            {!(isAccepted === listing.id) ? <button className="listing-tag-accept" onClick={() => setIsAccepted(listing.id)}>Accept</button>
                                                :
                                                <div className="applications-half">
                                                    <input type="datetime-local" className="listing-tag-accept" value={values.suggestedInterviewDate} name="suggestedInterviewDate" onChange={onChange}></input>
                                                    <button className="listing-tag-accept" onClick={() => acceptListing(listing.id)}>Accept</button>
                                                </div>
                                            }
                                            {!(isRejected === listing.id) ? <button className="listing-tag-reject" onClick={() => setIsRejected(listing.id)}>Reject</button>
                                                :
                                                <div className="applications-half">
                                                    <textarea className="listing-tag-reject" value={values.rejectionPurpose} name="rejectionPurpose" placeholder="Rejection purpose" onChange={onChange}></textarea>
                                                    <button className="listing-tag-reject" onClick={() => rejectListing(listing.id)}>Reject</button>
                                                </div>
                                            }
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )
                }
                ) : null}
            </div >
        </div>

    )

}

export default Applications;