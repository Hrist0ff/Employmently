import React from 'react';
import jwt from 'jwt-decode';
import axios from 'axios';



function ExpiredTokenCheck() {
    const token = localStorage.getItem("accessToken");

    if (token) {
        const decodedToken = jwt(token);
        const stack = new Error().stack;
        let exp = decodedToken['exp'];
        let date = new Date();
        let current = date.getTime() / 1000;
        let refreshToken = localStorage.getItem("refreshToken");

        if (exp < current) {
            axios.post(`${process.env.REACT_APP_BACKEND}/Token/getNewAccessToken`, refreshToken, {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    localStorage.setItem("accessToken", response.data.accessToken);
                    return false;
                })
                .catch(error => {
                    localStorage.removeItem("accessToken");
                    console.log(error.response.data)
                    setTimeout(() => {
                        window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/Login`;
                    }, 1000);
                })
        }
        return false;
    }
}


export default ExpiredTokenCheck;