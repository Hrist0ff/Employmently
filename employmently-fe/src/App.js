import React from "react";
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home";


function App() {
  return (
    /* <Route path="*" element={<NoPage />} /> */
    <BrowserRouter>
        <Routes>
          <Route path="/" element = {<Home />}/>
          
        </Routes>
    </BrowserRouter>      
  );
}


export default App;