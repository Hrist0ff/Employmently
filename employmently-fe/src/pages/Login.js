import React from "react";
import { useState } from "react";
import { Link } from 'react-router-dom';
import axios from 'axios';
import '../styles/login.css';
import FormInput from '../components/FormInput';
import { useEffect } from "react";
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Logo from "../images/employmently_letters.png";
import JobPhoto from "../images/login_jobphoto.png";
import { NotificationManager } from 'react-notifications';

function Login() {
    const showErrorMessage = (message) => {
        NotificationManager.error(message, 'Error');
    }


    const [values, setValues] = useState({
        email: "",
        password: "",
    });



    const inputs = [
        {
            id: 1,
            name: "email",
            type: "email",
            placeholder: "Email",
            errorMessage: "It should be a valid email address!",
            label: "Email",
            required: true,
        },
        {
            id: 2,
            name: "password",
            type: "password",
            placeholder: "Password",
            errorMessage:
                "Password is required!",
            label: "Password",
            required: true,
        }
    ];

    const isFormValid = inputs.every(input => {
        if (input.required && !values[input.name]) {
            return false;
        }
        if (input.type === "email" && !values[input.name].match(/^\S+@\S+$/)) {
            return false;
        }
        if (input.type === "password" && (values[input.name].length < 6 || values[input.name].length > 20)) {
            return false;
        }
        return true;
    });

    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    useEffect(() => {
        if (localStorage.getItem("accessToken") !== null) {
            window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
        }
    })


    const loginAction = (event) => {
        event.preventDefault();
        const email = values.email;
        const password = values.password;



        axios.post(`${process.env.REACT_APP_BACKEND}/Login`, { email, password })
            .then(response => {
                const token = response.data;
                localStorage.setItem("accessToken", token["accessToken"]);
                localStorage.setItem("refreshToken", token["refreshToken"]);
                axios.defaults.headers.common["Authorization"] = `Bearer ${token["accessToken"]}`;
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
            })
            .catch(error => {
                showErrorMessage(error.response.data.Error[0]);
            })
    }

    return (
        <div className="background">
            <div className="container">
                {ExpiredTokenCheck()}
                <div className="log-container">
                    <img src={Logo} className="employmently-logo-letters" alt="Employment.ly logo"></img>
                    <br></br>
                    <br></br>
                    <h1>Welcome to Employment.ly</h1>
                    <br></br>
                    <br></br>
                    <form className="form-input">
                        {inputs.map((input) => (
                            <FormInput className="input-login"
                                key={input.id}
                                {...input}
                                value={values[input.name]}
                                onChange={onChange}
                            />
                        ))}
                        <Link to={'/ForgotPassword'} className="btn-frgtpass">Forgot your Password?</Link>
                        <button disabled={!isFormValid} className="btn" type="submit" onClick={loginAction}>Sign in</button>
                    </form>
                    <div className="div-or">
                        <p className="line-or">________</p>
                        <p>or</p>
                        <p className="line-or">________</p>
                    </div>
                    <br></br>

                    <div className="div-signup">
                        <p className="new">New to Employment.ly? </p>
                        <Link to={'/Register'} className="btn-signup">Create account</Link>
                    </div>
                </div>
                <div className="signup-container">
                    <img src={JobPhoto} alt="Job seeking" className="job-photo"></img>
                    <p className="header-signup">A bridge between job seekers and their future employers.</p>
                    <p className="text-signup">Our website serves as a pathway between job seekers and their ideal employers, offering the necessary tools and resources for successful career matches.</p>
                </div>
            </div>
        </div>
    );
};

export default Login;