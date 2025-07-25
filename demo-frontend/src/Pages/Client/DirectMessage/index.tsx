import { Layout } from "antd";
import ListFriendSideBar from "./ListFriendSideBar";
import { useAddFriend, useFriends } from "Connections/AppBackend/UserRelationship";
import { useDispatch, useSelector } from "react-redux";
import { selectAuthUser, selectFriends } from "store/selectors/authSelectors";
import { useEffect } from "react";
import { setFriends } from "features/user-relationship/userRelationshipSlice";
import AddFriendSideBar from "./AddFriendSideBar";

const { Sider, Content } = Layout;

export default function DirectMessage() {
    const { id: userID } = useSelector(selectAuthUser) || {};
    const { data, isLoading } = useFriends({ userID });
    const dispatch = useDispatch();
    const friends = useSelector(selectFriends);

    useEffect(() => {
        if (data?.data) {
            dispatch(setFriends(data.data));
        }
    }, [data, dispatch]);


    return (
        <Layout style={{ height: '100%' }}>
            <Sider width={300} style={{ backgroundColor: "#21212a", padding: "10px 0px 10px 10px" }}>
                <ListFriendSideBar friends={friends} />
            </Sider>
            <Content style={{ backgroundColor: "#21212a", padding: "10px 10px 10px 0px" }}>
                <AddFriendSideBar />
            </Content>
        </Layout>
    )
}
