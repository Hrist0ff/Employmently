import React from 'react';
import { Nav } from 'react-bootstrap';
import '../styles/home.css';
import Logo from "../images/employment.png";
import jwt from 'jwt-decode';
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import Listings from '../components/Listings';
import { Link } from 'react-router-dom';


function Navbar() {
    const token = localStorage.getItem("accessToken");
    var id = 1;

    const logout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
    }

    const haveProfile = () => {
        if (token) {
            const decodedToken = jwt(token);
            let username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'];
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (role === 'Company') {
                return [
                    <Nav.Link key={0} disabled> Hello, {username}</Nav.Link>,
                    <Nav.Link key={1} href={`${process.env.REACT_APP_SERVER_PAGE}/MyProfile`}>My Profile</Nav.Link>,
                ]
            }
            return [
                <Nav.Link key={2} disabled> Hello, {username}</Nav.Link>,
                <Nav.Link key={3} href={`${process.env.REACT_APP_SERVER_PAGE}/MyProfile`}>My Profile</Nav.Link>,
                <Nav.Link key={4} onClick={logout}>Log Out</Nav.Link>

            ]
        }
        else {
            return [<Nav.Link key={5} href={`${process.env.REACT_APP_SERVER_PAGE}/Login`}>Вход</Nav.Link>,
            <Nav.Link key={6} href={`${process.env.REACT_APP_SERVER_PAGE}/Register`}>Създай акаунт</Nav.Link>,
            <span className="divider" />,
            <Nav.Link key={7} href={`${process.env.REACT_APP_SERVER_PAGE}/Listings`}>Обяви</Nav.Link>]
        }


    }

    const isAdmin = () => {
        if (token) {
            const decodedToken = jwt(token);
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (role === 'Administrator') {
                return <Nav.Link key={4} href={`${process.env.REACT_APP_SERVER_PAGE}/AdminPanel`}>Admin Panel</Nav.Link>;
            }
        }
    }

    const isCompany = () => {
        if (token) {
            const decodedToken = jwt(token);
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (role === 'Company') {
                return [
                    <Nav.Link key={5} href={`${process.env.REACT_APP_SERVER_PAGE}/myCompany`}>My Company</Nav.Link>,
                    <Nav.Link key={6} href={`${process.env.REACT_APP_SERVER_PAGE}/Listing`}>Create a listing</Nav.Link>,
                    <Nav.Link key={7} onClick={logout}>Log Out</Nav.Link>
                ]

            }
        }
    }

    return (
        <div key={id}>
            {haveProfile()}
            {isCompany()}
            {isAdmin()}
        </div>
    )

}

export default Navbar;