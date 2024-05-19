import React, {useRef, useState} from "react";
import Button from "../../button/Button.tsx";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import UserApi from "../../../dal/api/user/UserApi.ts";
import useClickOutside from "../../../hook/useClickOutside.ts";
import signUpModalClasses from "./SignUpModal.module.css"

interface SignUpModalProps {
    onClose: () => void;
}

export default function SignUpModal({onClose}: SignUpModalProps) {
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onClose);
    const [nickname, setNickname] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [error, setError] = useState("")
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (nickname.trim().length === 0 || email.trim().length === 0 || password.trim().length === 0) {
            setError("Не все поля заполнены");
            return
        }
        
        const response = await UserApi.signUp(nickname, email, password);

        if (response) {
            onClose();
        } else {
            setError("Ошибка регистрации");
            return
        }
    }

    return (
        <div ref={wrapperRef} 
             className={signUpModalClasses.signUp + " verticalPanel"} >
            <p style={{fontSize: "24px", fontWeight: "bold"}}>Регистрация</p>

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

                <label htmlFor="email">Почта</label>
                <input id="email"
                       className="text-input"
                       placeholder="Введит електронную почту"
                       type="email"
                       value={email}
                       onChange={e => setEmail(e.target.value)}/>

                <label htmlFor="password">Пароль</label>
                <input id="password"
                       className="text-input"
                       placeholder="Введите пароль"
                       type="password"
                       value={password}
                       onChange={e => setPassword(e.target.value)}/>

                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button primary={true}
                        alone={true}
                        onClick={handleSubmit}>
                    Продолжить
                </Button>

                <Button alone={true} onClick={onClose}>Назад</Button>
            </form>
        </div>
    )
}