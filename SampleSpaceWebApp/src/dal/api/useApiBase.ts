import axios from "axios";
import useAuth from "../../hook/useAuth.ts";
import {useNavigate} from "react-router-dom";
import useInformModal from "../../hook/useInformModal.ts";

interface useApiBaseType {
    baseAddress: string,
    get: (url: string) => Promise<any>,
    post: (url: string, blank?: any) => Promise<any>,
    put: (url: string, blank: any) => Promise<any>,
    del: (url: string) => Promise<any>,
    download: (url: string) => Promise<boolean>
}

export default function useApiBase(): useApiBaseType {
    const {delUser} = useAuth();
    const navigate = useNavigate();
    const {showInform} = useInformModal();

    const baseAddress = "http://158.160.171.213/api/";

    const get = async (url: string): Promise<any> => {
        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((error) => {
                return handleAxiosError(error);
            })
    }

    const post = async (url: string, blank?: any): Promise<any> => {
        return await axios.post(url, blank)
            .then(async res => {
                return res.data ? res.data : true;
            })
            .catch((error) => {
                return handleAxiosError(error);
            })
    }

    const put = async (url: string, blank: any): Promise<any> => {
        return await axios.put(url, blank)
            .then(async res => {
                return res.data ? res.data : true;
            })
            .catch((error) => {
                return handleAxiosError(error);
            })
    }

    const del = async(url: string): Promise<any> => {
        return await axios.delete(url)
            .then(async res => {
                return res.data ? res.data : true;
            })
            .catch((error) => {
                return handleAxiosError(error);
            })
    }
    
    const download = async (url: string): Promise<boolean> => {
        window.open(url);
        return true;
    }
    
    const handleAxiosError = (error : null | any) => {
        if(error.code === "ERR_NETWORK" || error.code === "ERR_BAD_RESPONSE"){
            showInform("Техническая неисправность на сервере. Пожалуйста попробуйте позже");
            return null;
        }

        const responseStatus = error.response.status;

        if(responseStatus === 401)
            handleUnauthorizedResponse();

        if(responseStatus === 400)
            return null;

        return responseStatus;
    }
    
    const handleUnauthorizedResponse = () => {
        navigate({pathname: "/"});
        delUser();
        showInform("Ошибка аутентификации");
    }
 
    return {baseAddress, get, post, put, del, download};
}