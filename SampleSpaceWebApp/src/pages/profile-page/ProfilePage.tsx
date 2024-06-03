import {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import profilePageClasses from "./ProfilePage.module.css";
import SampleList from "../../components/sample-list/SampleList.tsx";
import Button, {RadioButton} from "../../components/button/Button.tsx";
import useAuth from "../../hook/useAuth.ts";
import IUser from "../../dal/entities/IUser.ts";
import ISample from "../../dal/entities/ISample.ts";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner.tsx";
import NotFoundPage from "../not-found/NotFoundPage.tsx";
import IPlaylist from "../../dal/entities/IPlaylist.ts";
import PlaylistToolsProfilePanel
    from "../../components/playlist/playlist-tools-profile-panel/PlaylistToolsProfilePanel.tsx";
import useUserApi from "../../dal/api/user/useUserApi.ts";
import useSampleApi from "../../dal/api/sample/useSampleApi.ts";
import usePlaylistApi from "../../dal/api/playlist/usePlaylistApi.ts";
import UserAvatar from "../../components/user-avatar/UserAvatar.tsx";
import Modal from "../../components/modal/Modal.tsx";
import EditProfileModal from "../../components/profile/EditProfileModal.tsx";
import ConfirmModal from "../../components/dialog/confirm/ConfirmModal.tsx";

export default function ProfilePage() {
    const navigate = useNavigate()
    const {getUser, signOut} = useUserApi();
    const {getByPlaylist, getUserSamples} = useSampleApi();
    const {getUserPlaylists} = usePlaylistApi();

    const {nickname} = useParams<{ nickname: string }>();
    const [user, setUser] = useState<IUser | null>();
    const {loginUser, delUser} = useAuth()
    const [userPlaylists, setUserPlaylists] = useState<IPlaylist[]>();
    const [selectedPlaylist, setSelectedPlaylist] = useState<IPlaylist>();
    const [playlistSamples, setPlaylistSamples] = useState<ISample[]>();

    const [editProfileIsOpen, setEditProfileIsOpen] = useState(false);
    const [confirmIsOpen, setConfirmIsOpen] = useState(false);

    // const [statisticsIsOpen, setStatisticsIsOpen] = useState(false);

    // const [userSamples, setUserSamples] = useState<Array<ISample>>()

    // async function fetchUserSamples() {
    //     const response = await getUserSamples(user!.userGuid);
    //     setUserSamples(response);
    // }

    // useEffect(() => {
    //     void fetchUserSamples();
    // }, [user]);

    const handleSignOutConfirm = async () => {
        setConfirmIsOpen(false);
        await signOut();
        delUser();
    }

    const handleEditProfileOnSuccess = async (modifiedUser: IUser) => {
        setEditProfileIsOpen(false);
        navigate(`/${modifiedUser.nickname}`);
    }

    const handleEditProfileOnDelete = async () => {
        await signOut();
        delUser();
        navigate(`/`);
    }
    
    async function fetchUser() {
        const response = await getUser(nickname!);
        setUser(response);
    }

    async function fetchUserPlaylist() {
        const response: IPlaylist[] = await getUserPlaylists(user!.userGuid.toString());
        response.unshift({
            playlistGuid: user!.userGuid,
            userGuid: user!.userGuid,
            name: "Созданные",
            canBeModified: false
        });

        setUserPlaylists(response);
        setSelectedPlaylist(response[0]);
    }

    async function fetchPlaylistSamples(playlistGuid: string) {
        const response = await getByPlaylist(playlistGuid);
        setPlaylistSamples(response);
    }

    async function fetchUserSamples(userGuid: string) {
        const response = await getUserSamples(userGuid);
        setPlaylistSamples(response);
    }

    useEffect(() => {
        signOut();
        void fetchUser();
    }, [nickname]);

    useEffect(() => {
        if (user != null)
            void fetchUserPlaylist();
    }, [user]);

    useEffect(() => {
        if (selectedPlaylist) {
            if (selectedPlaylist.playlistGuid === user!.userGuid) {
                void fetchUserSamples(user!.userGuid);
                return;
            }

            void fetchPlaylistSamples(selectedPlaylist.playlistGuid);
        }
    }, [selectedPlaylist]);

    if (user === undefined)
        return <LoadingSpinner/>

    if (user === null)
        return <NotFoundPage/>

    return (
        <>
            <div className={profilePageClasses.profilePanel}>
                <div className="horizontalPanel">
                    <UserAvatar height={225} src={user.avatarPath}/>
                    <div className="verticalPanel">
                        <h1>{user.nickname}</h1>
                        <h2>{user.email}</h2>

                        {user.userGuid === loginUser?.userGuid &&
                            <div className="horizontalPanel">
                                {/*<Button primary={true}*/}
                                {/*        onClick={() => setStatisticsIsOpen(true)}>*/}
                                {/*    Статистика*/}
                                {/*</Button>*/}

                                <Button primary={false}
                                        onClick={() => setEditProfileIsOpen(true)}>
                                    Редактировать
                                </Button>

                                <Button warning={true}
                                        onClick={() => setConfirmIsOpen(true)}>
                                    Выйти
                                </Button>
                            </div>}

                    </div>
                </div>

                <div className={profilePageClasses.playlistsPanel + " horizontalPanel"}>

                    {user.userGuid === loginUser?.userGuid &&
                        <PlaylistToolsProfilePanel selectedPlaylist={selectedPlaylist!} onCreate={fetchUserPlaylist}
                                                   onEdit={fetchUserPlaylist}/>}

                    <div className={profilePageClasses.playlists + " horizontalPanel"}>
                        {userPlaylists ?
                            <>
                                {userPlaylists?.map(playlist => <RadioButton
                                    withHorizontalScroll={true}
                                    onSelected={() => setSelectedPlaylist(playlist)}
                                    selected={selectedPlaylist === playlist}
                                    children={playlist.name}
                                    key={playlist.playlistGuid}/>)}
                            </>
                            : <LoadingSpinner/>}
                    </div>
                </div>

                {playlistSamples && <SampleList samples={playlistSamples}/>}
            </div>

            <Modal open={editProfileIsOpen || confirmIsOpen}>
                {/*<StatisticsModal samples={userSamples} onClose={() => setStatisticsIsOpen(false)}/>*/}
                {editProfileIsOpen && <EditProfileModal onCancel={() => setEditProfileIsOpen(false)}
                                                        onSuccess={handleEditProfileOnSuccess}
                                                        onDelete={handleEditProfileOnDelete}/>}
                {confirmIsOpen && <ConfirmModal message={"Будет выполнени выход из аккаунта"}
                                                onConfirm={handleSignOutConfirm}
                                                onCancel={() => setConfirmIsOpen(false)}/>}
            </Modal>
        </>
    )
}