import { useParams } from "react-router-dom";
import { useState } from "react";
import React from "react";
import FormInput from "../components/FormInput";
import axios from 'axios';

function ChangePassword() {
    let parameters = useParams();
    var id = parameters.id;


    const [values, setValues] = useState({
        oldPassword: "",
        newPassword: "",
        confirmPassword: "",
    });

    const [errorMessage, setErrorMessage] = React.useState("");
    const [successMessage, setSuccessMessage] = React.useState("");

    const forgotPassAction = (event) => {
        event.preventDefault();
        const oldPassword = values.oldPassword;
        const newPassword = values.newPassword;


        axios.post(`${process.env.REACT_APP_BACKEND}/ForgotPassword/Changepassword?id=${id}`, { oldPassword, newPassword })
            .then(response => {
                const token = response.data;
                localStorage.setItem("token", token);
                axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                setSuccessMessage("Password changed successfully, you can now login.");
                setTimeout(() => {
                    window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Login`;
                }, 3000);
            })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })
    }


    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };


    const inputs = [
        {
            id: 1,
            name: "oldPassword",
            type: "password",
            placeholder: "Old Password",
            label: "Password",
            required: true,
        },
        {
            id: 2,
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
            id: 3,
            name: "confirmPassword",
            type: "password",
            placeholder: "Confirm New Password",
            errorMessage: "Passwords don't match!",
            label: "Confirm Password",
            pattern: values.password,
            required: true,
        },
    ];
    console.log(values)

    return (
        <div className="fp-container">
            {errorMessage && <div className="err"> Error: {errorMessage} </div>}
            {successMessage && <div className="suc"> Success: {successMessage} </div>}
            <h1>Forgot password</h1>
            <p>Enter your Old and new Passwords</p>
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