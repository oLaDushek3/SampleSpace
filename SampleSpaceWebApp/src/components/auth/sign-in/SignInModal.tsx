import React, {useRef, useState} from "react";
import Button from "../../button/Button.tsx";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import useAuth from "../../../hook/useAuth.ts";
import useClickOutside from "../../../hook/useClickOutside.ts";
import signInModalClasses from "./SignInModal.module.css"
import useUserApi from "../../../dal/api/user/useUserApi.ts";

interface SignInModalProps {
    onClose: () => void;
}

export default function SignInModal({onClose}: SignInModalProps) {
    const {signIn} = useUserApi();
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onClose);
    const [nickname, setNickname] = useState("")
    const [password, setPassword] = useState("")
    const [error, setError] = useState("")
    const {setUser} = useAuth();

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

                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button primary={true}
                        alone={true} 
                        onClick={handleSubmit}>
                    Войти
                </Button>

                <Button alone={true} onClick={onClose}>Назад</Button>
            </form>
        </div>
    )
}