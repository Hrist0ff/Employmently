import React from "react";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home";
import Login from "./pages/Login";
import ForgotPassword from "./pages/ForgotPassword";
import Register from "./pages/Register";
import ChangePassword from "./pages/ChangePassword";
import MyProfile from "./pages/MyProfile";
import MyCompany from "./pages/MyCompany";
import Company from "./pages/Company";
import AdminPanel from "./pages/AdminPanel";
import Listing from "./pages/Listing";
import CreateListing from "./pages/CreateListing";
import Applications from "./pages/Applications";
import Profile from "./pages/Profile";
import ListingsPage from "./pages/ListingsPage"

import "./fonts/rock.ttf";
import "./fonts/rockb.ttf";


function App() {
  return (
    /* <Route path="*" element={<NoPage />} /> */
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/Login" element={<Login />} />
        <Route path="/ForgotPassword" element={<ForgotPassword />} />
        <Route path="/Register" element={<Register />} />
        <Route path="Changepassword/id=:id/token=:token" element={<ChangePassword />} />
        <Route path="/MyProfile" element={<MyProfile />} />
        <Route path="/MyCompany" element={<MyCompany />} />
        <Route path="Company/:id" element={<Company />} />
        <Route path="AdminPanel" element={<AdminPanel />} />
        <Route path="Listing/:id" element={<Listing />} />
        <Route path="CreateListing" element={<CreateListing />} />
        <Route path="Applications" element={<Applications />} />
        <Route path="Profile/:id" element={<Profile />} />
        <Route path="ListingsPage" element={<ListingsPage />} />
      </Routes>
    </BrowserRouter>
  );
}


export default App;