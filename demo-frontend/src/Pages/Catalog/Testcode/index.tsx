import React, { Fragment, useState } from "react";
import CustomTable from "../../../Components/CustomTable";
import type { TableColumnsType, TableProps } from "antd";
import { ITestCode } from "../../../Interface/ITestCode";
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import { GetTestCodes } from "./helper";

const TestCode = ({}) => {
  const { data, isLoading, isError, error } = useQuery<ITestCode[], Error>({
    queryKey: ["testCodes"], // Use queryKey as an object property
    queryFn: GetTestCodes, // Use queryFn to specify the fetching function
  });
  const dataSource = data?.map((item) => ({
    ...item,
    key: item.testCode, // Ensure a unique `key` field
  }));
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
  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError) {
    return <div>Error: {error?.message}</div>;
  }

  return (
    <Fragment>
      <CustomTable data={dataSource} loading={isLoading} columns={columns} style={{ maxHeight: "calc(100vh - 271px)" }} />
    </Fragment>
  );
};

export default TestCode;
