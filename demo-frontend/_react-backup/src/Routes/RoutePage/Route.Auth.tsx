import React from "react";
import Login from "../../Pages/Client/Login";
import { RouteObject } from "react-router-dom";
import Register from "../../Pages/Client/Register";

const RouterAuth: RouteObject[] = [
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  }
];

export default RouterAuth;
