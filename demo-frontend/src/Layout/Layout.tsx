import React, { useState, useEffect } from "react";
import { Button, Menu } from "antd";
import { AppstoreOutlined, BarChartOutlined, MailOutlined, MenuFoldOutlined, MenuUnfoldOutlined, SettingOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";
import { Link } from "react-router-dom";
import { MenuItems } from "./MenuItems";

const { SubMenu } = Menu;

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [collapsed, setCollapsed] = useState(true);
  const [isMobile, setIsMobile] = useState(false);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  const handleResize = () => {
    console.log(window.innerWidth);
    setIsMobile(window.innerWidth < 768);
  };

  useEffect(() => {
    handleResize();
    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  return (
    <div>
      <div style={{ display: "flex", flexDirection: isMobile ? "column" : "row" }}>
        {!isMobile && (
          <div style={{ width: 75, transition: "width 0.3s" }}>
            <div
              style={{
                display: "flex",
                alignItems: "center",
                backgroundColor: "#001529",
                paddingLeft: 16,
                justifyContent: "space-between",
                borderBottom: "1px solid #fff",
                padding: 16,
              }}
            >
              <Button
                style={{
                  color: "#fff",
                  border: "none",
                  padding: "5px 10px",
                  borderRadius: "4px",
                  cursor: "pointer",
                  backgroundColor: "#001529",
                }}
                onClick={toggleCollapsed}
              >
                {collapsed ? <MenuUnfoldOutlined style={{ fontSize: 20 }} /> : <MenuFoldOutlined style={{ fontSize: 20 }} />}
              </Button>
            </div>
            <Menu mode="inline" inlineCollapsed={true} className="custom-menu">
              {MenuItems.map((item: any) =>
                item.children ? (
                  <SubMenu key={item.key} title={item.label} icon={item.icon} style={{ fontSize: 14 }}>
                    {item.children?.map((child: any) => (
                      <Menu.Item key={child.key}>
                        <Link to={"/" + child.key}>{child.label}</Link>
                      </Menu.Item>
                    ))}
                  </SubMenu>
                ) : (
                  <Menu.Item key={item.key} icon={item.icon}>
                    <Link to={"/" + item.key}>{item.label}</Link>
                  </Menu.Item>
                )
              )}
            </Menu>
          </div>
        )}
        {isMobile && (
          <header
            style={{
              backgroundColor: "#001529",
              color: "#fff",
              padding: "10px 16px",
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              width: "100%",
            }}
          >
            <Button
              style={{
                color: "#fff",
                border: "none",
                padding: "5px 10px",
                borderRadius: "4px",
                cursor: "pointer",
                backgroundColor: "#001529",
              }}
              onClick={toggleCollapsed}
            >
              {collapsed ? <MenuUnfoldOutlined style={{ fontSize: 20 }} /> : <MenuFoldOutlined style={{ fontSize: 20 }} />}
            </Button>
            {!collapsed && (
              <Menu mode="inline" inlineCollapsed={collapsed} className="custom-menu" style={{ position: "absolute", top: 50, left: 0, zIndex: 1000, width: "100%" }}>
                {MenuItems.map((item: any) =>
                  item.children ? (
                    <SubMenu key={item.key} title={item.label} icon={item.icon} style={{ fontSize: 14 }}>
                      {item.children?.map((child: any) => (
                        <Menu.Item
                          key={child.key}
                          onClick={() => {
                            setCollapsed(false);
                          }}
                        >
                          <Link to={"/" + child.key}>{child.label}</Link>
                        </Menu.Item>
                      ))}
                    </SubMenu>
                  ) : (
                    <Menu.Item
                      key={item.key}
                      icon={item.icon}
                      onClick={() => {
                        setCollapsed(false);
                      }}
                    >
                      <Link to={"/" + item.key}>{item.label}</Link>
                    </Menu.Item>
                  )
                )}
              </Menu>
            )}
          </header>
        )}
        <div style={{ flexGrow: 1 }}>
          <header
            style={{
              backgroundColor: "#fff",
              color: "black",
              boxShadow: "0 6px 8px rgba(0, 0, 0, 0.2)",
              padding: 10,
              display: "flex",
              justifyContent: "flex-end",
            }}
          >
            <div>
              <img src={require("../assets/Images/account.jpg")} width={40} height={40} style={{ borderRadius: "50%" }} />
            </div>
          </header>
          <main style={{ padding: "20px", minHeight: "calc(100vh - 170px)" }}>{children}</main>
        </div>
      </div>
    </div>
  );
};

export default Layout;
