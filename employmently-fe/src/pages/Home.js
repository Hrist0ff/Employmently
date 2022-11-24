import React from 'react';
import Dropdown from 'react-bootstrap/Dropdown';
import '../styles/home.css';

function Home() {
    console.log(process.env.REACT_APP_SERVER_PAGE);
    return(
        <body>
            <div class="nav">

                <label for="toggle">&#9776;</label>
                <input type="checkbox" id="toggle"/>
                <div class="menu">
                    <a>Вход</a>
                    <a href="#">Създай акаунт</a>
                    <a>Обяви</a>
                </div>
            </div>
        </body>

        /*
        <body>
            <div class="nav">
                <label for="toggle">&#9776;</label>
                <input type="checkbox" id="toggle"/>
                <div class="menu">
                    <a href="#">Business</a>
                    <a href="#">Services</a>
                    <a href="#">Learn More</a>
                    <a href="#"><span>Free Trial</span></a>
                </div>
            </div>
        </body>*/
    );
}

export default Home;