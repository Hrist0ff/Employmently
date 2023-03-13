import { useParams } from "react-router-dom";
import { useState } from "react";
import React from "react";
import FormInput from "../components/FormInput";
import axios from 'axios';
import { NotificationManager } from 'react-notifications';

function ChangePassword() {
    let parameters = useParams();
    var id = parameters.id;
    var token = parameters.token;


    const [values, setValues] = useState({
        newPassword: "",
        confirmPassword: "",
    });

    const showSuccessMessage = (message) => {
        NotificationManager.success(message, 'Success');
    }

    const showErrorMessage = (message) => {
        NotificationManager.error(message, 'Error');
    }

    const forgotPassAction = (event) => {
        event.preventDefault();
        const userId = id;
        const newPassword = values.newPassword;



        axios.post(`${process.env.REACT_APP_BACKEND}/ForgotPassword/Changepassword`, { userId, token, newPassword })
            .then(response => {
                const token = response.data;
                localStorage.setItem("token", token);
                axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                showSuccessMessage("Password changed successfully, you can now login.");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Login`;
                }, 3000);
            })
            .catch(error => {
                showErrorMessage("Couldn't change password.");
            })
    }


    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };


    const inputs = [
        {
            id: 1,
            name: "newPassword",
            type: "password",
            placeholder: "New Password",
            errorMessage:
                "Password should be at least 6 characters and include at least 1 letter, 1 number and 1 special character!",
            label: "Password",
            pattern: `^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,20}$`,
            required: true,
        },
        {
            id: 2,
            name: "confirmPassword",
            type: "password",
            placeholder: "Confirm New Password",
            errorMessage: "Passwords don't match!",
            label: "Confirm Password",
            pattern: values.password,
            required: true,
        },
    ];

    return (
        <div className="fp-container">
            <h1>Forgot password</h1>
            <p>Enter your new Password</p>
            <form className='fp-form'>
                {inputs.map((input) => (
                    <FormInput
                        key={input.id}
                        {...input}
                        value={values[input.name]}
                        onChange={onChange}
                    />
                ))}
                <button className="btn" type="submit" onClick={forgotPassAction}>Change password</button>
            </form>
        </div>
    );

}

export default ChangePassword;