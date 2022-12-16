import React, { useRef } from "react";
import { useState } from "react";
import { Link } from 'react-router-dom';
import axios from 'axios';
import '../styles/login.css';
import FormInput from '../components/FormInput';


function Login() {
    const [errorMessage, setErrorMessage] = React.useState("");

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

    const onChange = (e) => {
        setValues({ ...values, [e.target.name]: e.target.value });
    };

    const loginAction = (event) => {
        event.preventDefault();
        const email = values.email;
        const password = values.password;


        axios.post(`${process.env.REACT_APP_BACKEND}/Login`, { email, password })
            .then(response => {
                const token = response.data;
                localStorage.setItem("token", token);
                axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
                window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
            })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })
    }

    return (
        <div>
            <div className="container">
                <div className="log-container">
                    {errorMessage && <div className="err"> Error: {errorMessage} </div>}
                    <h1>Sign in</h1>
                    <form>
                        {inputs.map((input) => (
                            <FormInput
                                key={input.id}
                                {...input}
                                value={values[input.name]}
                                onChange={onChange}
                            />
                        ))}
                        <button className="btn" type="submit" onClick={loginAction}>Sign in</button>
                    </form>
                    <p>
                        <Link to={'/ForgotPassword'} className="btn-frgtpass">Forgot your Password?</Link>
                    </p>
                </div>
                <div className="signup-container">
                    <h1>Sign up</h1>
                    <p>Sign up here if you don't have account.</p>
                    <p></p>
                    <p>
                        <Link to={'/Register'} className="btn">Sign up</Link>
                    </p>
                </div>

            </div>
        </div>
    );
};

export default Login;