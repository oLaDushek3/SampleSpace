import ILoginBlank from "../blanks/user/ILoginBlank";
import IUser from "../../entities/IUser.ts";
import useApiBase from "../useApiBase.ts";

interface useUserApiType {
    signUp: Function,
    signIn: Function,
    signOut: Function,
    getUser: Function
}

export default function useUserApi(): useUserApiType {
    const {baseAddress, get, post} = useApiBase();
    
    const signUp = async (avatarBlob: Blob, nickname: string, email: string, password: string): Promise<boolean | string> => {
        let url = baseAddress + "user/sign-up";

        const formData = new FormData();
        formData.append('AvatarFile', avatarBlob);
        formData.append('Nickname', nickname);
        formData.append('Email', email);
        formData.append('Password', password);
        
        return await post(url, formData);
    }

    const signIn = async (nickname: string, password: string): Promise<IUser> => {
        let url = baseAddress + "user/sign-in";
        let blank: ILoginBlank = {nickname, password};
        return await post(url, blank);
    }

    const signOut = async (): Promise<IUser> => {
        let url = baseAddress + "user/sign-out";
        return await post(url);
    }

    const getUser = async (nickname: string): Promise<IUser>  => {
        let url = baseAddress + `user/get-user-by-nickname?nickname=${nickname}`;
        return await get(url);
    }
    
    return {signUp, signIn, signOut, getUser};
}