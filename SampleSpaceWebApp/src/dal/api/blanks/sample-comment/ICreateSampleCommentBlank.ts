import {Guid} from "guid-typescript";

export default interface ICreateSampleCommentBlank {
    sampleGuid: Guid;
    userGuid: Guid;
    comment: string;
}