import {useRef} from "react";
import useClickOutside from "../../../hook/useClickOutside.ts";
import Button from "../../button/Button.tsx";
import confirmModalClasses from "./ConfirmModal.module.css";

interface ConfirmModalProps {
    message: string;
    onConfirm: () => void;
    onCancel: () => void;
}

export default function ConfirmModal({message, onConfirm, onCancel}: ConfirmModalProps) {
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onCancel);

    return (
        <div ref={wrapperRef}
             className={confirmModalClasses.confirm + " verticalPanel"}>
            <h2>Вы уверены?</h2>
            <p className={confirmModalClasses.message}>{message}</p>

            <Button primary={true}
                    alone={true} 
                    onClick={onConfirm}>
                Да
            </Button>

            <Button alone={true}
                    onClick={onCancel}>
                Нет
            </Button>
        </div>
    )
}