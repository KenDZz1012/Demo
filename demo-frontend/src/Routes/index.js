import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import RouterPage from "./Route.page";

const index = () => {
  return (
    <Routes>
      {RouterPage.map((item, idx) => {
        return <Route key={idx} {...item} />;
      })}
    </Routes>
  );
};

export default index;
