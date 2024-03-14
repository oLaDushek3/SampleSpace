import {createPortal} from "react-dom";
import React, {useEffect, useRef} from "react";
import './Modal.module.css'

interface ModalProps extends React.ComponentProps<'dialog'>{
    open: boolean;
}

export default function Modal({open, ...props}: ModalProps) {
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
        <dialog ref={dialog}
                {...props}/>,
        portalDiv
    )
}