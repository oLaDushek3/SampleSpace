import React, {useEffect, useRef, useState} from "react";
import Button from "../../button/Button.tsx";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import useClickOutside from "../../../hook/useClickOutside.ts";
import signUpModalClasses from "./SignUpModal.module.css"
import useUserApi from "../../../dal/api/user/useUserApi.ts";
import FileInput, {FileInputAccept} from "../../file-input/FileInput.tsx";
import UserAvatar from "../../user-avatar/UserAvatar.tsx";

interface SignUpModalProps {
    onClose: () => void;
}

export default function SignUpModal({onClose}: SignUpModalProps) {
    const {signUp} = useUserApi();
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onClose);
    const [avatarBlob, setAvatarBlob] = useState<Blob>();
    const [avatarSrc, setAvatarSrc] = useState("");
    const [nickname, setNickname] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (nickname.trim().length === 0 || email.trim().length === 0 || password.trim().length === 0) {
            setError("Не все поля заполнены");
            return;
        }
        
        const response = await signUp(avatarBlob, nickname, email, password);
        
        if(response === 409){
            setError("Имя пользователя занято");
            return;
        }
        
        if (response === 200) {
            onClose();
        } else {
            setError("Ошибка регистрации");
            return;
        }
    }

    useEffect(() => {
        if(avatarBlob)
            setAvatarSrc(URL.createObjectURL(avatarBlob));
    }, [avatarBlob]);
    
    const avatarPreview = (
        <UserAvatar src={avatarSrc} height={125}/>)

    return (
        <div ref={wrapperRef} 
             className={signUpModalClasses.signUp + " verticalPanel"} >
            <p style={{fontSize: "24px", fontWeight: "bold"}}>Регистрация</p>

            <form className={"verticalPanel"}
                  onSubmit={handleSubmit}>

                <div className={signUpModalClasses.avatarPanel}>
                    <FileInput filePreview={avatarPreview}
                               accept={FileInputAccept.image}
                               onUpload={(file) => setAvatarBlob(new Blob([file]))}
                               setError={setError}/>
                </div>

                
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