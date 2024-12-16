import React from "react";
import DashboardGeneral from "../../Pages/Dashboard/DashboardGeneral";
import { RouteObject } from "react-router-dom";

const RouterDashboard: RouteObject[] = [
  {
    path: "/DashboardGeneral",
    element: <DashboardGeneral />,
  },
];

export default RouterDashboard;
