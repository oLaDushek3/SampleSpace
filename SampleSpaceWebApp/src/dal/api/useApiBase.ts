import axios from "axios";
import useAuth from "../../hook/useAuth.ts";

interface useApiBaseType {
    baseAddress: string,
    get: Function,
    post: Function,
    put: Function,
    del: Function
}

export default function useApiBase(): useApiBaseType {
    const {delUser} = useAuth();

    const baseAddress = "http://localhost:5000/api/"

    const get = async (url: string) => {
        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401)
                    delUser();
                
                if(responseStatus === 409)
                    return responseStatus;
                
                return null;
            })
    }

    const post = async (url: string, blank: any) => {
        return await axios.post(url, blank)
            .then(async res => {
                return res.data;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401)
                    delUser();

                if(responseStatus === 409)
                    return responseStatus;

                return responseStatus === 200;
            })
    }

    const put = async (url: string, blank: any) => {
        return await axios.put(url, blank)
            .then(_ => {
                return true;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401)
                    delUser();

                if(responseStatus === 409)
                    return responseStatus;

                return responseStatus === 200;
            })
    }

    const del = async(url: string) => {
        return await axios.delete(url)
            .then(_ => {
                return true;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401)
                    delUser();

                if(responseStatus === 409)
                    return responseStatus;

                return responseStatus === 200;
            })
    }
    
    return {baseAddress, get, post: post, put: put, del: del};
}