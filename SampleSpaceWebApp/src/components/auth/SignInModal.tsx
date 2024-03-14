import React, {useEffect, useState} from "react";
import Button from "../button/Button.tsx";
import ErrorMessage from "../error-message/ErrorMessage.tsx";
import UserApi from "../../dal/api/user/UserApi.ts";

interface SignInModalProps {
    onClose: Function
}

export default function SignInModal({onClose}: SignInModalProps) {
    const [nickname, setNickname] = useState("")
    const [password, setPassword] = useState("")
    const [error, setError] = useState("")
    

    useEffect(() =>{
        setNickname("");
        setPassword("");
        setError("");
    }, [onClose])

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (nickname.trim().length === 0 || password.trim().length === 0) {
            setError("Не корректные данные");
            return
        }

        const response = await UserApi.signIn(nickname, password);

        if (response) {
            onClose();
        } else {
            setError("Ошибка авторизации");
            return
        }
    }

    return (
        <div onClick={e => (e.currentTarget === e.target) && onClose()}>
            <h2>Авторизация</h2>

            <form onSubmit={handleSubmit}>
                <label htmlFor="nickname">Имя пользователя</label>
                <input id="nickname"
                       className="control"
                       placeholder="Введите имя пользователя"
                       type="text"
                       maxLength={75}
                       value={nickname}
                       onChange={e => setNickname(e.target.value)}/>

                <label htmlFor="password">Пароль</label>
                <input id="password"
                       className="control"
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