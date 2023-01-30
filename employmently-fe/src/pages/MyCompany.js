import React, { useEffect, useState } from 'react';
import jwt from 'jwt-decode';
import axios from 'axios';
import FormInput from '../components/FormInput';
import { Nav } from "react-bootstrap";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import { FileUploader } from "react-drag-drop-files";
import Navbar from '../components/Navbar';
import CompanyListings from '../components/CompanyListings';
import Logo from "../images/employment.png";
import { Link } from 'react-router-dom';





function MyCompany() {
    const token = localStorage.getItem("accessToken");

    const [user, setUser] = React.useState({});
    const [performed, setPerformed] = React.useState(false);
    const [selectedYear, setSelectedYear] = useState(null);


    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    // Inputs for company info
    const [photoInput, setPhotoInput] = React.useState(false);
    const [phoneInput, setPhoneInput] = React.useState(false);
    const [yearInput, setYearInput] = React.useState(false);
    const [employeesInput, setEmployeesInput] = React.useState(false);
    const [descriptionInput, setDescriptionInput] = React.useState(false);

    const [employeeCount, setEmployeeCount] = useState('');


    // File upload 
    const [file, setFile] = useState(null);
    const fileTypes = ["PNG"];

    const handleChange = (file) => {
        setFile(file);
    };

    // Year handling
    const yearOptions = [];
    for (let year = 1970; year <= 2023; year++) {
        yearOptions.push(<option key={year} value={year}>{year}</option>);
    }

    const handleChangeYear = (event) => {
        setSelectedYear(event.target.value);
    };



    // Inputs handling
    const [values, setValues] = useState({
        yearCreated: "",
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



    const handleChangeEmployees = (event) => {
        setEmployeeCount(event.target.value);
    };

    //Year change event
    const onYearUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        axios.put(`${process.env.REACT_APP_BACKEND}/Company/changeYearOfCreation/${id}`, selectedYear, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setSuccessMessage("Year of creation changed successfully!");

                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyCompany`;
                }, 4000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change year. ");
            })

    }
    // Picture change event
    const onPhotoUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        const formData = new FormData();
        formData.append('image', file);
        console.log(file);


        axios.post(`${process.env.REACT_APP_BACKEND}/Company/uploadPic/${id}`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                "Authorization": `Bearer ${token}`,
                "Expires": "0"
            }
        })
            .then(response => {
                setSuccessMessage("Profile picture changed successfully!");

                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyCompany`;
                }, 4000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change picture. ");
            })
    }

    // Employees quantity change event
    const onEmployeesUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const employees = employeeCount;

        axios.put(`${process.env.REACT_APP_BACKEND}/Company/changeEmployees/${id}`, employees, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setEmployeesInput(false);
                setSuccessMessage("Employees quantity changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyCompany`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change employees quantity. ");
            })
    }

    // not used
    const onPhoneUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const phoneNumber = values.phoneNumber;


        axios.put(`${process.env.REACT_APP_BACKEND}/Company/changePhoneNumber/${id}`, phoneNumber, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setPhoneInput(false);
                setSuccessMessage("Phone number changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyCompany`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change phone number. ");
            })
    }

    // Description change event
    const onDescriptionUpload = (event) => {
        event.preventDefault();
        const decodedToken = jwt(token);
        let id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const description = values.description;


        axios.put(`${process.env.REACT_APP_BACKEND}/Company/changeDescription/${id}`, description, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            }
        })
            .then(response => {
                setDescriptionInput(false);
                setSuccessMessage("Description changed successfully!");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/MyCompany`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage("Couldn't change description. ");
            })
    }



    // Checking account and getting user data
    useEffect(() => {
        if (token) {
            if (!performed) {
                setEmployeeCount("1-5");
                setSelectedYear("1970");
                const decodedToken = jwt(token);
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                axios.get(`${process.env.REACT_APP_BACKEND}/Company/${userId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                })
                    .then(response => {
                        setUser(response.data);

                        console.log(response.data);

                    })
                    .catch(error => {
                        setErrorMessage(error.response.data.Error[0]);
                    })
                setPerformed(true);
            }
        }
    }, [token, performed]);

    // return (
    //     < div className={`my-profile-comp ${(descriptionInput || phoneInput) ? 'my-profile-comp--expanded' : ''}`}>
    //         {errorMessage && <div className="err"> Error: {errorMessage} </div>}
    //         {successMessage && <div className="sucMessage"> Success: {successMessage} </div>}
    //         {ExpiredTokenCheck()}
    //         <h1 className="heading">My company</h1>

    //         <br></br>
    //         <div className="profile-header">
    //             <img src={user.profilePicture} className="profile-picture" alt="Profile pic"></img>
    //             {!photoInput && <button className="upload-btn" onClick={() => setPhotoInput(true)}>Edit Photo</button>}
    //             {photoInput && (
    //                 <div className="profile-header">
    //                     <FileUploader
    //                         handleChange={handleChange}
    //                         name="file"
    //                         types={fileTypes}
    //                     />
    //                     <button onClick={onPhotoUpload} className="upload-btn">Upload photo</button>
    //                 </div>
    //             )}
    //         </div>
    //         <div className="profile-details">

    //             <div style={{ display: 'flex', flexWrap: 'nowrap', gap: '150px' }}>
    //                 <div style={{ display: 'flex', flexDirection: 'column' }}>
    //                     <p><strong>USERNAME &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>    {user.name}</p>
    //                     <p><strong>YEAR CREATED &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong>    {user.yearCreated}</p>

    //                 </div>
    //                 <div style={{ display: 'flex', flexDirection: 'column' }}>
    //                     <p><strong>EIK &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>    {user.uniqueIdentifier}</p>
    //                     <p><strong>EMPLOYEES &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.employees ? user.employees : "0"} employees</p>
    //                 </div>
    //             </div>
    //             <div style={{ display: 'flex', gap: '370px' }}>
    // {!yearInput && <button className="edit-btn" onClick={() => setYearInput(true)}>Edit year of creation</button>}
    // {yearInput &&
    //     (
    //         <div className="profile-header">
    //             <select id="year-dropdown" onChange={handleChangeYear} value={selectedYear} style={{ width: 200, height: 20 }}>
    //                 {yearOptions}
    //             </select>
    //             <button onClick={onYearUpload} className="upload-ph-btn">Update year</button>
    //         </div>)
    // }
    // {!employeesInput && <button className="edit-btn" onClick={() => setEmployeesInput(true)}>Edit employees</button>}
    // {employeesInput &&
    //     (
    //         <div className="profile-header">
    //             <select style={{ width: 200 }} value={employeeCount} onChange={handleChangeEmployees}>
    //                 <option value="1-5">1-5</option>
    //                 <option value="5-15">5-15</option>
    //                 <option value="15-30">15-30</option>
    //                 <option value="30-50">30-50</option>
    //                 <option value="50-100">50-100</option>
    //                 <option value="100-300">100-300</option>
    //                 <option value="300+">300+</option>
    //             </select>
    //             <button onClick={onEmployeesUpload} className="upload-ph-btn">Update employees</button>
    //         </div>)
    // }
    //             </div>

    //             <div>
    //                 <p><strong>PHONE NUMBER &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.phoneNumber}</p>
    // {!phoneInput && <button className="edit-btn" onClick={() => setPhoneInput(true)}>Edit Phone</button>}
    // {phoneInput && (
    //     <div className="profile-header">
    //         <form>
    //             <FormInput
    //                 key={inputs[0].id}
    //                 {...inputs[0]}
    //                 value={values[inputs[0].name]}
    //                 onChange={onChange}
    //             />
    //             <button onClick={onPhoneUpload} className="upload-ph-btn">Update phone number</button>
    //         </form>

    //     </div>
    // )}
    //             </div>
    //             <p><strong>DESCRIPTION &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>{user.description}</p>
    // {!descriptionInput && <button className="edit-btn" onClick={() => setDescriptionInput(true)}>Edit Description</button>}
    // {descriptionInput && (
    //     <div className="profile-header">
    //         <form>
    //             <FormInput
    //                 key={inputs[1].id}
    //                 {...inputs[1]}
    //                 value={values[inputs[1].name]}
    //                 onChange={onChange}
    //             />

    //             <button onClick={onDescriptionUpload} className="upload-ph-btn-right">Update description</button>
    //         </form>

    //     </div>
    // )}
    //         </div>
    //         <Nav.Link className="back-but-comp" href={`${process.env.REACT_APP_SERVER_PAGE}/`}>Back</Nav.Link>
    //     </div>
    // )
    console.log(user.companyId)

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
                    {errorMessage && <div className="err"> Error: {errorMessage} </div>}
                    {successMessage && <div className="sucMessage"> Success: {successMessage} </div>}
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
                                {!descriptionInput && <button className="company-edit-button" onClick={() => setDescriptionInput(true)}>Edit Description</button>}
                                {descriptionInput && (
                                    <div className="profile-header">
                                        <form className='company-form'>
                                            <FormInput
                                                key={inputs[1].id}
                                                {...inputs[1]}
                                                value={values[inputs[1].name]}
                                                onChange={onChange}
                                            />

                                            <button onClick={onDescriptionUpload} className="company-button">Update description</button>
                                        </form>

                                    </div>
                                )}
                            </div>
                            <div className="technologies-list-mycomp">
                                <div className="company-row">
                                    <p className="tag">Year created: {user.yearCreated}</p>
                                    {!yearInput && <button className="company-edit-button-year" onClick={() => setYearInput(true)}>Edit year of creation</button>}
                                    {yearInput &&
                                        (
                                            <div className="profile-header">
                                                <select id="year-dropdown" onChange={handleChangeYear} value={selectedYear} style={{ width: 140, height: 40, marginTop: '15px' }}>
                                                    {yearOptions}
                                                </select>
                                                <button onClick={onYearUpload} className="company-button-year">Update year</button>
                                            </div>)
                                    }
                                </div>

                                <div className="company-row">
                                    <p className="tag">Number of employees: {user.employees}</p>
                                    {!employeesInput && <button className="company-edit-button-emp" onClick={() => setEmployeesInput(true)}>Edit employees</button>}
                                    {employeesInput &&
                                        (
                                            <div className="profile-header">
                                                <div className='select'>
                                                    <select value={employeeCount} onChange={handleChangeEmployees}>
                                                        <option value="1-5">1-5</option>
                                                        <option value="5-15">5-15</option>
                                                        <option value="15-30">15-30</option>
                                                        <option value="30-50">30-50</option>
                                                        <option value="50-100">50-100</option>
                                                        <option value="100-300">100-300</option>
                                                        <option value="300+">300+</option>
                                                    </select>
                                                </div>
                                                <button onClick={onEmployeesUpload} className="company-button">Update employees</button>
                                            </div>)
                                    }
                                </div>

                                <p className="tag-ui" > Unique Identifier: {user.uniqueIdentifier}</p>

                                <div className="company-row">
                                    <p className="tag">Phone number: {user.phoneNumber}</p>
                                    {!phoneInput && <button className="company-edit-button-emp" onClick={() => setPhoneInput(true)}>Edit Phone</button>}
                                    {phoneInput && (
                                        <div className="profile-header">
                                            <form className="phone-form">
                                                <FormInput
                                                    key={inputs[0].id}
                                                    {...inputs[0]}
                                                    value={values[inputs[0].name]}
                                                    onChange={onChange}
                                                />

                                            </form>
                                            <button onClick={onPhoneUpload} className="company-button-ph">Update phone number</button>
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>
                        <div className="company-technologies">
                            <div className="company-for-heading">
                                <p className="company-for-text">Technologies</p>
                            </div>
                            <p className="company-technologies-text">Technologies that we are using:</p>
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
                        {user && user.companyId ? <CompanyListings companyId={user.companyId} /> : null}

                    </div>
                </div>
            </div>
        </div >
    )
}

export default MyCompany;