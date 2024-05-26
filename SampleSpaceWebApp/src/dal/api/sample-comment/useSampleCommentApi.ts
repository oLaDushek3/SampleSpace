import ICreateSampleCommentBlank from "../blanks/sample-comment/ICreateSampleCommentBlank.ts";
import ISampleComment from "../../entities/ISampleComment.ts";
import IEditSampleCommentBlank from "../blanks/sample-comment/IEditSampleCommentBlank.ts";
import useApiBase from "../useApiBase.ts";

interface useSampleCommentApiType {
    getSampleComments: Function,
    createNewComment: Function,
    editComment: Function,
    deleteComment: Function
}

export default  function useSampleCommentApi(): useSampleCommentApiType {
    const {baseAddress, get, post, put, del} = useApiBase()

    const getSampleComments = async (sampleGuid: string): Promise<Array<ISampleComment>> => {
        let url= baseAddress + `sample-comment/get-comments?sample-guid=${sampleGuid}`;
        return await get(url);
    }

    const createNewComment = async (sampleGuid: string, userGuid: string, comment: string): Promise<string> => {
        let url= baseAddress + "sample-comment/create-comment";
        let blank: ICreateSampleCommentBlank = {sampleGuid, userGuid, comment};
        return await post(url, blank);
    }

    const editComment = async (commentGuid: string, comment: string): Promise<boolean> => {
        let url= baseAddress + "sample-comment/edit-comment";
        let blank: IEditSampleCommentBlank = {commentGuid, comment};
        return await put(url, blank);
    }

    const deleteComment = async (commentGuid: string): Promise<boolean> => {
        let url = baseAddress + `sample-comment/delete-comment?comment-guid=${commentGuid}`;
        return await del(url);
    }
    
    return {getSampleComments, createNewComment, editComment, deleteComment}
}