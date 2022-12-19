import React from "react";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home";
import Login from "./pages/Login";
import ForgotPassword from "./pages/ForgotPassword";
import Register from "./pages/Register";
import ChangePassword from "./pages/ChangePassword";
import MyProfile from "./pages/MyProfile";

function App() {
  return (
    /* <Route path="*" element={<NoPage />} /> */
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/Login" element={<Login />} />
        <Route path="/ForgotPassword" element={<ForgotPassword />} />
        <Route path="/Register" element={<Register />} />
        <Route path="Changepassword/id=:id" element={<ChangePassword />} />
        <Route path="/MyProfile" element={<MyProfile />} />
      </Routes>
    </BrowserRouter>
  );
}


export default App;