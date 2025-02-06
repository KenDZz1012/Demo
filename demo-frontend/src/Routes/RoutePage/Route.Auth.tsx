import React from "react";
import Login from "../../Pages/Client/Authorize/Login";
import { RouteObject } from "react-router-dom";
import Register from "../../Pages/Client/Authorize/Register";

const RouterAuth: RouteObject[] = [
  {
    path: "/tv/SignIn",
    element: <Login />,
  },
  {
    path: "/tv/SignUp",
    element: <Register />,
  },
];

export default RouterAuth;
