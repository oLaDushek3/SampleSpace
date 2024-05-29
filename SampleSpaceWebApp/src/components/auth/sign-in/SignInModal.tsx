import React, {RefObject, useRef, useState} from "react";
import Button from "../../button/Button.tsx";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import useAuth from "../../../hook/useAuth.ts";
import useClickOutside from "../../../hook/useClickOutside.ts";
import signInModalClasses from "./SignInModal.module.css"
import useUserApi from "../../../dal/api/user/useUserApi.ts";
import Modal from "../../modal/Modal.tsx";
import ForgotPasswordModal from "../forgot-password/ForgotPasswordModal.tsx";

interface SignInModalProps {
    onClose: () => void;
}

export default function SignInModal({onClose}: SignInModalProps) {
    const wrapperRef = useRef(null);
    const [clickOutsideRef, setClickOutsideRef] = useState<RefObject<any> | null>(wrapperRef)
    const [informIsOpen, setInformIsOpen] = useState(false);
    useClickOutside(clickOutsideRef, onClose);

    const {signIn} = useUserApi();
    const {setUser} = useAuth();

    const [nickname, setNickname] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (nickname.trim().length === 0 || password.trim().length === 0) {
            setError("Не все поля заполнены");
            return
        }

        const response = await signIn(nickname, password);

        if (response) {
            onClose();
            setUser(response);
        } else {
            setError("Ошибка авторизации");
            return
        }
    }

    const handleForgotPassword = () => {
        setClickOutsideRef(null);
        setInformIsOpen(true);
    }

    const handleForgotPasswordOnClose = () => {
        setClickOutsideRef(wrapperRef);
        setInformIsOpen(false);
    }
    
    return (
        <div ref={wrapperRef}
             className={signInModalClasses.signIn + " verticalPanel"}>
            <p style={{fontSize: "24px", fontWeight: "bold"}}>Авторизация</p>

            <form className={"verticalPanel"}
                  onSubmit={handleSubmit}>

                <label htmlFor="nickname">Имя пользователя</label>
                <input id="nickname"
                       className="text-input"
                       placeholder="Введите имя пользователя"
                       type="text"
                       maxLength={75}
                       value={nickname}
                       onChange={e => setNickname(e.target.value)}/>

                <label htmlFor="password">Пароль</label>
                <input id="password"
                       className="text-input"
                       placeholder="Введите пароль"
                       type="password"
                       value={password}
                       onChange={e => setPassword(e.target.value)}/>

                <p className={signInModalClasses.forgotPassword}
                    onClick={handleForgotPassword}>Забыли пароль?</p>


                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button primary={true}
                        alone={true}
                        onClick={handleSubmit}>
                    Войти
                </Button>

                <Button alone={true} onClick={onClose}>Назад</Button>
            </form>

            <Modal open={informIsOpen}>
                <ForgotPasswordModal onClose={handleForgotPasswordOnClose}/>
            </Modal>
        </div>
    )
}