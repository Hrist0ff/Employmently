import React from 'react';
import '../styles/home.css';
import Logo from "../images/employment.png";
import ExpiredTokenCheck from '../components/ExpiredTokenCheck';
import Listings from '../components/Listings';
import Navbar from '../components/Navbar';
import { Link } from 'react-router-dom';


function Home() {

    return (
        <div className="background">
            <div className='home-page'>
                <div className="nav">
                    <Link to="/">
                        <img src={Logo} className='log' alt='Employmently logo'></img>
                    </Link>
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
            </div >
        </div>
    );
}

export default Home;