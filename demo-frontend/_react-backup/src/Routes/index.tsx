import React from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import RouterAuth from "./RoutePage/Route.Auth";
import Home from "../Pages/Client/Home";
import App from "../Pages/Client/App";
import Server from "../Pages/Client/Server";
import DirectMessage from "../Pages/Client/DirectMessage";
const index: React.FC = () => {
  return (
    <Routes>
      {RouterAuth.map((item: any, idx) => (
        <Route key={idx} {...item} />
      ))}
      <Route path="/" element={<Home />} />
      <Route path="/server" element={<App />}>
        <Route index element={<Navigate to="@me" replace />} />
        <Route path="@me" element={<DirectMessage />} />
        <Route path=":id" element={<Server />} />
      </Route>
      <Route path="*" element={<div style={{ color: '#fff', padding: 20 }}>404 Not Found</div>} />
    </Routes>
  );
};

export default index;
