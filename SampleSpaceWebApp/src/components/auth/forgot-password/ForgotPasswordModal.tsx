import React, {RefObject, useRef, useState} from "react";
import useClickOutside from "../../../hook/useClickOutside.ts";
import editPlaylistModalClasses from "../../playlist/edit-playlist/EditPlaylistModal.module.css";
import Modal from "../../modal/Modal.tsx";
import InformModal from "../../dialog/inform/InformModal.tsx";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import Button from "../../button/Button.tsx";
import useUserApi from "../../../dal/api/user/useUserApi.ts";

interface ForgotPasswordModalProps {
    onClose: () => void;
}

export default function ForgotPasswordModal({onClose}: ForgotPasswordModalProps) {
    const wrapperRef = useRef(null);
    const [clickOutsideRef, setClickOutsideRef] = useState<RefObject<any> | null>(wrapperRef)
    const [informIsOpen, setInformIsOpen] = useState(false);
    useClickOutside(clickOutsideRef, onClose);

    const {forgotPassword} = useUserApi();
    
    const [email, setEmail] = useState("");
    const [error, setError] = useState("");
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (email.trim().length === 0) {
            setError("Не все поля заполнены");
            return;
        }
        
        const response = await forgotPassword(window.location.origin + "/reset-password", email);

        if(response === 404){
            setError("Пользователь с такой вочтой не найден");
            return;
        }
        
        setClickOutsideRef(null);
        setInformIsOpen(true);
    }

    const handleInformOnClose = () => {
        setClickOutsideRef(wrapperRef);
        setInformIsOpen(false);
        onClose();
    }
    
    return (
        <div ref={wrapperRef}
             className={editPlaylistModalClasses.editPlaylist + " verticalPanel"}>

            <form className={"verticalPanel"}
                  onSubmit={handleSubmit}>

                <label htmlFor="email">Почта</label>
                <input id="email"
                       className="text-input"
                       placeholder="Введите почту аккаунта"
                       type="email"
                       value={email}
                       onChange={e => setEmail(e.target.value)}/>
                
                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button primary={true}
                        alone={true}
                        onClick={handleSubmit}>
                    Продолжить
                </Button>

                <Button alone={true} onClick={onClose}>Отмена</Button>
            </form>
            
            <Modal open={informIsOpen}>
                <InformModal message={"На вашу почту отправленно письмо с сылкой на востановление пароля"}
                             onClose={handleInformOnClose}/>
            </Modal>
        </div>
    )
}