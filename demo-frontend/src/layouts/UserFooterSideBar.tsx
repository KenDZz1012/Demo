import { Dropdown, Menu } from "antd"
import { useSelector } from "react-redux"
import { selectAuthUser } from "store/selectors/authSelectors"
import { ExportOutlined, LogoutOutlined } from '@ant-design/icons';
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useLogout } from "Connections/AppBackend/Auth";
import { refreshToken } from "features/auth/authAPI";


export default function UserFooterSideBar() {
    const user = useSelector(selectAuthUser)
    const [isHovered, setIsHovered] = useState(false);
    const navigate = useNavigate();
    const { mutateAsync } = useLogout();

    const serverMenu = (
        <Menu
            className="menu-server-setting"
            theme="dark"
            style={{
                backgroundColor: "#001529",
                width: "100%",
                placeSelf: "center"
            }}
            items={[
                {
                    key: 'leave',
                    label: (
                        <div style={{ display: "flex", justifyContent: "space-between" }}>
                            <span style={{ color: '#f17875' }}>Log out</span>
                            <LogoutOutlined style={{ color: '#f17875', fontSize: 14 }} />
                        </div>
                    ),
                    onClick: () => {
                        handleLogout()
                    },
                },
            ]}
        />
    );


    const handleLogout = async () => {
        try {
            const refreshToken = localStorage.getItem('refreshToken') || '';
            const response = await mutateAsync({ refreshToken });
            if (response) {
                navigate('/login', { replace: true });
            } else {
                console.error('Logout failed from server.');
            }
        } catch (error) {
            console.error('Logout failed:', error);
        }
    };

    return (
        <div style={{ position: "absolute", bottom: 10, width: 390, paddingLeft: 10 }}>
            <div
                style={{
                    position: 'sticky',
                    bottom: 0,
                    padding: 8,
                    backgroundColor: '#3b3b47ff',
                    zIndex: 1,
                    borderRadius: 10,
                    display: 'flex',
                    flexDirection: 'row',
                }}
            >
                <Dropdown overlay={serverMenu} trigger={['click']} placement="bottomLeft">
                    <div
                        style={{ display: 'flex', alignItems: 'center', gap: 10, width: "80%", backgroundColor: isHovered ? '#41414bff' : 'transparent', paddingTop: 6, paddingBottom: 6, borderRadius: 10, cursor: 'pointer' }}
                        onMouseEnter={() => setIsHovered(true)}
                        onMouseLeave={() => setIsHovered(false)}
                    >
                        <div style={{ position: 'relative', width: 36, height: 36 }}>
                            <img
                                src={user?.avatarUrl || '/logo.png'}
                                alt={user?.username}
                                style={{
                                    width: '100%',
                                    height: '100%',
                                    borderRadius: '50%',
                                    objectFit: 'cover',
                                    display: 'block',
                                    backgroundColor: "#6b6967"
                                }}
                            />
                            <span
                                style={{
                                    position: 'absolute',
                                    bottom: 0,
                                    right: 0,
                                    width: 10,
                                    height: 10,
                                    backgroundColor: 'green',
                                    borderRadius: '50%',
                                    border: '2px solid white',
                                }}
                            />
                        </div>
                        <div style={{ display: 'flex', flexDirection: 'column', alignItems: "flex-start" }}>
                            <span style={{ color: 'white', fontSize: 16 }}>{user?.displayName}</span>
                            <span style={{ color: 'white', fontSize: 12 }}>Online</span>
                        </div>
                    </div>
                </Dropdown>
            </div>
        </div>
    )
}