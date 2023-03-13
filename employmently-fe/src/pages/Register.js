import React, { useEffect } from "react";
import { useState } from 'react';
import { Link } from 'react-router-dom'
import axios from 'axios';
import '../styles/login.css';
import FormInput from '../components/FormInput';
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Candidate from "../images/candidate.png";
import Company from "../images/company.png";
import Checkmark from "../images/checkmark.png";
import Handshake from "../images/handshake.png";
import { NotificationManager } from 'react-notifications';




function Register() {

    const [selected, setSelected] = useState(false);


    const [values, setValues] = useState({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
    });

    const [companyValues, setCompanyValues] = useState({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
        companyId: "",
    });


    const inputs = [
        {
            id: 1,
            name: "username",
            type: "text",
            placeholder: "Username",
            errorMessage:
                "Username should be 3-16 characters and shouldn't include any special character!",
            label: "Username",
            pattern: "^[A-Za-z0-9]{3,16}$",
            required: true,
        },
        {
            id: 2,
            name: "email",
            type: "email",
            placeholder: "Email",
            errorMessage: "It should be a valid email address!",
            label: "Email",
            required: true,
        },
        {
            id: 3,
            name: "password",
            type: "password",
            placeholder: "Password",
            errorMessage:
                "Password should be at least 6 characters and include at least 1 letter, 1 number and 1 special character!",
            label: "Password",
            pattern: `^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,20}$`,
            required: true,
        },
        {
            id: 4,
            name: "confirmPassword",
            type: "password",
            placeholder: "Confirm Password",
            errorMessage: "Passwords don't match!",
            label: "Confirm Password",
            pattern: values.password,
            required: true,
        },
    ];

    const inputsCompany = [
        {
            id: 1,
            name: "username",
            type: "text",
            placeholder: "Username",
            errorMessage:
                "Username should be 3-16 characters and shouldn't include any special character!",
            label: "Username",
            pattern: "^[A-Za-z0-9]{3,16}$",
            required: true,
        },
        {
            id: 2,
            name: "email",
            type: "email",
            placeholder: "Email",
            errorMessage: "It should be a valid email address!",
            label: "Email",
            required: true,
        },
        {
            id: 3,
            name: "password",
            type: "password",
            placeholder: "Password",
            errorMessage:
                "Password should be at least 6 characters and include at least 1 letter, 1 number and 1 special character!",
            label: "Password",
            pattern: `^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,20}$`,
            required: true,
        },
        {
            id: 4,
            name: "confirmPassword",
            type: "password",
            placeholder: "Confirm Password",
            errorMessage: "Passwords don't match!",
            label: "Confirm Password",
            pattern: companyValues.password,
            required: true,
        },
        {
            id: 5,
            name: "companyId",
            type: "text",
            placeholder: "Company ID",
            errorMessage: "Company ID should be 9 characters!",
            label: "Company ID",
            pattern: "^[0-9]{9}$",
            required: true,
        },
    ];

    useEffect(() => {
        if (selected) {
            setValues({
                username: "",
                email: "",
                password: "",
                confirmPassword: "",
            });
        }
        else {
            setCompanyValues({
                username: "",
                email: "",
                password: "",
                confirmPassword: "",
                companyId: "",
            });
        }
    }, [selected]);

    const showSuccessMessage = (message) => {
        NotificationManager.success(message, 'Success');
    }

    const showErrorMessage = (message) => {
        NotificationManager.error(message, 'Error');
    }


    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    const onChangeCompany = (e) => {
        setCompanyValues({ ...companyValues, [e.target.name]: e.target.value });
    };

    useEffect(() => {
        if (ExpiredTokenCheck() === false) {
            window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
        }
    })

    const registerAction = (event) => {
        event.preventDefault();

        if (values.length !== 0) {

            for (let key in values) {
                if (values[key] === "") {
                    showErrorMessage("Please fill in all the fields!");
                    return;
                }
            }

            const username = values.username;
            const email = values.email;
            const password = values.password;
            const confirmPassword = values.confirmPassword;


            axios.post(`${process.env.REACT_APP_BACKEND}/Register`, { username, email, password, confirmPassword })
                .then(response => {
                    const token = response.data;
                    localStorage.setItem("token", token);
                    axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                    showSuccessMessage("Account created successfully, check your email to confirm it.");
                    setTimeout(() => { window.location.href = "/Login" }, 3000);

                })
                .catch(error => {
                    showErrorMessage(error.response.data.Error[0]);
                })


        }
        else {
            const username = companyValues.username;
            const email = companyValues.email;
            const password = companyValues.password;
            const confirmPassword = companyValues.confirmPassword;
            const companyId = companyValues.companyId;

            axios.post(`${process.env.REACT_APP_BACKEND}/Register/Company`, { username, email, password, confirmPassword, companyId })
                .then(response => {
                    const token = response.data;
                    localStorage.setItem("token", token);
                    axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                    showSuccessMessage("Account created successfully, check your email to confirm it.");
                    setTimeout(() => { window.location.href = "/Login" }, 3000);

                })
                .catch(error => {
                    showErrorMessage(error.response.data.Error[0]);
                })
        }


    }

    return (
        <div className="background">
            <div className="container-register">
                <div className="log-container">
                    <p className="acc-type">Choose account type</p>
                    <div className="acc-type-div">
                        <div className="candidate-div">
                            <img src={Candidate} alt="Candidate"
                                className={`image-button ${selected ? 'selected' : ''}`}
                                onClick={() => setSelected(false)}>
                            </img>
                            <p className="paragraph-candidates">Candidate</p>
                            <img src={Checkmark} alt="Checkmark" className={`${selected ? 'no-checkmark' : 'checkmark'}`}></img>
                        </div>
                        <div className="candidate-div">
                            <img src={Company} alt="Company"
                                className={`image-button ${selected ? 'selected' : ''}`}
                                onClick={() => setSelected(true)}>
                            </img>
                            <p className="paragraph-candidates">Company</p>
                            <img src={Checkmark} alt="Checkmark" className={`${selected ? 'checkmark' : 'no-checkmark'}`}></img>

                        </div>
                    </div>
                    <h1>Sign up</h1>

                    <form className="form-signup">
                        {selected ?
                            inputsCompany.map((input) => (
                                <FormInput
                                    key={input.id}
                                    {...input}
                                    value={companyValues[input.name]}
                                    onChange={onChangeCompany}
                                />))
                            : inputs.map((input) => (
                                <FormInput
                                    key={input.id}
                                    {...input}
                                    value={values[input.name]}
                                    onChange={onChange}
                                />
                            ))}
                        {selected ?
                            <button disabled={!companyValues.username || !companyValues.email || !companyValues.password || !companyValues.confirmPassword
                                || companyValues.password !== companyValues.confirmPassword || !companyValues.companyId || companyValues.companyId.length !== 9}
                                className="btn" type="submit" onClick={registerAction}>Sign up</button>

                            :
                            <button disabled={!values.username || !values.email || !values.password || !values.confirmPassword
                                || values.password !== values.confirmPassword}
                                className="btn" type="submit" onClick={registerAction}>Sign up</button>
                        }
                    </form>

                </div>
                <div className="signup-container">
                    <img src={Handshake} alt="Handshaking" className="handshake-image"></img>
                    <p className="header-signup"> Find your next career opportunity at Employment.ly</p>
                    <p className="text-signup-reg">where job seekers and top employers connect</p>

                </div>

            </div>
        </div>
    );
};

export default Register;