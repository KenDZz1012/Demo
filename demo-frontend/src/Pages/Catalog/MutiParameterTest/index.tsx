import React, { Fragment, useState } from "react";
import CustomTable from "../../../Components/CustomTable";

const MultiParameterTest = ({}) => {
  const [dataSource, setDataSource] = useState([]);
  const [loading, setLoading] = useState(false);
  const columns = [{}];
  return (
    <Fragment>
      <CustomTable data={dataSource} loading={loading} columns={columns} style={{ maxHeight: "calc(100vh - 271px)" }} />
    </Fragment>
  );
};

export default MultiParameterTest;
