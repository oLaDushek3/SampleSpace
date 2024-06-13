export default interface IUser {
    userGuid: string
    nickname: string;
    email: string;
    password: string;
    avatarPath: string;
    isAdmin: boolean;
}