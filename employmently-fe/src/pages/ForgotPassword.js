import React, { useState, useEffect } from 'react';
import '../styles/forgotpassword.css';
import FormInput from '../components/FormInput';
import axios from 'axios';
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Logo from "../images/employmently_letters.png";
import { NotificationManager } from 'react-notifications';

function ForgotPassword() {

    const [values, setValues] = useState({
        email: "",
    });

    const showSuccessMessage = (message) => {
        NotificationManager.success(message, 'Success');
    }

    const showErrorMessage = (message) => {
        NotificationManager.error(message, 'Error');
    }

    const inputs = [
        {
            id: 1,
            name: "email",
            type: "email",
            placeholder: "Email",
            errorMessage: "It should be a valid email address!",
            label: "Email",
            required: true,
        }
    ];

    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    useEffect(() => {
        if (ExpiredTokenCheck() === false) {
            window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
        }
    })

    const ForgotPassAction = (event) => {
        event.preventDefault();
        const email = values.email;


        axios.post(`${process.env.REACT_APP_BACKEND}/ForgotPassword`, { email })
            .then(response => {
                const token = response.data;
                localStorage.setItem("token", token);
                axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                showSuccessMessage("Check your email for a link to reset your password. If it doesn’t appear within a few minutes, check your spam folder.");
            })
            .catch(error => {
                showErrorMessage("There is no account associated with this email address.");
            })
    }
    return (
        <div className="background">
            <div className="fp-container">
                <img src={Logo} className="employmently-logo-letters" alt="Employment.ly logo"></img>
                <br></br>
                <br></br>
                <p className="heading-fp">Forgot password</p>
                <p style={{ "font-size": "13px" }}>Enter your email address and we'll send you a link to reset your password.</p>
                <form className='fp-form'>
                    {inputs.map((input) => (
                        <FormInput
                            key={input.id}
                            {...input}
                            value={values[input.name]}
                            onChange={onChange}
                        />
                    ))}
                    <button className="btn" type="submit" onClick={ForgotPassAction}>Email Reset</button>
                </form>
            </div>
        </div>
    );
}

export default ForgotPassword;