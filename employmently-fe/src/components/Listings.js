import axios from "axios";
import { useEffect } from "react";
import Remote from "../images/remote.png";
import { useState } from "react";
import '../styles/listings.css';
import format from 'date-fns/format';
import { isToday, isYesterday } from 'date-fns';


function Listings() {
    const [listings, setListings] = useState([]);

    const [performed, setPerformed] = useState(false);


    useEffect(() => {
        axios.get(`${process.env.REACT_APP_BACKEND}/Information/getAcceptedListings`)
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
        setPerformed(true);
    }, [performed]);


    return (
        <div >
            {listings.map((listing, idx) => {
                return (
                    <div style={{ paddingTop: '20px' }}>
                        <div className="listing-section-home">
                            <div className="listing-container">

                                <div className="listing-item">
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
                                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}><img src={Remote} className="listing-tag-image"></img> &nbsp;{listing.arrangement}</p>
                                                    : null}
                                                {listing.arrangement && listing.arrangement === "On-site" ?
                                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💼 {listing.arrangement}</p>
                                                    : null}
                                                {listing.arrangement && listing.arrangement === "Hybrid" ?
                                                    <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%', alignItems: 'center' }}><img src={Remote} className="listing-tag-image-hybrid"></img> &nbsp; / 💼 {listing.arrangement}</p>
                                                    : null}
                                                {listing.salary ? <p className="listing-tag" style={{ display: 'flex', justifyContent: 'center', marginLeft: '1%' }}>💰 {listing.salary} lv.</p> : null}

                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                )
            }
            )}
        </div >
    );
};

export default Listings;