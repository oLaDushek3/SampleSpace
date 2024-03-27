import axios from 'axios';
import ApiBase from "../ApiBase";
import ISample from "../../models/ISample.ts";
import {Guid} from "guid-typescript";

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

    static async searchSamples(searchString: string): Promise<Array<ISample>> {

        let url = this.baseAddress + "Sample/SearchSamples?search_string=" + searchString;

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }

    static async getUserSamples(userGuid: string): Promise<Array<ISample>> {

        let url = this.baseAddress + "Sample/GetUserSamples?user_guid=" + userGuid;

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }

    static async addAnListensToSample(sampleGuid: Guid): Promise<boolean> {

        let url = this.baseAddress + "Sample/AddAnListensToSample?sample_guid=" + sampleGuid;

        return await axios.get(url)
            .then(async () => {
                return true;
            }).catch(() => {
                return false;
            })
    }

    static async generateWord(userGuid: string): Promise<boolean> {

        let url = this.baseAddress + "Sample/GenerateWord?user_guid=" + userGuid;

        return await axios.get(url)
            .then(async () => {
                window.open(url)
                return true;
            }).catch(() => {
                return false;
            })
    }

    static async generateExcel(userGuid: string): Promise<boolean> {

        let url = this.baseAddress + "Sample/GenerateExcel?user_guid=" + userGuid;

        return await axios.get(url)
            .then(async () => {
                window.open(url)
                return true;
            }).catch(() => {
                return false;
            })
    }
}