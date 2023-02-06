import { Link } from "react-router-dom";
import Logo from "../images/employment.png"
import ExpiredTokenCheck from "../components/ExpiredTokenCheck";
import Navbar from "../components/Navbar";
import Remote from "../images/remote.png"
import { useState } from "react";
import { useEffect } from "react";
import axios from "axios";
import format from 'date-fns/format';
import { isToday, isYesterday } from 'date-fns';
import "../styles/filters.css"
import Briefcase from "../images/briefcase.png"
import React from "react";
import Location from "../images/location.png"
import Money from "../images/money.png"



function ListingsPage() {
    const [listings, setListings] = useState([]);
    const [categories, setCategories] = useState([]);
    const [locations, setLocations] = useState([]);


    const [errorMessage, setErrorMessage] = React.useState("");


    const [performed, setPerformed] = useState(false);


    const [checkedCategories, setCheckedCategories] = useState([]);
    const [checkedLocations, setCheckedLocations] = useState([]);
    const [checkedSalary, setCheckedSalary] = useState([]);
    const [statedSalary, setStatedSalary] = useState(false);
    const [arrangement, setArrangement] = useState("");

    const handleCheckboxChange = (category) => {
        const value = category.target.value;
        setCheckedCategories(prevCheckedCategories => {
            if (prevCheckedCategories.includes(value)) {
                return prevCheckedCategories.filter(item => item !== value);
            } else {
                return [...prevCheckedCategories, value];
            }
        });
    };

    const handleCheckboxChangeLocation = (location) => {
        const value = location.target.value;
        setCheckedLocations(prevCheckedLocations => {
            if (prevCheckedLocations.includes(value)) {
                return prevCheckedLocations.filter(item => item !== value);
            } else {
                return [...prevCheckedLocations, value];
            }
        });
    };

    const handleCheckboxChangeSalary = (salary) => {
        const value = salary.target.value;
        setCheckedSalary(prevCheckedSalary => {
            if (prevCheckedSalary.includes(value)) {
                return prevCheckedSalary.filter(item => item !== value);
            } else {
                return [...prevCheckedSalary, value];
            }
        });
    };

    const handleCheckboxStatedSalary = (salary) => {
        setStatedSalary(prevStatedSalary => !prevStatedSalary);
    };

    const handleCheckboxChangeArrangement = (arrangement) => {
        const value = arrangement.target.value;
        setArrangement(prevArrangement => (prevArrangement === value ? "" : value));
    };



    useEffect(() => {
        FilteredListings(checkedCategories);
    }, [checkedCategories]);

    useEffect(() => {
        FilteredListings(checkedLocations);
    }, [checkedLocations]);

    useEffect(() => {
        FilteredListings(checkedSalary);
    }, [checkedSalary]);

    useEffect(() => {
        FilteredListings(statedSalary);
    }, [statedSalary]);
    useEffect(() => {
        FilteredListings(arrangement);
    }, [arrangement]);

    const FilteredListings = () => {

        const categories = [...checkedCategories];

        const locations = [...checkedLocations];

        let salary = [...checkedSalary];
        salary = salary.toString().split("-");

        const queryParams = [];


        if (salary.length === 2) {
            const minSalary = salary[0];
            const maxSalary = salary[1];

            if (minSalary == 0) {
                queryParams.push("minSalary=" + 1);
            }
            else {
                queryParams.push("minSalary=" + minSalary);
            }

            queryParams.push("maxSalary=" + maxSalary);
        }

        const statedSalaryy = statedSalary;
        if (statedSalaryy === true) {
            queryParams.push("salaryStated=" + statedSalaryy);
        }

        const arrangementt = arrangement;
        if (arrangementt !== "") {
            queryParams.push("arrangement=" + arrangementt);
        }




        for (let i = 0; i < categories.length; i++) {
            queryParams.push("categories=" + categories[i]);
        }

        for (let i = 0; i < locations.length; i++) {
            queryParams.push("locations=" + locations[i]);
        }



        axios.get(`${process.env.REACT_APP_BACKEND}/Filter?${queryParams.join("&")}`, {
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                const listings = response.data.map(listing => {
                    listing.createdDate = new Date(listing.createdDate);
                    if (isToday(listing.createdDate)) {
                        listing.createdDate = 'Today';
                    } else if (isYesterday(listing.createdDate)) {
                        listing.createdDate = 'Yesterday';
                    } else {
                        listing.createdDate = format(listing.createdDate, "dd MMMM");
                    }
                    return listing;
                });
                setListings(listings);

            })
            .catch(error => {
                setErrorMessage(error.response.data.Error[0]);
            })

    }

    useEffect(() => {
        axios.get(`${process.env.REACT_APP_BACKEND}/Filter`)
            .then(response => {
                const listings = response.data.map(listing => {
                    listing.createdDate = new Date(listing.createdDate);
                    if (isToday(listing.createdDate)) {
                        listing.createdDate = 'Today';
                    } else if (isYesterday(listing.createdDate)) {
                        listing.createdDate = 'Yesterday';
                    } else {
                        listing.createdDate = format(listing.createdDate, "dd MMMM");
                    }
                    return listing;
                });
                setListings(listings);
                console.log(listings)
            })
            .catch(error => {
                console.log(error);
            })

        axios.get(`${process.env.REACT_APP_BACKEND}/Information/getCategories`)
            .then(response => {
                setCategories(response.data);
                console.log(response.data)
            })
            .catch(error => {
                console.log(error);
            })

        axios.get(`${process.env.REACT_APP_BACKEND}/Information/getLocations`)
            .then(response => {
                setLocations(response.data);
                console.log(response.data)
            })
            .catch(error => {
                console.log(error);
            })
        setPerformed(true);
    }, [performed]);

    return (
        <div className='background'>
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
            <div style={{ display: "flex" }}>
                <div className="filters-div">
                    <div className="box">
                        <div className="filters-categories">
                            <div className="filter-head">

                                <div style={{ display: "flex", flexDirection: "row", alignItems: "center", gap: "5px" }}>
                                    <img src={Briefcase} className="icon" alt="Icon for the 'Seniority'" style={{ width: "22px", height: "20px" }}></img>
                                    <p style={{ fontSize: '16px', color: '#5cadff', fontWeight: '400' }}>Categories</p>
                                </div>
                                <div style={{ display: "flex", flexDirection: "column", width: "100%" }}>
                                    {categories ? categories.map((category) => (
                                        <div className="filter-one-category" >
                                            <input type="checkbox" name={category} value={category} label={category} onClick={handleCheckboxChange} />
                                            <p style={{ marginTop: '12px' }}>{category}</p>
                                        </div>
                                    ))
                                        : null}
                                </div>

                                <div style={{ display: "flex", flexDirection: "row", alignItems: "center", gap: "5px" }}>
                                    <img src={Location} className="icon" alt="Icon for the 'Seniority'" style={{ width: "20px", height: "23px" }}></img>
                                    <p style={{ fontSize: '16px', color: '#5cadff', fontWeight: '400' }}>Locations</p>
                                </div>
                                <div style={{ display: "flex", flexDirection: "column" }}>
                                    {locations ? locations.map((location) => (
                                        <div className="filter-one-category" >
                                            <input type="checkbox" name={location} value={location} label={location} onClick={handleCheckboxChangeLocation} />
                                            <p style={{ marginTop: '12px' }}>{location}</p>
                                        </div>
                                    ))
                                        : null}
                                </div>

                                <div style={{ display: "flex", flexDirection: "row", alignItems: "center", gap: "5px" }}>
                                    <img src={Money} className="icon" alt="Icon for the 'Seniority'" style={{ width: "20px", height: "20px" }}></img>
                                    <p style={{ fontSize: '16px', color: '#5cadff', fontWeight: '400' }}>Salary</p>
                                </div>
                                <div style={{ display: "flex", flexDirection: "column", width: "100%" }}>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="salary" value="stated" label="salary" onClick={handleCheckboxStatedSalary} />
                                        <p style={{ marginTop: '12px' }}>Stated salary</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="salary" value="0-500" label="salary" onClick={handleCheckboxChangeSalary} />
                                        <p style={{ marginTop: '12px' }}>0-500</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="salary" value="500-1500" label="salary" onClick={handleCheckboxChangeSalary} />
                                        <p style={{ marginTop: '12px' }}>500-1500</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="salary" value="1500-2500" label="salary" onClick={handleCheckboxChangeSalary} />
                                        <p style={{ marginTop: '12px' }}>1500-2500</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="salary" value="2500-5000" label="salary" onClick={handleCheckboxChangeSalary} />
                                        <p style={{ marginTop: '12px' }}>2500-5000</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="salary" value="5000-100000" label="salary" onClick={handleCheckboxChangeSalary} />
                                        <p style={{ marginTop: '12px' }}>5000+</p>
                                    </div>
                                </div>

                                <div style={{ display: "flex", flexDirection: "row", alignItems: "center", gap: "5px" }}>
                                    <img src={Briefcase} className="icon" alt="Icon for the 'Seniority'" style={{ width: "22px", height: "20px" }}></img>
                                    <p style={{ fontSize: '16px', color: '#5cadff', fontWeight: '400' }}>Arrangement</p>
                                </div>
                                <div style={{ display: "flex", flexDirection: "column", width: "100%" }}>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="Remote" value="Remote" label="Remote" onClick={handleCheckboxChangeArrangement} />
                                        <p style={{ marginTop: '12px' }}>Remote</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="On-site" value="On-site" label="On-site" onClick={handleCheckboxChangeArrangement} />
                                        <p style={{ marginTop: '12px' }}>On-site</p>
                                    </div>
                                    <div className="filter-one-category" >
                                        <input type="checkbox" name="Hybrid" value="Hybrid" label="Hybrid" onClick={handleCheckboxChangeArrangement} />
                                        <p style={{ marginTop: '12px' }}>Hybrid</p>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div className="filters-listings-div">
                    {listings.map((listing, idx) =>
                        <div style={{ paddingTop: '20px' }}>
                            <div className="listing-section-home">
                                <div className="filter-listing-container">
                                    <div className="filter-listing-item">
                                        <a href={`${process.env.REACT_APP_SERVER_PAGE}/Company/${listing.companyId}`} className="listing-a-div" >
                                            <div>
                                                <img className="listing-company-logo" src={listing.authorPic} alt="Company logo"></img>
                                                <p className="listing-company-name-text">{listing.authorName}</p>
                                            </div>
                                        </a>
                                        <a href={`${process.env.REACT_APP_SERVER_PAGE}/Listing/${listing.id}`} className="listing-a">
                                            <div>
                                                <div className="listing-header">
                                                    <p className="listing-title-text">{listing.name}</p>
                                                    <p className="listing-date">📅{listing.createdDate}</p>
                                                </div>
                                                <div className="listing-categories-tags-div">
                                                    <p className="listing-tag">📍{listing.location}</p>
                                                    {listing.arrangement && listing.arrangement === "Remote" ?
                                                        <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}><img src={Remote} alt="Remote" className="listing-tag-image"></img> &nbsp;{listing.arrangement}</p>
                                                        : null}
                                                    {listing.arrangement && listing.arrangement === "On-site" ?
                                                        <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💼 {listing.arrangement}</p>
                                                        : null}
                                                    {listing.arrangement && listing.arrangement === "Hybrid" ?
                                                        <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%', alignItems: 'center' }}><img src={Remote} alt="Remote" className="listing-tag-image-hybrid"></img> &nbsp; / 💼 {listing.arrangement}</p>
                                                        : null}
                                                    {listing.salary ? <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💰 {listing.salary} lv.</p> : null}

                                                </div>
                                            </div>
                                        </a>
                                    </div>


                                </div>
                            </div>
                        </div >
                    )}
                </div>


            </div>

        </div >
    )

}


export default ListingsPage;