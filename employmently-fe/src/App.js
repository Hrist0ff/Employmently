import React from "react";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home";
import Login from "./pages/Login";
import ForgotPassword from "./pages/ForgotPassword";
import Register from "./pages/Register";

function App() {
  return (
    /* <Route path="*" element={<NoPage />} /> */
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/Login" element={<Login />} />
        {/* <Route path="/ForgotPassword" element={<ForgotPassword />} /> */}
        <Route path="/Register" element={<Register />} />
      </Routes>
    </BrowserRouter>
  );
}


export default App;