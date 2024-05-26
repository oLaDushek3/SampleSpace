import {useEffect, useState} from "react";
import playlistSamplePanelClasses from "./PlaylistSamplePanel.module.css";
import useAuth from "../../../hook/useAuth.ts";
import Button from "../../button/Button.tsx";
import LoadingSpinner from "../../loading-spinner/LoadingSpinner.tsx";
import IPlaylistRelativeSample from "../../../dal/entities/IPlaylistRelativeSample.ts";
import usePlaylistApi from "../../../dal/api/playlist/usePlaylistApi.ts";

interface PlaylistSamplePanelProps {
    isActive: boolean;
    sampleGuid: string;
}

export default function PlaylistSamplePanel({isActive = false, sampleGuid}: PlaylistSamplePanelProps) {
    const {addSampleToPlaylist, deleteSampleFromPlaylist, getUserPlaylistsRelativeSample} = usePlaylistApi();
    const [classes, setClasses] = useState(playlistSamplePanelClasses.playlistPanel)
    const [playlists, setPlaylists] = useState<IPlaylistRelativeSample[]>()
    const {loginUser} = useAuth();

    useEffect(() => {
        if (isActive)
            setClasses(`${playlistSamplePanelClasses.playlistPanel} ${playlistSamplePanelClasses.active}`);
        else
            setClasses(playlistSamplePanelClasses.playlistPanel);

    }, [isActive]);

    async function fetchPlaylists() {
        const response = await getUserPlaylistsRelativeSample(loginUser!.userGuid, sampleGuid);
        setPlaylists(response);
    }

    useEffect(() => {
        void fetchPlaylists();
    }, []);

    if (playlists === undefined) {
        return (
            <div className={classes}>
                <LoadingSpinner/>
            </div>
        )
    }

    const handleAddSampleToPlaylist = async (playlistGuid: string) => {
        void await addSampleToPlaylist(playlistGuid, sampleGuid);
        void fetchPlaylists();
    }

    const handleRemoveSampleToPlaylist = async (playlistGuid: string) => {
        void await deleteSampleFromPlaylist(playlistGuid, sampleGuid);
        void fetchPlaylists();
    }
    
    return (
        <div className={classes}>
            {playlists.map(playlist => <Button primary={playlist.contain}
                                               alone={true}
                                               children={playlist.playlist.name}
                                               onClick={playlist.contain ? 
                                                   () => handleRemoveSampleToPlaylist(playlist.playlistGuid) 
                                                   : () => handleAddSampleToPlaylist(playlist.playlistGuid)}
                                               key={playlist.playlistGuid.toString()}/>)}
        </div>
    )
}