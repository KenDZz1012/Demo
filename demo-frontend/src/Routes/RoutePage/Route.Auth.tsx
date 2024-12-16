import React from "react";
import Login from "../../Pages/Authorize/Login";
import { RouteObject } from "react-router-dom";

const RouterAuth: RouteObject[] = [
  {
    path: "/login",
    element: <Login />,
  },
];

export default RouterAuth;
