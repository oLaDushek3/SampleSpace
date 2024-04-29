import axios from 'axios';
import ApiBase from "../ApiBase";
import IUserBlank from "../blanks/IUserBlank";
import ILoginBlank from "../blanks/ILoginBlank";
import IUser from "../../entities/IUser.ts";

export default class UserApi extends ApiBase {

    static async signUp(nickname: string, email: string, password: string): Promise<boolean> {

        let url = this.baseAddress + "user/sign-up";

        let blank: IUserBlank = {nickname, email, password};

        return await axios.post(url, blank)
            .then(async res => {
                console.log(res)
                return res.status === 200;
            })
            .catch(() => {
                return false;
            })
    }

    static async signIn(nickname: string, password: string): Promise<IUser> {

        let url = this.baseAddress + "user/sign-in";

        let blank: ILoginBlank = {nickname, password};

        return await axios.post(url, blank)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return null;
            })
    }

    static async getUser(nickname: string): Promise<IUser> {

        let url = this.baseAddress + `user/get-user-by-nickname?nickname=${nickname}`;

        return await axios.post(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return null;
            })
    }
}