import React from 'react';
import { Nav } from 'react-bootstrap';
import '../styles/home.css';
import Logo from "../images/employment.png";
import jwt from 'jwt-decode';

function Home() {
    const token = localStorage.getItem("token");
    const decodedToken = jwt(token);
    let role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    const haveProfile = () => {
        if (token) {
            return <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Login`}>My Profile</Nav.Link>;
        }
        return [<Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Login`}>Вход</Nav.Link>,
        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Register`}>Създай акаунт</Nav.Link>,
        <span class="divider" />,
        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Listings`}>Обяви</Nav.Link>]
    }

    const isCompany = () => {
        if (role === 'Company') {
            return <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Listing`}>Създай обява</Nav.Link>;
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
                        {haveProfile()}
                        {isCompany()}
                    </div>
                </div>
            </div>
        </body>

    );
}

export default Home;