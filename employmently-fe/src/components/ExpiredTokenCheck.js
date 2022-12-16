import React from 'react';
import jwt from 'jwt-decode';


function ExpiredTokenCheck() {
    const token = localStorage.getItem("token");

    if (token) {
        const decodedToken = jwt(token);
        let exp = decodedToken['exp'];
        let date = new Date();
        let current = date.getTime() / 1000;
        console.log(current);
        console.log(exp);
        if (exp < current) {
            localStorage.removeItem("token");
            return window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
        }
    }
}

export default ExpiredTokenCheck;