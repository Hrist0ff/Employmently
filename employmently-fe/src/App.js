import React from "react";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home";
import Login from "./pages/Login";

function App() {
  return (
    /* <Route path="*" element={<NoPage />} /> */
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/Login" element={<Login />} />

      </Routes>
    </BrowserRouter>
  );
}


export default App;