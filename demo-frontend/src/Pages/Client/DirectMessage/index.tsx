import { Layout } from "antd";
import ListFriendSideBar from "./ListFriendSideBar";
import { useAddFriend, useFriends, useFriendsPending } from "Connections/AppBackend/UserRelationship";
import { useDispatch, useSelector } from "react-redux";
import { selectAuthUser, selectedFriend, selectedFriendId, selectFriends, selectFriendsPending } from "store/selectors/authSelectors";
import { useEffect, useState } from "react";
import { setSelectedFriend, setFriends, setFriendsPending } from "features/user-relationship/userRelationshipSlice";
import AddFriendSideBar from "./AddFriendSideBar";
import { Friend } from "types/user";
import ChatArea from "./ChatArea";
import { useSendMessage } from "Connections/AppBackend/DirectMessage";

const { Sider, Content } = Layout;

export default function DirectMessage() {
    const { id: userID } = useSelector(selectAuthUser) || {};
    const { data, isLoading } = useFriends({ userID });
    const { data: dataFriendsPending, isLoading: isLoadingFriendsPending } = useFriendsPending({ userID });

    const dispatch = useDispatch();
    const friends = useSelector(selectFriends);
    const friendPending = useSelector(selectFriendsPending);
    const friendId = useSelector(selectedFriendId)
    const friend = useSelector(selectedFriend)
    const [input, setInput] = useState('');
    const { mutate: mutateSendMessage } = useSendMessage();

    const onSelectedFriend = (friend: Friend | null) => {
        if (!friend) {
            dispatch(setSelectedFriend(null));
            return;
        }
        dispatch(setSelectedFriend(friend));
    }

    const sendMessage = () => {
        if (input.trim() && friendId) {
            mutateSendMessage({
                senderId: userID || '',
                recipientIds: [friendId],
                content: input.trim()
            })
        }
    };

    useEffect(() => {
        if (data?.data) {
            dispatch(setFriends(data.data));
        }
    }, [data, dispatch]);

    useEffect(() => {
        if (dataFriendsPending?.data) {
            dispatch(setFriendsPending(dataFriendsPending.data));
        }
    }, [dataFriendsPending, dispatch]);


    return (
        <Layout style={{ height: '100%' }}>
            <Sider width={300} style={{ backgroundColor: "#21212a", padding: "10px 0px 10px 10px" }}>
                <ListFriendSideBar friends={friends} onSelectedFriend={onSelectedFriend} friendId={friendId} />
            </Sider>
            <Content style={{ backgroundColor: "#21212a", padding: "10px 10px 10px 0px" }}>
                {friendId ?
                    <ChatArea friend={friend}
                        input={input}
                        setInput={setInput}
                        sendMessage={sendMessage} />
                    :
                    <AddFriendSideBar friendPending={friendPending} />
                }
            </Content>
        </Layout>
    )
}
