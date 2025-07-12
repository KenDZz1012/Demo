import React from "react";
import { Route, Routes } from "react-router-dom";
import RouterAuth from "./RoutePage/Route.Auth";
import Home from "../Pages/Client/Home";
import App from "../Pages/Client/App";
const index: React.FC = () => {
  return (
    <Routes>
      {RouterAuth.map((item: any, idx) => (
        <Route key={idx} {...item} />
      ))}
      <Route path="/app" element={<App />} />

      <Route path="/" element={<Home />} />
    </Routes>
  );
};

export default index;
