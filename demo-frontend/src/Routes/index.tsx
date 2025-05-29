import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import RouterAuth from "./RoutePage/Route.Auth";
import Layout from "../Layout/Layout";
import Home from "../Pages/Client/Home";

const index: React.FC = () => {
  return (
    <Routes>
      {RouterAuth.map((item: any, idx) => (
        <Route key={idx} {...item} />
      ))}
      <Route path="/" element={<Home />} />
    </Routes>
  );
};

export default index;
