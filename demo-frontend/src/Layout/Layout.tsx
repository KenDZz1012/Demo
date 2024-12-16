import React, { useState } from "react";
import { Button, Menu } from "antd";
import { AppstoreOutlined, BarChartOutlined, MailOutlined, MenuFoldOutlined, MenuUnfoldOutlined, SettingOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";
import { Link } from "react-router-dom";
import { MenuItems } from "./MenuItems";

const { SubMenu } = Menu;

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [collapsed, setCollapsed] = useState(true);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  return (
    <div>
      <header
        style={{
          backgroundColor: "#fff",
          color: "black",
          padding: "10px",
          boxShadow: "0 6px 8px rgba(0, 0, 0, 0.2)",
          position: "relative",
          paddingLeft: 18,
        }}
      >
        <div style={{ display: "flex", alignItems: "center" }}>
          <Button
            style={{
              backgroundColor: "#fff",
              color: "black",
              border: "none",
              padding: "5px 10px",
              borderRadius: "4px",
              cursor: "pointer",
            }}
            onClick={toggleCollapsed}
          >
            {collapsed ? <MenuUnfoldOutlined style={{ fontSize: 20 }} /> : <MenuFoldOutlined style={{ fontSize: 20 }} />}
          </Button>
          <h1 style={{ marginLeft: "10px", fontSize: "18px", fontWeight: "bold" }}>My Lab</h1>
        </div>
      </header>

      <div style={{ display: "flex" }}>
        <div style={{ textAlignLast: "left", width: collapsed ? 75 : 256 }}>
          <Menu mode="inline" inlineCollapsed={collapsed}>
            {MenuItems.map((item: any) => (
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
        <main style={{ padding: "20px", minHeight: "calc(100vh - 144px)" }}>{children}</main>
      </div>
      <footer style={{ backgroundColor: "#f1f1f1", textAlign: "center", padding: "10px", position: "fixed", bottom: 0 }}>© 2024 My Lab</footer>
    </div>
  );
};

export default Layout;
