import React, {useEffect, useState} from "react";
import Button from "../button/Button.tsx";
import ErrorMessage from "../error-message/ErrorMessage.tsx";
import UserApi from "../../dal/api/user/UserApi.ts";

interface SignUpModalProps {
    onClose: Function
}

export default function SignUpModal({onClose}: SignUpModalProps) {
    const [nickname, setNickname] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [error, setError] = useState("")

    useEffect(() =>{
        setNickname("");
        setEmail("");
        setPassword("");
        setError("");
    }, [onClose])
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (nickname.trim().length === 0 || email.trim().length === 0 || password.trim().length === 0) {
            setError("Не корректные данные");
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
        <div className={"verticalPanel"} 
             onClick={e => (e.currentTarget === e.target) && onClose()}>
            <h2>Регистрация</h2>

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

                {error && <ErrorMessage error={error}/>}

                <Button isPrimary={true}
                        alone={true}>
                    Продолжить
                </Button>
                
            </form>

            <p style={{textAlign: "center", margin: "0.5rem", cursor: "pointer"}} 
               onClick={() => onClose()}>Назад</p>
        </div>
    )
}