import commentInputClasses from "./CommentInput.module.css";
import {FormEvent, useRef, useState} from "react";
import Button, {ButtonVisualType} from "../../../button/Button.tsx";
import { IoSend, IoCloseOutline } from "react-icons/io5";
import ErrorMessage from "../../../error-message/ErrorMessage.tsx";

export enum CommentAction {
    create,
    edit
}

interface CommentInputProps {
    value?: string; 
    action?: CommentAction;
    submitCallBack: (comment: string) => void;
    cancelCallBack?: () => void;
}

export default function CommentInput({value = "", action = CommentAction.create, submitCallBack, cancelCallBack = () => {}}: CommentInputProps) {
    const commentInput = useRef<HTMLInputElement>(null)
    const [commentValue, setCommentValue] = useState(value)
    const [error, setError] = useState("")

    const handleSubmit = (event: FormEvent) => {
        event.preventDefault();

        if(commentValue?.length > 200){
            setError("Максимум 200 символов");
            return;
        }
        
        if (commentValue !== ""){
            submitCallBack(commentValue);
            setCommentValue("");
        }
    }
    
    const handleCancel = () => {
        setCommentValue("");
        cancelCallBack();
    }
    
    return (
        <div className={"verticalPanel"}>
            <form className={commentInputClasses.commentInputForm + " horizontalPanel"}
                  onClick={() => commentInput.current!.focus()}
                  onSubmit={handleSubmit}
                  autoComplete="false"
                  spellCheck="false">

                <input ref={commentInput}
                       className={commentInputClasses.commentInput}
                       type={"text"}
                       placeholder={"Добавить комментарий"}
                       value={commentValue}
                       onChange={(event) => setCommentValue(event.target!.value)}/>

                {commentValue?.trim() &&
                    <Button type="button"
                            visualType={ButtonVisualType.withIcon}
                            isPrimary={true}
                            onClick={handleSubmit}>
                        <IoSend/>
                    </Button>
                }

                {action === CommentAction.edit &&
                    <Button type="button"
                            visualType={ButtonVisualType.withIcon}
                            isPrimary={false}
                            onClick={handleCancel}>
                        <IoCloseOutline/>
                    </Button>}
            </form>
            
            {error && <ErrorMessage error={error} setError={setError}/>}
        </div>
    )
}