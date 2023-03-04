import axios from "axios";
import React, { useEffect, useState } from "react";
import jwt from "jwt-decode";
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Navbar from "../components/Navbar";
import Logo from "../images/employment.png";
import { Link } from "react-router-dom";
import '../styles/createlisting.css';

function CreateListing() {
    const token = localStorage.getItem("accessToken");
    const [performed, setPerformed] = React.useState(false);

    const [company, setCompany] = React.useState({});

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    const [values, setValues] = useState({
        title: "",
        description: "",
        tags: [],
        salary: 0,
        location: "",
        arrangement: "",
    });

    const handleChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    const selectedTags = (newTags) => {
        setValues({ ...values, tags: [...newTags] });
    };


    const TagsInput = props => {
        const [tags, setTags] = React.useState(props.tags);
        const removeTags = indexToRemove => {
            setTags([...tags.filter((_, index) => index !== indexToRemove)]);
        };
        const addTags = event => {
            if (event.target.value !== "") {
                setTags([...tags, event.target.value]);
                props.selectedTags([...tags, event.target.value]);
                event.target.value = "";
            }
        };

        return (
            <div className="tags-input">
                <ul id="tags">
                    {tags.map((tag, index) => (
                        <li key={index} className="tag-input">
                            <span className='tag-title'>{tag}</span>
                            <span className='tag-close-icon'
                                onClick={() => removeTags(index)}
                            >
                                x
                            </span>
                        </li>
                    ))}
                </ul>

                <input
                    type="text"
                    onKeyUp={event => event.key === "Enter" ? addTags(event) : null}
                    placeholder="Press enter to add tags"
                />
            </div>
        );
    };

    const createListingAction = (event) => {
        event.preventDefault();

        const name = values.title;
        const description = values.description;
        const categories = values.tags;
        const salary = values.salary;
        const location = values.location;
        const arrangement = values.arrangement;

        axios.post(`${process.env.REACT_APP_BACKEND}/Listing`, { name, description, categories, salary, location, arrangement }, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        }
        )
            .then(response => {
                setSuccessMessage(response.data);

                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyCompany`;
                }, 4000);
            })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })
    }

    // Checking account and getting user data
    useEffect(() => {
        if (token) {
            if (!performed) {
                const decodedToken = jwt(token);
                const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                ExpiredTokenCheck();
                if (role !== "Company") {
                    setErrorMessage("You do not have permission to create a listing!");
                    setTimeout(() => {
                        window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
                    }, 0);
                }
                axios.get(`${process.env.REACT_APP_BACKEND}/Company/${userId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                })
                    .then(response => {
                        setCompany(response.data);
                    })
                    .catch(error => {
                        setErrorMessage(error.response.data.Error[0]);
                    })
                setPerformed(true);
            }
        }
        else {
            setTimeout(() => {
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
            }, 0);
        }
    }, [token, performed]);

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

            <div className="create-listing-container">
                <div className="create-listing-div">

                    <div className="create-listing">
                        {errorMessage && <div className="err"> Error: {errorMessage} </div>}
                        {successMessage && <div className="sucMessage-cl"> Success: {successMessage} </div>}
                        <div className="create-listing-for-heading">
                            <p className="company-for-text-cl">Create a listing</p>
                        </div>
                        <div className="create-listing-margined-input">
                            <div className="create-listing-horizontal-info">
                                <p>Your account </p>
                                <p>You are currently signed in as <b>{company.name}</b></p>
                            </div>
                            <div className="create-listing-horizontal-info">
                                <p>Job Title</p>
                                <input
                                    className="job-title-input"
                                    type="text" placeholder="Job Title"
                                    value={values["title"]}
                                    onChange={handleChange}
                                    name="title"
                                />
                            </div>
                            <div className="create-listing-horizontal-info">
                                <p>Job Categories</p>
                                <TagsInput selectedTags={selectedTags}
                                    tags={values.tags}
                                    value={values.tags}
                                    onChange={handleChange}
                                    name="tags"
                                />
                            </div>
                            <div className="create-listing-horizontal-info">
                                <p>Job Description (optional)</p>
                                <textarea
                                    className="job-description-input"
                                    rows={8}
                                    cols={50}
                                    style={{
                                        padding: '12px 25px',
                                        margin: "10px 8px"
                                    }}
                                    placeholder="Enter your description here..."
                                    value={values.description}
                                    onChange={handleChange}
                                    name="description"
                                />
                            </div>
                            <div className="create-listing-horizontal-info">
                                <p>Salary (optional)</p>
                                <input
                                    className="job-title-input"
                                    type="number"
                                    placeholder="Salary"
                                    value={values.salary}
                                    onChange={handleChange}
                                    name="salary"
                                />

                            </div>

                            <div className="create-listing-horizontal-info">
                                <p>Arrangement (optional)</p>
                                <div className='select'>
                                    <select className="select-cl"
                                        name="arrangement"
                                        value={values.arrangement}
                                        onChange={handleChange}>
                                        <option value="Remote">Remote</option>
                                        <option value="On-site">On-site</option>
                                        <option value="Hybrid">Hybrid</option>
                                    </select>
                                </div>

                            </div>
                            <div className="create-listing-horizontal-info">
                                <p>Location (optional)</p>
                                <input
                                    className="job-title-input"
                                    type="text"
                                    placeholder="e.g. Sofia"
                                    value={values.location}
                                    onChange={handleChange}
                                    name="location"
                                />
                            </div>
                            <div className="create-listing-horizontal-info">
                                <button className="create-listing-btn" onClick={createListingAction}>Create job listing</button>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    )
}

export default CreateListing;