import informModalClasses from "./InformModal.module.css";
import Button from "../../button/Button.tsx";
import {useRef} from "react";
import useClickOutside from "../../../hook/useClickOutside.ts";

interface InformModalProps {
    message: string;
    onClose: () => void;
}

export default function InformModal({message, onClose}: InformModalProps) {
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onClose);
    
    return (
        <div ref={wrapperRef}
             className={informModalClasses.inform + " verticalPanel"}>
            <h2>Сообщение</h2>
            <p className={informModalClasses.message}>{message}</p>

            <Button primary={true}
                    alone={true}
                    onClick={onClose}>
                Ок
            </Button>
        </div>
    )
}