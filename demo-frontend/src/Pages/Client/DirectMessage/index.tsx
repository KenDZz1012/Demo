import { Layout } from "antd";
import ListFriendSideBar from "./ListFriendSideBar";
const { Sider, Content } = Layout;

export default function DirectMessage() {
    return (
        <Layout style={{ height: '100%' }}>
            <Sider width={300} style={{ backgroundColor: "#21212a", padding: "10px 0px 10px 10px" }}>
                <ListFriendSideBar />
            </Sider>
            <Content style={{ backgroundColor: "#21212a", padding: "10px 10px 10px 0px" }}>

            </Content>
        </Layout>
    )
}
