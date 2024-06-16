import IPlaylist from "../../entities/IPlaylist.ts";
import IPlaylistRelativeSample from "../../entities/IPlaylistRelativeSample.ts";
import ICreatePlaylistBlank from "../blanks/playlist/ICreatePlaylistBlank.ts";
import IEditPlaylistBlank from "../blanks/playlist/IEditPlaylistBlank.ts";
import IAddSampleToPlaylistBlank from "../blanks/playlist/IAddSampleToPlaylistBlank.ts";
import useApiBase from "../useApiBase.ts";

interface usePlaylistApiType{
    getUserPlaylists: (userGuid: string) => Promise<IPlaylist[]>,
    getUserPlaylistsRelativeSample: (userGuid: string, sampleGuid: string) => Promise<IPlaylistRelativeSample[]>,
    createPlaylist: (userGuid: string, name: string) => Promise<string>,
    editPlaylist: (playlistGuid: string, name: string) => Promise<boolean>,
    addSampleToPlaylist: (playlistGuid: string, sampleGuid: string) => Promise<boolean>,
    deleteSampleFromPlaylist: (playlistGuid: string, sampleGuid: string) => Promise<boolean>,
    deletePlaylist: (playlistGuid: string) => Promise<boolean>
}

export default function usePlaylistApi(): usePlaylistApiType{
    const {baseAddress, get , post, put, del} = useApiBase();

    const getUserPlaylists = async (userGuid: string): Promise<Array<IPlaylist>> => {
        let url = baseAddress + `playlist/get-user-playlists?user-guid=${userGuid}`;
        return await get(url);
    }

    const getUserPlaylistsRelativeSample = async (userGuid: string, sampleGuid: string): Promise<Array<IPlaylistRelativeSample>> => {
        let url = baseAddress + `playlist/get-user-playlists-relative-sample?user-guid=${userGuid}&sample-guid=${sampleGuid}`;
        return await get(url);
    }

    const createPlaylist = async (userGuid: string, name: string): Promise<string> => {
        let url = baseAddress + "playlist/create-playlist";
        let blank: ICreatePlaylistBlank = {userGuid, name};
        return await post(url, blank);
    }

    const editPlaylist = async (playlistGuid: string, name: string): Promise<boolean> => {
        let url = baseAddress + "playlist/edit-playlist";
        let blank: IEditPlaylistBlank = {playlistGuid, name};
        return await put(url, blank);
    }

    const addSampleToPlaylist = async (playlistGuid: string, sampleGuid: string): Promise<boolean> => {
        let url = baseAddress + "playlist/add-sample-to-playlist";
        let blank: IAddSampleToPlaylistBlank = {playlistGuid, sampleGuid};
        return await post(url, blank);
    }

    const deleteSampleFromPlaylist = async (playlistGuid: string, sampleGuid: string): Promise<boolean> => {
        let url = baseAddress + `playlist/delete-sample-from-playlist?playlist-guid=${playlistGuid}&sample-guid=${sampleGuid}`;
        return await del(url);
    }

    const deletePlaylist = async (playlistGuid: string): Promise<boolean> => {
        let url = baseAddress + `playlist/delete-playlist?playlist-guid=${playlistGuid}`;
        return await del(url);
    }
    
    return {
        getUserPlaylists,
        getUserPlaylistsRelativeSample,
        createPlaylist,
        editPlaylist,
        addSampleToPlaylist,
        deleteSampleFromPlaylist,
        deletePlaylist
    }
}