import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import RouterAuth from "./RoutePage/Route.Auth";
import Layout from "../Layout/Layout";

const index: React.FC = () => {
  return (
    <Routes>
      {RouterAuth.map((item: any, idx) => (
        <Route key={idx} {...item} />
      ))}
      <Route
        path="/*"
        element={
          <Layout>
            <Routes></Routes>
          </Layout>
        }
      />
    </Routes>
  );
};

export default index;
