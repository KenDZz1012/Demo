import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import RouterAuth from "./RoutePage/Route.Auth";
import RouterCatalog from "./RoutePage/Route.Catalog";
import Layout from "../Layout/Layout";
import RouterDashboard from "./RoutePage/Route.Dashboard";

const index: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/login" />} />
      {RouterAuth.map((item: any, idx) => (
        <Route key={idx} {...item} />
      ))}
      <Route
        path="/*"
        element={
          <Layout>
            <Routes>
              {RouterDashboard.map((item: any, idx) => (
                <Route key={idx} {...item} />
              ))}
              {RouterCatalog.map((item: any, idx) => (
                <Route key={idx} {...item} />
              ))}
            </Routes>
          </Layout>
        }
      />
    </Routes>
  );
};

export default index;
