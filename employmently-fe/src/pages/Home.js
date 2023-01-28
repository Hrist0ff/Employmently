import React from 'react';
import '../styles/home.css';
import Logo from "../images/employment.png";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import Listings from '../components/Listings';
import Navbar from '../components/Navbar';

function Home() {

    return (
        <div className='home-page'>
            <div className="nav">
                <img src={Logo} className='log' alt='Employmently logo'></img>
                {/* Aligned items to the right */}
                <div className="justify-end">
                    <label htmlFor="toggle">&#9776;</label>
                    <input type="checkbox" id="toggle" />
                    <div className="menu" >
                        {ExpiredTokenCheck()}
                        {Navbar()}
                    </div>
                </div>
            </div>
            {Listings()}
        </div>
    );
}

export default Home;