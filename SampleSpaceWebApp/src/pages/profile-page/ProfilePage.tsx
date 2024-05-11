import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import UserApi from "../../dal/api/user/UserApi.ts";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import profilePageClasses from "./ProfilePage.module.css";
import SampleList from "../../components/sample-list/SampleList.tsx";
import Button, {ButtonVisualType, RadioButton} from "../../components/button/Button.tsx";
import {MdAdd, MdEdit} from "react-icons/md";
import useAuth from "../../hook/useAuth.ts";
import IUser from "../../dal/entities/IUser.ts";
import ISample from "../../dal/entities/ISample.ts";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner.tsx";
import NotFoundPage from "../not-found/NotFoundPage.tsx";
import PlaylistApi from "../../dal/api/playlist/PlaylistApi.ts";
import IPlaylist from "../../dal/entities/IPlaylist.ts";
import Icon from "../../components/icon/Icon.tsx";
import Modal from "../../components/modal/Modal.tsx";
import CreatePlaylistModal from "../../components/playlist/create-playlist/CreatePlaylistModal.tsx";
import EditPlaylistModal from "../../components/playlist/edit-playlist/EditPlaylistModal.tsx";

interface ProfilePageProps {
}

export default function ProfilePage({}: ProfilePageProps) {
    const {nickname} = useParams<{ nickname: string }>();
    const [user, setUser] = useState<IUser | null>();
    const {signOut} = useAuth()
    const [userPlaylists, setUserPlaylists] = useState<IPlaylist[]>();
    const [selectedPlaylist, setSelectedPlaylist] = useState<IPlaylist>();
    const [playlistSamples, setPlaylistSamples] = useState<ISample[]>();
    const [createPlaylistIsOpen, setCreatePlaylistIsOpen] = useState(false)
    const [editPlaylistIsOpen, setEditPlaylistIsOpen] = useState(false)

    //const [statisticsIsOpen, setStatisticsIsOpen] = useState(false);

    async function fetchUser() {
        const response = await UserApi.getUser(nickname!);
        setUser(response);
    }

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
                            {/*<Button primary={true}*/}
                            {/*        onClick={() => setStatisticsIsOpen(true)}>*/}
                            {/*    Статистика*/}
                            {/*</Button>*/}

                            <Button primary={true}
                                    onClick={() => signOut()}>
                                Выйти
                            </Button>
                        </div>
                    </div>
                </div>

                <div className={profilePageClasses.playlistsPanel + " horizontalPanel"}>
                    <Button visualType={ButtonVisualType.withIcon}
                            onClick={() => setCreatePlaylistIsOpen(true)}>
                        <Icon>
                            <MdAdd/>
                        </Icon>
                    </Button>

                    {selectedPlaylist &&
                        <Button visualType={ButtonVisualType.withIcon}
                                active={selectedPlaylist?.canBeModified}
                                onClick={() => setEditPlaylistIsOpen(true)}>
                            <Icon>
                                <MdEdit/>
                            </Icon>
                        </Button>}
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

            {/*<Modal open={statisticsIsOpen}>*/}
            {/*    <StatisticsModal samples={userSamples} onClose={() => setStatisticsIsOpen(false)}/>*/}
            {/*</Modal>*/}

            <Modal open={createPlaylistIsOpen || editPlaylistIsOpen}>
                {createPlaylistIsOpen && <CreatePlaylistModal onClose={() => setCreatePlaylistIsOpen(false)}
                                                              onCreate={fetchUserPlaylist}/>}
                {editPlaylistIsOpen && <EditPlaylistModal playlist={selectedPlaylist!}
                                                          onClose={() => setEditPlaylistIsOpen(false)}
                                                          onEdit={fetchUserPlaylist}/>}
            </Modal>
        </>

    )
}