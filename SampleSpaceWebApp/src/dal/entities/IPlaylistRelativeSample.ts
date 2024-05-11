import IPlaylist from "./IPlaylist.ts";

export default interface IPlaylistRelativeSample {
    playlistGuid: string;
    contain: boolean;
    playlist: IPlaylist;
}