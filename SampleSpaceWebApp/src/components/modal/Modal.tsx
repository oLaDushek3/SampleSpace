import {createPortal} from "react-dom";
import {ReactNode, useEffect, useRef} from "react";
import './Modal.module.css'

interface ModalProps{
    open: boolean;
    children: ReactNode
}

export default function Modal({open, children}: ModalProps) {
    const dialog = useRef<HTMLDialogElement>(null);
    const portalDiv = document.getElementById('modal')!;
    
    useEffect(() => {
        if(open){
            dialog.current?.showModal();
        } else {
            dialog.current?.close();
        }
    }, [open])
    
    return createPortal(
        <dialog ref={dialog} children={children}/>,
        portalDiv
    )
}