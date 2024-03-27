import {Guid} from "guid-typescript";

export default interface ISample {
    sampleGuid: Guid;
    samplePath: string;
    coverPath: string;
    name: string;
    artist: string;
    numberOfListens: number;
}