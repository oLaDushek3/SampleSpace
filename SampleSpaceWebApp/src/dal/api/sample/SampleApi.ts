import axios from 'axios';
import ApiBase from "../ApiBase";
import ISample from "../../models/ISample.ts";

export default class UserApi extends ApiBase {

    static async getAllSamples(): Promise<Array<ISample>> {

        let url = this.baseAddress + "Sample/GetAllSamples";

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }
}