import { MenuProps } from "antd";
import { AppstoreOutlined, BarChartOutlined, ControlOutlined, ExperimentOutlined, MailOutlined, MenuFoldOutlined, MenuUnfoldOutlined, SettingOutlined } from "@ant-design/icons";

type MenuItem = Required<MenuProps>["items"][number];

export const MenuItems: MenuItem[] = [
  {
    key: "dashboard",
    label: "Dashboard",
    icon: <BarChartOutlined />,
    children: [{ key: "DashboardGeneral", label: "Tổng hợp" }],
  },
  {
    key: "patient-result",
    label: "Quy trình xét nghiệm",
    icon: <ExperimentOutlined />,
    children: [
      { key: "PatientReception", label: "Tiếp nhận bệnh phẩm" },
      { key: "PatientResult", label: "Kết quả xét nghiệm" },
    ],
  },
  {
    key: "catalog",
    label: "Danh mục",
    icon: <SettingOutlined />,
    children: [
      { key: "TestCode", label: "Xét nghiệm" },
      { key: "TestType", label: "Loại mẫu" },
      { key: "Category", label: "Nhóm xét nghiệm" },
    ],
  },
  {
    key: "config",
    label: "Cài đặt",
    icon: <ControlOutlined />,
    children: [{ key: "Config", label: "Cấu hình hệ thống" }],
  },
];
