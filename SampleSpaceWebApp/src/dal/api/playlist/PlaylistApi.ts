import axios from 'axios';
import ApiBase from "../ApiBase";
import IPlaylist from "../../entities/IPlaylist.ts";
import IPlaylistRelativeSample from "../../entities/IPlaylistRelativeSample.ts";
import IEditPlaylistBlank from "../blanks/sample-comment/playlist-blank/IEditPlaylistBlank.ts";
import IAddSampleToPlaylistBlank from "../blanks/sample-comment/playlist-blank/IAddSampleToPlaylistBlank.ts";
import ICreatePlaylistBlank from "../blanks/sample-comment/playlist-blank/ICreatePlaylistBlank.ts";

export default class PlaylistApi extends ApiBase {
    static async getUserPlaylists(userGuid: string): Promise<Array<IPlaylist>> {

        let url = this.baseAddress + `playlist/get-user-playlists?user-guid=${userGuid}`;

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async getUserPlaylistsRelativeSample(userGuid: string, sampleGuid: string): Promise<Array<IPlaylistRelativeSample>> {

        let url = this.baseAddress + `playlist/get-user-playlists-relative-sample?user-guid=${userGuid}&sample-guid=${sampleGuid}`;

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async createPlaylist(userGuid: string, name: string): Promise<string> {

        let url = this.baseAddress + "playlist/create-playlist";

        let blank: ICreatePlaylistBlank = {userGuid, name};

        return await axios.post(url, blank)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async editPlaylist(playlistGuid: string, name: string): Promise<string> {

        let url = this.baseAddress + "playlist/edit-playlist";

        let blank: IEditPlaylistBlank = {playlistGuid, name};

        return await axios.put(url, blank)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async addSampleToPlaylist(playlistGuid: string, sampleGuid: string): Promise<string> {

        let url = this.baseAddress + "playlist/add-sample-to-playlist";
        
        let blank: IAddSampleToPlaylistBlank = {playlistGuid, sampleGuid};
        
        return await axios.post(url, blank)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async deleteSampleFromPlaylist(playlistGuid: string, sampleGuid: string): Promise<string> {

        let url = this.baseAddress + `playlist/delete-sample-from-playlist?playlist-guid=${playlistGuid}&sample-guid=${sampleGuid}`;

        return await axios.delete(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async deletePlaylist(playlistGuid: string): Promise<string> {

        let url = this.baseAddress + `playlist/delete-playlist?playlist-guid=${playlistGuid}`;

        return await axios.delete(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }
}