import {Guid} from "guid-typescript";
import IUser from "./IUser.ts";

export default interface ISample {
    sampleGuid: Guid;
    sampleLink: string;
    coverLink: string;
    name: string;
    artist: string;
    userGuid: Guid
    duration: number;
    vkontakteLink: string;
    spotifyLink: string;
    soundcloudLink: string;
    numberOfListens: number;
    user: IUser;
}