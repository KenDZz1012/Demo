import React, { Fragment, useState } from "react";
import CustomTable from "../../../Components/CustomTable";
import type { TableColumnsType, TableProps } from "antd";
import { ITestCode } from "../../../Interface/ITestCode";

const TestCode = ({}) => {
  const [dataSource, setDataSource] = useState([]);
  const [loading, setLoading] = useState(false);
  const columns: TableColumnsType<ITestCode> = [
    { title: "Mã xét nghiệm", dataIndex: "testCode", key: "testCode" },
    { title: "Tên xét nghiệm", dataIndex: "testName", key: "testName" },
    { title: "Nhóm xét nghiệm", dataIndex: "category", key: "category" },
    {
      title: "Loại mẫu",
      dataIndex: "type",
      key: "type",
    },
    { title: "Khoảng tham chiếu", dataIndex: "normalRange", key: "normalRange" },
    { title: "Đơn vị", dataIndex: "unit", key: "unit" },
    {
      title: "Giá",
      dataIndex: "price",
      key: "price",
    },
  ];
  return (
    <Fragment>
      <CustomTable data={dataSource} loading={loading} columns={columns} style={{ maxHeight: "calc(100vh - 271px)" }} />
    </Fragment>
  );
};

export default TestCode;
