import React from 'react';
import { Nav } from 'react-bootstrap';
import '../styles/home.css';
import Logo from "../images/employment.png"

function Home() {
    return (
        <body>
            <div class="nav">
                <img src={Logo} className='log' alt='Employmently logo'></img>
                {/* Aligned items to the right */}
                <div class="justify-end">
                    <label for="toggle">&#9776;</label>
                    <input type="checkbox" id="toggle" />
                    <div class="menu">
                        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Login`}>Вход</Nav.Link>
                        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Register`}>Създай акаунт</Nav.Link>
                        <span class="divider" />
                        <Nav.Link href={`${process.env.REACT_APP_SERVER_PAGE}/Listings`}>Обяви</Nav.Link>

                    </div>
                </div>
            </div>
        </body>

    );
}

export default Home;