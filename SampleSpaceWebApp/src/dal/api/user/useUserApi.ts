import ILoginBlank from "../blanks/user/ILoginBlank";
import IUser from "../../entities/IUser.ts";
import useApiBase from "../useApiBase.ts";
import IForgotPassword from "../blanks/user/IForgotPassword.ts";
import IResetPassword from "../blanks/user/IResetPassword.ts";
import ISampleAdditionStatistic from "../../entities/ISampleAdditionStatistic.ts";

interface useUserApiType {
    signUp: (nickname: string, email: string, password: string, avatarBlob?: Blob) => Promise<boolean | string>,
    signIn: (nickname: string, password: string) => Promise<IUser | string>,
    signOut: () => Promise<IUser | string>,
    editUser: (userGuid: string, avatarBlob?: Blob, nickname?: string, email?: string) => Promise<IUser | string>
    forgotPassword: (route: string, email: string) => Promise<IUser | string>,
    resetPassword: (resetToken: string, newPassword: string) => Promise<IUser | string>,
    deleteUser: (userGuid: string) => Promise<IUser | string>,
    getUser: (nickname: string) => Promise<IUser | string>,
    getSampleAdditionStatistics: (userGuid: string) => Promise<ISampleAdditionStatistic[] | string>
}

export default function useUserApi(): useUserApiType {
    const {baseAddress, get, post, put, del} = useApiBase();

    const signUp = async (nickname: string, email: string, password: string, avatarBlob?: Blob): Promise<boolean | string> => {
        let url = baseAddress + "user/sign-up";

        const formData = new FormData();
        if(avatarBlob)
            formData.append('AvatarFile', avatarBlob);
        formData.append('Nickname', nickname);
        formData.append('Email', email);
        formData.append('Password', password);

        return await post(url, formData);
    }

    const signIn = async (nickname: string, password: string): Promise<IUser | string> => {
        let url = baseAddress + "user/sign-in";
        let blank: ILoginBlank = {nickname, password};
        return await post(url, blank);
    }

    const signOut = async (): Promise<IUser> => {
        let url = baseAddress + "user/sign-out";
        return await post(url);
    }

    const editUser = async (userGuid: string, avatarBlob?: Blob, nickname?: string, email?: string): Promise<IUser | string> => {
        let url = baseAddress + `user/edit-user`;

        const formData = new FormData();
        formData.append('UserGuid', userGuid);
        formData.append('AvatarFile', avatarBlob ? avatarBlob : "");
        formData.append('Nickname', nickname ? nickname : "");
        formData.append('Email', email ? email : "");

        return await put(url, formData);
    }

    const forgotPassword = async (route: string, email: string): Promise<IUser | string> => {
        let url = baseAddress + `user/forgot-password`;
        let blank: IForgotPassword = {route, email};
        return await post(url, blank);
    }

    const resetPassword = async (resetToken: string, newPassword: string): Promise<IUser | string> => {
        let url = baseAddress + `user/reset-password`;
        let blank: IResetPassword = {resetToken, newPassword};
        return await put(url, blank);
    }

    const deleteUser = async (userGuid: string): Promise<IUser | string> => {
        let url = baseAddress + `user/delete-user?user-guid=${userGuid}`;
        return await del(url);
    }

    const getUser = async (nickname: string): Promise<IUser | string> => {
        let url = baseAddress + `user/get-user-by-nickname?nickname=${nickname}`;
        return await get(url);
    }

    const getSampleAdditionStatistics = async (userGuid: string): Promise<ISampleAdditionStatistic[] | string> => {
        let url = baseAddress + `user/get-sample-addition-statistics?user-guid=${userGuid}`
        return await get(url);
    }
    
    return {
        signUp, 
        signIn, 
        signOut, 
        editUser, 
        forgotPassword,
        resetPassword, 
        deleteUser, 
        getUser,
        getSampleAdditionStatistics
    };
}