import ISample from "../../entities/ISample.ts";
import useApiBase from "../useApiBase.ts";

interface useSampleApiType {
    getAllSamples: () => Promise<ISample[]>,
    searchSamples: (searchString: string) => Promise<ISample[]>,
    getByPlaylist: (playlistGuid: string) => Promise<ISample[]>,
    getSample: (sampleGuid: string) => Promise<ISample | null>,
    getUserSamples: (userGuid: string) => Promise<ISample[]>,
    addAnListensToSample: (sampleGuid: string) => Promise<boolean>,
    createSample: (uploadedSampleFile: File, sampleStart: number, sampleEnd: number,
                   coverBlob: Blob, sampleName: string, sampleArtist: string,
                   userGuid: string, vkontakteLink: string, spotifyLink: string,
                   soundcloudLink: string) => Promise<boolean>,
    deleteSample: (sampleGuid: string) => Promise<boolean>
}

export default function useSampleApi(): useSampleApiType {
    const {baseAddress, get, post, del} = useApiBase();

    const getAllSamples = async (): Promise<ISample[]> => {
        let url = baseAddress + "sample/get-all-samples";
        return await get(url);
    }

    const searchSamples = async (searchString: string): Promise<Array<ISample>> => {
        let url = baseAddress + `sample/search-samples?search-string=${searchString}`
        return await get(url);
    }

    const getByPlaylist = async (playlistGuid: string): Promise<ISample[]> => {
        let url = baseAddress + `sample/get-by-playlist?playlist-guid=${playlistGuid}`
        return await get(url);
    }

    const getSample = async (sampleGuid: string): Promise<ISample | null> => {
        let url = baseAddress + `sample/get-sample?sample-guid=${sampleGuid}`
        return await get(url);
    }

    const getUserSamples = async (userGuid: string): Promise<ISample[]> => {
        let url = baseAddress + `sample/get-user-samples?user-guid=${userGuid}`
        return await get(url);
    }

    const addAnListensToSample = async (sampleGuid: string): Promise<boolean> => {
        let url = baseAddress + `sample/add-an-listens-to-sample?sample-guid=${sampleGuid}`;
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

    const deleteSample = async (sampleGuid: string): Promise<boolean>  => {
        let url = baseAddress + `sample/delete-sample?sample-guid=${sampleGuid}`;
        return await del(url);
    }
    
    return {
        getAllSamples,
        searchSamples,
        getByPlaylist,
        getSample,
        getUserSamples,
        addAnListensToSample,
        createSample,
        deleteSample
    }
}