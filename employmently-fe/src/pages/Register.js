import React, { useRef } from "react";
import { useState } from 'react';
import { Link } from 'react-router-dom'
import axios from 'axios';
import '../styles/login.css';
import FormInput from '../components/FormInput';





function Register() {

    const [values, setValues] = useState({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
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
            id: 4,
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
            id: 5,
            name: "confirmPassword",
            type: "password",
            placeholder: "Confirm Password",
            errorMessage: "Passwords don't match!",
            label: "Confirm Password",
            pattern: values.password,
            required: true,
        },
    ];

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");



    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    const registerAction = (event) => {
        event.preventDefault();
        const username = values.username;
        const email = values.email;
        const password = values.password;
        const confirmPassword = values.confirmPassword;


        axios.post(`${process.env.REACT_APP_BACKEND}/Register`, { username, email, password, confirmPassword })
            .then(response => {
                const token = response.data;
                localStorage.setItem("token", token);
                axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                setSuccessMessage("Account created successfully, check your email to confirm it.");
                setTimeout(() => { window.location.href = "/Login" }, 3000);

            })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })
    }

    return (
        <div>
            <div className="container">
                <div className="signup-container">
                    {errorMessage && <div className="err"> Error: {errorMessage} </div>}
                    {successMessage && <div className="suc"> Success: {successMessage} </div>}
                    <Link to={'/RegisterCompany'} className="btn-reg">Register a company?</Link>
                    <h1>Sign up</h1>
                    <form className="form-signup">
                        {inputs.map((input) => (
                            <FormInput
                                key={input.id}
                                {...input}
                                value={values[input.name]}
                                onChange={onChange}
                            />
                        ))}
                        <button className="btn" type="submit" onClick={registerAction}>Sign up</button>
                    </form>

                </div>
                <div className="log-container">
                    <h1>Sign in</h1>
                    <p>Sign in here if you already have an account.</p>
                    <p></p>
                    <p>
                        <Link to={'/Login'} className="btn">Sign in</Link>
                    </p>
                </div>

            </div>
        </div>
    );
};

export default Register;