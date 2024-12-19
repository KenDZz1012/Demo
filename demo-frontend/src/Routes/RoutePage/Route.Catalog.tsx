import React from "react";
import TestCode from "../../Pages/Catalog/Testcode";
import { RouteObject } from "react-router-dom";
import TestType from "../../Pages/Catalog/TestType";
import Catalog from "../../Pages/Catalog";

const RouterCatalog: RouteObject[] = [
  {
    path: "/Catalog",
    element: <Catalog />,
  },
];

export default RouterCatalog;
