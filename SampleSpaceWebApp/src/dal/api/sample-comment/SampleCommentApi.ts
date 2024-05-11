import axios from 'axios';
import ApiBase from "../ApiBase";
import ICreateSampleCommentBlank from "../blanks/sample-comment/ICreateSampleCommentBlank.ts";
import ISampleComment from "../../entities/ISampleComment.ts";
import IEditSampleCommentBlank from "../blanks/sample-comment/IEditSampleCommentBlank.ts";

export default class SampleCommentApi extends ApiBase {
    static async getSampleComments(sampleGuid: string): Promise<Array<ISampleComment>> {

        let url = this.baseAddress + `sample-comment/get-comments?sample-guid=${sampleGuid}`;

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }
    
    static async createNewComment(sampleGuid: string, userGuid: string, comment: string): Promise<string> {

        let url = this.baseAddress + "sample-comment/create-comment";   
        
        let blank: ICreateSampleCommentBlank = {sampleGuid, userGuid, comment};

        return await axios.post(url, blank)
            .then(async res => {
                return res.data;
            })
            .catch(() => {
                return false;
            })
    }

    static async editComment(commentGuid: string, comment: string): Promise<boolean> {

        let url = this.baseAddress + "sample-comment/edit-comment";

        let blank: IEditSampleCommentBlank = {commentGuid, comment};

        return await axios.put(url, blank)
            .then(() => {
                return true;
            })
            .catch(() => {
                return false;
            })
    }
    
    static async deleteComment(commentGuid: string): Promise<boolean> {
        
        let url = this.baseAddress + `sample-comment/delete-comment?comment-guid=${commentGuid}`;

        return await axios.delete(url)
            .then(()  => {
                return true;
            })
            .catch(() => {
                return false;
            })
    }
}