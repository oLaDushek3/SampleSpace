import ISampleComment from "../../../dal/entities/ISampleComment.ts";
import commentClasses from "./Comment.module.css";
import {Link} from "react-router-dom";
import UserAvatar from "../../user-avatar/UserAvatar.tsx";
import ShortDate from "../../date/ShortDate.tsx";
import Icon from "../../icon/Icon.tsx";
import {MdEdit, MdClose} from "react-icons/md";
import Button, {ButtonVisualType} from "../../button/Button.tsx";
import useAuth from "../../../hook/useAuth.ts";
import SampleCommentApi from "../../../dal/api/sample-comment/SampleCommentApi.ts";
import {useState} from "react";
import CommentInput, {CommentAction} from "../../comment-input/CommentInput.tsx";

interface CommentProps {
    comment: ISampleComment;
    updateCallBack: Function;
}

export default function Comment({comment, updateCallBack}: CommentProps) {
    const {user} = useAuth();
    const [toolsPanelActive, setToolsPanelActive] = useState(false);
    const [editingActive, setEditingActive] = useState(false)
    
    const handleDeleteComment = async () => {
        await SampleCommentApi.deleteComment(comment.sampleCommentGuid);
        updateCallBack();
    }
    
    const handleEditComment = async (commentValue: string) => {
        await SampleCommentApi.editComment(comment.sampleCommentGuid, commentValue);
        updateCallBack();
        setEditingActive(false);
    }
    
    return (
        <>
            {!editingActive ?
                <div className={"horizontalPanel"}
                     onMouseEnter={() => setToolsPanelActive(true)}
                     onMouseLeave={() => setToolsPanelActive(false)}>
                    <Link to={`/${comment.user.nickname}`}>
                        <UserAvatar src={comment.user.avatarPath} height={32}/>
                    </Link>

                    <div className={commentClasses.commentPanel}>
                        <div className={toolsPanelActive ? commentClasses.header + " " + commentClasses.active : commentClasses.header}>
                            <Link to={`/${comment.user.nickname}`}>
                                <p>{comment.user.nickname}</p>
                            </Link>

                            {user?.userGuid == comment.userGuid &&
                                <div className={commentClasses.toolsPanel + " horizontalPanel"}>
                                    <Button isPrimary={false}
                                            visualType={ButtonVisualType.icon}
                                            onClick={handleDeleteComment}>
                                        <Icon height={18} isPrimary={false}>
                                            <MdClose/>
                                        </Icon>
                                    </Button>

                                    <Button isPrimary={false}
                                            visualType={ButtonVisualType.icon}
                                            onClick={() => setEditingActive(true)}>
                                        <Icon height={18} isPrimary={false}>
                                            <MdEdit/>
                                        </Icon>
                                    </Button>
                                </div>
                            }

                        </div>

                        <p>{comment.comment}</p>

                        <ShortDate date={comment.date}/>
                    </div>
                </div>
                : <CommentInput value={comment.comment} action={CommentAction.edit} 
                                submitCallBack={handleEditComment} 
                                cancelCallBack={() => setEditingActive(false)}/>
            }
        </>
    )
}