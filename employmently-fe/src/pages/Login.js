import React, { useRef } from "react";
import { Link } from 'react-router-dom'
import axios from 'axios';
import '../styles/login.css';


function Login() {
    const emailField = useRef();
    const passwordField = useRef();

    const loginAction = (event) => {
        event.preventDefault();
        const email = emailField.current.value;
        const password = passwordField.current.value;


        axios.post(`${process.env.REACT_APP_BACKEND}/Login`, { email, password })
            .then(response => {
                const token = response.data;
                console.log(token);
            })
            .catch(error => {
                console.log(error)
            })
    }

    return (
        <div className="container">
            <div className="log-container">
                <h1>Sign in</h1>
                <p>fb   google</p>
                <form>
                    <div>
                        <input
                            className="form-control"
                            type="email"
                            placeholder="Enter email"
                            name="email"
                            ref={emailField}
                            required
                        />
                    </div>
                    <div>
                        <input
                            className="form-control"
                            type="password"
                            placeholder="Enter password"
                            name="password"
                            minLength='8'
                            ref={passwordField}
                            required
                        />
                    </div>
                    <button className="btn" type="submit" onClick={loginAction}>Sign in</button>
                </form>
                <p>
                    <Link to={''} className="btn-frgtpass">Forgot your Password?</Link>
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
    );
};

export default Login;