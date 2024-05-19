import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import UserApi from "../../dal/api/user/UserApi.ts";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import profilePageClasses from "./ProfilePage.module.css";
import SampleList from "../../components/sample-list/SampleList.tsx";
import Button, {RadioButton} from "../../components/button/Button.tsx";
import useAuth from "../../hook/useAuth.ts";
import IUser from "../../dal/entities/IUser.ts";
import ISample from "../../dal/entities/ISample.ts";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner.tsx";
import NotFoundPage from "../not-found/NotFoundPage.tsx";
import PlaylistApi from "../../dal/api/playlist/PlaylistApi.ts";
import IPlaylist from "../../dal/entities/IPlaylist.ts";
import PlaylistToolsProfilePanel
    from "../../components/playlist/playlist-tools-profile-panel/PlaylistToolsProfilePanel.tsx";
import Modal from "../../components/modal/Modal.tsx";
import StatisticsModal from "../../components/statistics/StatisticsModal.tsx";

export default function ProfilePage() {
    const {nickname} = useParams<{ nickname: string }>();
    const [user, setUser] = useState<IUser | null>();
    const {signOut} = useAuth()
    const [userPlaylists, setUserPlaylists] = useState<IPlaylist[]>();
    const [selectedPlaylist, setSelectedPlaylist] = useState<IPlaylist>();
    const [playlistSamples, setPlaylistSamples] = useState<ISample[]>();

    const [statisticsIsOpen, setStatisticsIsOpen] = useState(false);

    async function fetchUser() {
        const response = await UserApi.getUser(nickname!);
        setUser(response);
    }

    const [userSamples, setUserSamples] = useState<Array<ISample>>()

    async function fetchUserSamples() {
        const response = await SampleApi.getUserSamples(user!.userGuid);
        setUserSamples(response);
    }

    useEffect(() => {
        fetchUserSamples();
    }, [user]);

    async function fetchUserPlaylist() {
        const response = await PlaylistApi.getUserPlaylists(user!.userGuid.toString())
        setUserPlaylists(response)
        setSelectedPlaylist(response[0]);
    }

    async function fetchPlaylistSamples() {
        const response = await SampleApi.getByPlaylist(selectedPlaylist!.playlistGuid);
        setPlaylistSamples(response);
    }

    useEffect(() => {
        void fetchUser();
    }, []);

    useEffect(() => {
        if (user != null)
            void fetchUserPlaylist();
    }, [user]);

    useEffect(() => {
        if (selectedPlaylist)
            void fetchPlaylistSamples();
    }, [selectedPlaylist]);

    if (user === undefined)
        return <LoadingSpinner/>

    if (user === null)
        return <NotFoundPage/>

    return (
        <>
            <div className={profilePageClasses.profilePanel}>
                <div className="horizontalPanel">
                    <img className={profilePageClasses.avatar} src={user?.avatarPath} alt={"avatar"}/>
                    <div className="verticalPanel">
                        <h1>{user?.nickname}</h1>
                        <h2>{user?.email}</h2>

                        <div className="horizontalPanel">
                            <Button primary={true}
                                    onClick={() => setStatisticsIsOpen(true)}>
                                Статистика
                            </Button>

                            <Button primary={true}
                                    onClick={() => signOut()}>
                                Выйти
                            </Button>
                        </div>
                    </div>
                </div>

                <div className={profilePageClasses.playlistsPanel + " horizontalPanel"}>

                    <PlaylistToolsProfilePanel selectedPlaylist={selectedPlaylist!} onCreate={fetchUserPlaylist}
                                               onEdit={fetchUserPlaylist}/>

                    <div className={profilePageClasses.playlists + " horizontalPanel"}>
                        {userPlaylists ?
                            <>
                                {/*<RadioButton onSelected={}/>*/}

                                {userPlaylists?.map(playlist => <RadioButton
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

            <Modal open={statisticsIsOpen}>
                <StatisticsModal samples={userSamples} onClose={() => setStatisticsIsOpen(false)}/>
            </Modal>
        </>
    )
}