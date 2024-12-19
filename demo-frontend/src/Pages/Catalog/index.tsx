import { Tabs } from "antd";
import React from "react";
import type { TabsProps } from "antd";
import TestCode from "./Testcode";
import TestType from "./TestType";
import Category from "./Category";
import MultiParameterTest from "./MutiParameterTest";
import Result from "./Result";
import Location from "./Location";
import Doctor from "./Doctor";
import Object from "./Object";
import Diagnostic from "./Diagnostic";
import { AppstoreOutlined, BarChartOutlined, ControlOutlined, ExperimentOutlined, MailOutlined, MenuFoldOutlined, MenuUnfoldOutlined, SettingOutlined } from "@ant-design/icons";

const items: TabsProps["items"] = [
  {
    key: "TestCode",
    label: "Xét nghiệm",
    children: <TestCode />,
  },
  {
    key: "TestType",
    label: "Loại mẫu",
    children: <TestType />,
  },
  {
    key: "Category",
    label: "Nhóm XN",
    children: <Category />,
  },
  {
    key: "MultiParameterTest",
    label: "Dịch vụ nhiều chỉ số",
    children: <MultiParameterTest />,
  },
  {
    key: "Result",
    label: "Kết quả",
    children: <Result />,
  },
  {
    key: "Location",
    label: "Khoa phòng chỉ định",
    children: <Location />,
  },
  {
    key: "Doctor",
    label: "Bác sĩ",
    children: <Doctor />,
  },
  {
    key: "Object",
    label: "Đối tượng",
    children: <Object />,
  },
  {
    key: "Diagnostic",
    label: "Chẩn đoán",
    children: <Diagnostic />,
  },
];

const Catalog = ({}) => {
  return (
    <div>
      <div style={{ display: "flex", justifyContent: "flex-start", fontSize: 20 }}>
        <div>
          <SettingOutlined />
        </div>
        <div style={{ marginLeft: 16 }}>Cấu hình danh mục</div>
      </div>
      <div style={{ border: "1px solid #ccc", borderRadius: 10, padding: 10, marginTop: 20 }}>
        <Tabs defaultActiveKey="TestCode" items={items} />;
      </div>
    </div>
  );
};

export default Catalog;
