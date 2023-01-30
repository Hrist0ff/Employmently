import axios from "axios";
import { useEffect } from "react";
import { useState } from "react";
import '../styles/listings.css';
import format from 'date-fns/format';
import ru from 'date-fns/locale/bg';
import { isToday, isYesterday } from 'date-fns';


function CompanyListings({ companyId }) {
    const [listings, setListings] = useState([]);
    const [performed, setPerformed] = useState(false);


    useEffect(() => {
        axios.get(`${process.env.REACT_APP_BACKEND}/Information/getCompanyListings/${companyId}`)
            .then(response => {
                const listings = response.data.map(listing => {
                    listing.createdDate = new Date(listing.createdDate);
                    if (isToday(listing.createdDate)) {
                        listing.createdDate = 'Днес';
                    } else if (isYesterday(listing.createdDate)) {
                        listing.createdDate = 'Вчера';
                    } else {
                        listing.createdDate = format(listing.createdDate, "dd MMMM", { locale: ru });
                    }
                    return listing;
                });
                setListings(listings);
            })
            .catch(error => {
                console.log(error);
            })
        setPerformed(true);
    }, [companyId,performed]);

    return (
        <div>
            {listings.map((listing, idx) => {
                return (
                    <div className="company-listing-card" key={idx}>
                        <div className="listing-company">
                            <img src={listing.authorPic} alt="Company" className="img-company"></img>
                            <p className="company-creator">{listing.authorName}</p>
                        </div>
                        <div className="listing-left-side" key={idx}>
                            <a className="listing-name" href={`${process.env.REACT_APP_SERVER_PAGE}/Listing/${listing.id}`}>{listing.name}
                                {listing.location ? <p key={listing.location} className="paragraph-details">🏢{listing.location}</p> : null}
                                {listing.salary ? <p key={listing.salary} className="paragraph-details">💰Заплата от {listing.salary} лв.</p> : null}
                                {listing.categoryNames ? <p className="paragraph-details">📁{listing.categoryNames.map((category, index) => {
                                    return (
                                        <span key={index}>{category} &nbsp;</span>
                                    )
                                })}</p> : null}
                                {listing.arrangement ? <p key={listing.arrangement} className="paragraph-details">💼{listing.arrangement}</p> : null}
                            </a>
                        </div>
                        <div className="listing-right-side">
                            <p className="listing-date">📅{listing.createdDate}</p>
                        </div>
                    </div>
                )
            }
            )}
        </div >
    );
};

export default CompanyListings;