import React, { useState } from "react";
import { Button, Menu } from "antd";
import { AppstoreOutlined, BarChartOutlined, MailOutlined, MenuFoldOutlined, MenuUnfoldOutlined, SettingOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";
import { Link } from "react-router-dom";

type MenuItem = Required<MenuProps>["items"][number];
const { SubMenu } = Menu;
const items: MenuItem[] = [
  {
    key: "dashboard",
    label: "Dashboard",
    icon: <BarChartOutlined />,
    children: [{ key: "dashboardgeneral", label: "Tổng hợp" }],
  },
  {
    key: "catalog",
    label: "Danh mục",
    icon: <SettingOutlined />,
    children: [
      { key: "testcode", label: "Xét nghiệm" },
      { key: "type", label: "Loại mẫu" },
    ],
  },
];

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [collapsed, setCollapsed] = useState(false);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  return (
    <div>
      <header
        style={{
          backgroundColor: "#333399",
          color: "#fff",
          padding: "10px",
          boxShadow: "0 6px 8px rgba(0, 0, 0, 0.2)",
        }}
      >
        <div style={{ display: "flex", alignItems: "center" }}>
          <Button
            style={{
              backgroundColor: "#333399",
              color: "#fff",
              border: "none",
              padding: "5px 10px",
              borderRadius: "4px",
              cursor: "pointer",
            }}
            onClick={toggleCollapsed}
          >
            {collapsed ? <MenuUnfoldOutlined style={{ fontSize: 20 }} /> : <MenuFoldOutlined style={{ fontSize: 20 }} />}
          </Button>
          <h1 style={{ marginLeft: "10px", fontSize: "24px", fontWeight: "bold" }}>My App</h1>
        </div>
      </header>

      <div style={{ display: "flex" }}>
        <div style={{ textAlignLast: "left", width: collapsed ? 75 : 256 }}>
          <Menu mode="inline" inlineCollapsed={collapsed}>
            {items.map((item: any) => (
              <SubMenu key={item.key} title={item.label} icon={item.icon} style={{ fontSize: 14 }}>
                {item.children?.map((child: any) => (
                  <Menu.Item key={child.key}>
                    <Link to={"/" + child.key}>{child.label}</Link>
                  </Menu.Item>
                ))}
              </SubMenu>
            ))}
          </Menu>
        </div>
        <main style={{ padding: "20px", minHeight: "calc(100vh - 158px)" }}>{children}</main>
      </div>
      <footer style={{ backgroundColor: "#f1f1f1", textAlign: "center", padding: "10px" }}>© 2024 My App</footer>
    </div>
  );
};

export default Layout;
