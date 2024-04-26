import {Guid} from "guid-typescript";

export default interface IUser {
    userGuid: Guid
    nickname: string;
    email: string;
    password: string;
    avatarPath: string;
}