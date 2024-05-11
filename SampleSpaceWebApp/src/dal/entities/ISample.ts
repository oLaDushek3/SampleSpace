import IUser from "./IUser.ts";

export default interface ISample {
    sampleGuid: string;
    sampleLink: string;
    coverLink: string;
    name: string;
    artist: string;
    userGuid: string
    duration: number;
    vkontakteLink: string;
    spotifyLink: string;
    soundcloudLink: string;
    numberOfListens: number;
    user: IUser;
}