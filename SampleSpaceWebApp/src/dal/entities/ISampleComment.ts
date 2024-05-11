import IUser from "./IUser.ts";

export default interface ISampleComment {
    sampleCommentGuid: string;
    sampleGuid: string;
    userGuid: string;
    date: Date;
    comment: string;
    user: IUser;
}