import React from "react";
import TestCode from "../../Pages/Catalog/Testcode";
import { RouteObject } from "react-router-dom";

const RouterCatalog: RouteObject[] = [
  {
    path: "/TestCode",
    element: <TestCode />,
  },
];

export default RouterCatalog;
