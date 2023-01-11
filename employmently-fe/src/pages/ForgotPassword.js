import React, { useState, useEffect } from 'react';
import '../styles/forgotpassword.css';
import FormInput from '../components/FormInput';
import axios from 'axios';
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";

function ForgotPassword() {

    const [values, setValues] = useState({
        email: "",
    });
    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

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
                setSuccessMessage("Check your email for a link to reset your password. If it doesn’t appear within a few minutes, check your spam folder.");

            })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })
    }
    return (
        <div className="fp-container">
            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="suc"> Success: {successMessage} </div>}
            <h1>Forgot password</h1>
            <p>Enter your email address and we'll </p>
            <p>send you a link to reset your password.</p>
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
    );
}

export default ForgotPassword;