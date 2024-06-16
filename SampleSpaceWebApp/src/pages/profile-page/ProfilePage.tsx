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
import StatisticsModal from "../../components/statistics/StatisticsModal.tsx";

export default function ProfilePage() {
    const navigate = useNavigate()
    const {getUser, signOut} = useUserApi();
    const {getByPlaylist, getUserSamples, deleteSample} = useSampleApi();
    const {getUserPlaylists, deleteSampleFromPlaylist} = usePlaylistApi();
    const [fetchFunction, setFetchFunction] = useState<(numberOfPage: number) => Promise<ISample[]>>()

    const {nickname} = useParams<{ nickname: string }>();
    const [user, setUser] = useState<IUser | null>();
    const {loginUser, delUser} = useAuth()
    const [userPlaylists, setUserPlaylists] = useState<IPlaylist[]>();
    const [selectedPlaylist, setSelectedPlaylist] = useState<IPlaylist>();

    const [editProfileIsOpen, setEditProfileIsOpen] = useState(false);
    const [confirmIsOpen, setConfirmIsOpen] = useState(false);
    const [confirmMessage, setConfirmMessage] = useState("");
    const [onConfirm, setOnConfirm] = useState(() => () => {
    })

    const [statisticsIsOpen, setStatisticsIsOpen] = useState(false);

    const handleSignOut = () => {
        setConfirmIsOpen(true);
        setConfirmMessage("Будет выполнен выход из аккаунта");
        setOnConfirm(() => () => handleSignOutConfirm())
    }

    const handleSignOutConfirm = async () => {
        setConfirmIsOpen(false);
        await signOut();
        delUser();
        navigate(`/`);
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

    const handleDeleteSample = async (sampleGuid: string) => {
        setConfirmIsOpen(true);
        setConfirmMessage("Семпл будет удален");
        setOnConfirm(() => () => handleDeleteSampleConfirm(sampleGuid))
    }

    const handleDeleteSampleConfirm = async (sampleGuid: string) => {
        setConfirmIsOpen(false);
        await deleteSample(sampleGuid);
        setFetchFunction(() => (numberOfPage: number) => getUserSamples(user!.userGuid, 15, numberOfPage));
    }

    const handleDeleteSampleFromPlaylist = async (sampleGuid: string) => {
        await deleteSampleFromPlaylist(selectedPlaylist!.playlistGuid, sampleGuid);
        setFetchFunction(() => (numberOfPage: number) => getByPlaylist(selectedPlaylist!.playlistGuid, 15, numberOfPage));
    }

    useEffect(() => {
        void fetchUser();
    }, [nickname]);

    useEffect(() => {
        if (user != null)
            void fetchUserPlaylist();
    }, [user]);

    useEffect(() => {
        if (selectedPlaylist) {
            if (selectedPlaylist.playlistGuid === user!.userGuid) {
                setFetchFunction(() => (numberOfPage: number) => getUserSamples(user!.userGuid, 15, numberOfPage));
                return;
            }

            setFetchFunction(() => (numberOfPage: number) => getByPlaylist(selectedPlaylist.playlistGuid, 15, numberOfPage));
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
                                <Button primary={false}
                                        onClick={() => setStatisticsIsOpen(true)}>
                                    Статистика
                                </Button>

                                <Button primary={false}
                                        onClick={() => setEditProfileIsOpen(true)}>
                                    Редактировать
                                </Button>

                                <Button warning={true}
                                        onClick={handleSignOut}>
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
                                {userPlaylists?.map(playlist =>
                                    <RadioButton
                                        withHorizontalScroll={true}
                                        onSelected={() => setSelectedPlaylist(playlist)}
                                        selected={selectedPlaylist === playlist}
                                        children={playlist.name}
                                        key={playlist.playlistGuid}/>)}
                            </>
                            : <LoadingSpinner/>}
                    </div>
                </div>

                {selectedPlaylist && fetchFunction &&
                    <SampleList onDelete={selectedPlaylist?.playlistGuid === user!.userGuid ?
                        handleDeleteSample :
                        handleDeleteSampleFromPlaylist} fetchFunction={fetchFunction}/>}
            </div>

            <Modal open={editProfileIsOpen || confirmIsOpen || statisticsIsOpen}>
                {statisticsIsOpen && <StatisticsModal userGuid={user.userGuid}
                                                      onClose={() => setStatisticsIsOpen(false)}/>}

                {editProfileIsOpen && <EditProfileModal onCancel={() => setEditProfileIsOpen(false)}
                                                        onSuccess={handleEditProfileOnSuccess}
                                                        onDelete={handleEditProfileOnDelete}/>}

                {confirmIsOpen && <ConfirmModal message={confirmMessage}
                                                onConfirm={onConfirm}
                                                onCancel={() => setConfirmIsOpen(false)}/>}
            </Modal>
        </>
    )
}