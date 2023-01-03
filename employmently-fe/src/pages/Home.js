import React from 'react';
import { Nav, NavLink, Navbar } from 'react-bootstrap';
import '../styles/home.css';
import Logo from "../images/employment.png";
import jwt from 'jwt-decode';
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';

function Home() {
    const token = localStorage.getItem("token");

    const logout = () => {
        localStorage.removeItem("token");
        window.location.href = `${process.env.REACT_APP_SERVER_PAGE}/`;
    }

    const haveProfile = () => {
        if (token) {
            const decodedToken = jwt(token);
            let username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'];
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (role === 'Company') {
                return [
                    <Nav.Link disabled> Hello, {username}</Nav.Link>,
                    <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/MyProfile`}>My Profile</Nav.Link>,
                ]
            }
            return [
                <Nav.Link disabled> Hello, {username}</Nav.Link>,
                <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/MyProfile`}>My Profile</Nav.Link>,
                <Nav.Link onClick={logout}>Log Out</Nav.Link>

            ]
        }
        return [<Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Login`}>Вход</Nav.Link>,
        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Register`}>Създай акаунт</Nav.Link>,
        <span className="divider" />,
        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Listings`}>Обяви</Nav.Link>]
    }

    const isCompany = () => {
        if (token) {
            const decodedToken = jwt(token);
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (role === 'Company') {
                return [
                    <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/myCompany`}>My Company</Nav.Link>,
                    <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Listing`}>Създай обява</Nav.Link>,
                    <Nav.Link onClick={logout}>Log Out</Nav.Link>
                ]

            }
        }
    }

    const isAdmin = () => {
        if (token) {
            const decodedToken = jwt(token);
            let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            if (role === 'Administrator') {
                return <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Listing`}>Admin Panel</Nav.Link>;
            }
        }
    }

    return (
        <body>
            <div class="nav">
                <img src={Logo} className='log' alt='Employmently logo'></img>
                {/* Aligned items to the right */}
                <div class="justify-end">
                    <label for="toggle">&#9776;</label>
                    <input type="checkbox" id="toggle" />
                    <div class="menu">
                        {ExpiredTokenCheck()}
                        {haveProfile()}
                        {isCompany()}
                        {isAdmin()}
                    </div>
                </div>
            </div>
        </body>

    );
}

export default Home;