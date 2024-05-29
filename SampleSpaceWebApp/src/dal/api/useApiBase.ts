import axios from "axios";
import useAuth from "../../hook/useAuth.ts";
import {useNavigate} from "react-router-dom";

interface useApiBaseType {
    baseAddress: string,
    get: Function,
    post: Function,
    put: Function,
    del: Function
}

export default function useApiBase(): useApiBaseType {
    const {delUser} = useAuth();
    const navigate = useNavigate()

    const baseAddress = "http://localhost:5000/api/"

    const get = async (url: string) => {
        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401){
                    navigate({pathname: "/"});
                    delUser();
                }
                
                if(responseStatus === 409)
                    return responseStatus;
                
                return null;
            })
    }

    const post = async (url: string, blank: any) => {
        return await axios.post(url, blank)
            .then(async res => {
                return res.data ? res.data : true;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401){
                    navigate({pathname: "/"});
                    delUser();
                }

                if(responseStatus === 409)
                    return responseStatus;
                
                return null;
            })
    }

    const put = async (url: string, blank: any) => {
        return await axios.put(url, blank)
            .then(async res => {
                return res.data ? res.data : true;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401){
                    navigate({pathname: "/"});
                    delUser();
                }

                if(responseStatus === 409)
                    return responseStatus;

                return null;
            })
    }

    const del = async(url: string) => {
        return await axios.delete(url)
            .then(async res => {
                return res.data ? res.data : true;
            })
            .catch((error) => {
                const responseStatus = error.response.status;
                if(responseStatus === 401){
                    navigate({pathname: "/"});
                    delUser();
                }

                if(responseStatus === 409)
                    return responseStatus;

                return null;
            })
    }
    
    return {baseAddress, get, post: post, put: put, del: del};
}