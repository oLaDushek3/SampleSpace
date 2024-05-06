import {Guid} from "guid-typescript";
import IUser from "./IUser.ts";

export default interface ISampleComment {
    sampleCommentGuid: Guid;
    sampleGuid: Guid;
    userGuid: Guid;
    date: Date;
    comment: string;
    user: IUser;
}