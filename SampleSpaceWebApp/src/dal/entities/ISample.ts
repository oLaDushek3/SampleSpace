import {Guid} from "guid-typescript";

export default interface ISample {
    sampleGuid: Guid;
    sampleLink: string;
    coverLink: string;
    name: string;
    artist: string;
    duration: number;
    vkontakteLink: string;
    spotifyLink: string;
    soundcloudLink: string;
    numberOfListens: number;
}