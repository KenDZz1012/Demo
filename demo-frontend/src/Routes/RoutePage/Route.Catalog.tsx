import React from "react";
import TestCode from "../../Pages/Catalog/Testcode";
import { RouteObject } from "react-router-dom";
import TestType from "../../Pages/Catalog/TestType";

const RouterCatalog: RouteObject[] = [
  {
    path: "/TestCode",
    element: <TestCode />,
  },
  {
    path: "/TestType",
    element: <TestType />,
  },
];

export default RouterCatalog;
