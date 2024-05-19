import axios from 'axios';
import ApiBase from "../ApiBase";
import ISample from "../../entities/ISample.ts";

export default class SampleApi extends ApiBase {

    static async getAllSamples(): Promise<Array<ISample>> {

        let url = this.baseAddress + "sample/get-all-samples";

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }

    static async searchSamples(searchString: string): Promise<Array<ISample>> {

        let url = this.baseAddress + `sample/search-samples?search-string=${searchString}`

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }

    static async getByPlaylist(playlistGuid: string): Promise<Array<ISample>> {

        let url = this.baseAddress + `sample/get-by-playlist?playlist-guid=${playlistGuid}`

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }

    static async getSample(sampleGuid: string): Promise<ISample | null> {

        let url = this.baseAddress + `sample/get-sample?sample-guid=${sampleGuid}`

        return axios.get(url).then(async res => {
            return res.data;
        }).catch(() => {
            return null;
        })
    }

    static async getUserSamples(userGuid: string): Promise<Array<ISample>> {

        let url = this.baseAddress + `sample/get-user-samples?user-guid=${userGuid}`

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return [];
            })
    }

    static async addAnListensToSample(sampleGuid: string): Promise<boolean> {

        let url = this.baseAddress + `sample/add-an-listens-to-sample?sample-guid=${sampleGuid}`;

        return await axios.get(url)
            .then(async () => {
                return true;
            }).catch(() => {
                return false;
            })
    }

    static async generateWord(userGuid: string): Promise<boolean> {

        let url = this.baseAddress + `sample/generate-word?user-guid=${userGuid}`

        return await axios.get(url)
            .then(async () => {
                window.open(url)
                return true;
            }).catch(() => {
                return false;
            })
    }

    static async generateExcel(userGuid: string): Promise<boolean> {

        let url = this.baseAddress + `sample/generate-excel?user-guid=${userGuid}`

        return await axios.get(url)
            .then(async () => {
                window.open(url)
                return true;
            }).catch(() => {
                return false;
            })
    }

    static async createSample(uploadedSampleFile: File, sampleStart: number, sampleEnd: number, 
                              coverBlob: Blob, sampleName: string, sampleArtist: string, 
                              userGuid: string, vkontakteLink: string, spotifyLink: string, 
                              soundcloudLink: string): Promise<boolean> {
        let url = this.baseAddress + `sample/create-sample`

        const formData = new FormData();
        formData.append('SampleFile', uploadedSampleFile);
        formData.append('SampleStart', sampleStart.toString());
        formData.append('SampleEnd', sampleEnd.toString());
        formData.append('CoverFile', coverBlob);
        formData.append('Name', sampleName);
        formData.append('Artist', sampleArtist);
        formData.append('UserGuid', userGuid);
        formData.append('VkontakteLink', vkontakteLink);
        formData.append('SpotifyLink', spotifyLink);
        formData.append('SoundcloudLink', soundcloudLink);


        return await axios.post(url, formData)
            .then(async () => {
                return true;
            }).catch(() => {
                return false;
            })
    }
}