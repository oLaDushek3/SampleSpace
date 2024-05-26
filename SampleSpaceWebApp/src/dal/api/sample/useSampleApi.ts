import ISample from "../../entities/ISample.ts";
import useApiBase from "../useApiBase.ts";

interface useSampleApiType {
    getAllSamples: Function,
    searchSamples: Function,
    getByPlaylist: Function,
    getSample: Function,
    getUserSamples: Function,
    addAnListensToSample: Function,
    generateWord: Function,
    generateExcel: Function,
    createSample: Function
}

export default function useSampleApi(): useSampleApiType {
    const {baseAddress, get, post} = useApiBase();

    const getAllSamples = async (): Promise<Array<ISample>> => {
        let url = baseAddress + "sample/get-all-samples";
        return await get(url);
    }

    const searchSamples = async (searchString: string): Promise<Array<ISample>> => {
        let url = baseAddress + `sample/search-samples?search-string=${searchString}`
        return await get(url);
    }

    const getByPlaylist = async (playlistGuid: string): Promise<Array<ISample>> => {
        let url = baseAddress + `sample/get-by-playlist?playlist-guid=${playlistGuid}`
        return await get(url);
    }

    const getSample = async (sampleGuid: string): Promise<ISample | null> => {
        let url = baseAddress + `sample/get-sample?sample-guid=${sampleGuid}`
        return await get(url);
    }

    const getUserSamples = async (userGuid: string): Promise<Array<ISample>> => {
        let url = baseAddress + `sample/get-user-samples?user-guid=${userGuid}`
        return await get(url);
    }

    const addAnListensToSample = async (sampleGuid: string): Promise<boolean> => {
        let url = baseAddress + `sample/add-an-listens-to-sample?sample-guid=${sampleGuid}`;
        return await get(url);
    }

    const generateWord = async (userGuid: string): Promise<boolean> => {
        let url = baseAddress + `sample/generate-word?user-guid=${userGuid}`
        return await get(url);
    }

    const generateExcel = async (userGuid: string): Promise<boolean> => {
        let url = baseAddress + `sample/generate-excel?user-guid=${userGuid}`
        return await get(url);
    }

    const createSample = async (uploadedSampleFile: File, sampleStart: number, sampleEnd: number,
                                coverBlob: Blob, sampleName: string, sampleArtist: string,
                                userGuid: string, vkontakteLink: string, spotifyLink: string,
                                soundcloudLink: string): Promise<boolean> => {
        let url = baseAddress + `sample/create-sample`

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

        return await post(url, formData);
    }

    return {
        getAllSamples,
        searchSamples,
        getByPlaylist,
        getSample,
        getUserSamples,
        addAnListensToSample,
        generateWord,
        generateExcel,
        createSample
    }
}