import editProfileClasses from "./EditProfileModal.module.css"
import ErrorMessage from "../error-message/ErrorMessage.tsx";
import Button from "../button/Button.tsx";
import React, {useEffect, useRef, useState} from "react";
import useUserApi from "../../dal/api/user/useUserApi.ts";
import FileInput, {FileInputAccept} from "../file-input/FileInput.tsx";
import UserAvatar from "../user-avatar/UserAvatar.tsx";
import useAuth from "../../hook/useAuth.ts";
import IUser from "../../dal/entities/IUser.ts";

interface EditProfileModal {
    onCancel: () => void;
    onSuccess: (modifiedUser: IUser) => void;
}

export default function EditProfileModal({onCancel, onSuccess}: EditProfileModal) {
    const {editUser} = useUserApi();
    const {loginUser, setUser} = useAuth();
    const wrapperRef = useRef(null);
    const [avatarBlob, setAvatarBlob] = useState<Blob>();
    const [avatarSrc, setAvatarSrc] = useState(loginUser!.avatarPath ? loginUser!.avatarPath : "");
    const [nickname, setNickname] = useState(loginUser!.nickname);
    const [email, setEmail] = useState(loginUser!.email);
    const [error, setError] = useState("");
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (nickname.trim().length === 0 || email.trim().length === 0) {
            setError("Не все поля заполнены");
            return
        }

        const response = await editUser(loginUser?.userGuid, avatarBlob, nickname, email);

        if (response) {
            setUser(response);
            onSuccess(response);
        } else {
            setError("Ошибка сохранения изменений");
            return
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
             className={editProfileClasses.editProfile + " verticalPanel"}>
            <p style={{fontSize: "24px", fontWeight: "bold"}}>Редактирование профиля</p>

            <form className={"verticalPanel"}
                  onSubmit={handleSubmit}>

                <div className={editProfileClasses.avatarPanel}>
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
                       placeholder="Введите почту"
                       type="email"
                       value={email}
                       onChange={e => setEmail(e.target.value)}/>

                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button primary={true}
                        alone={true}
                        onClick={handleSubmit}>
                    Сохранить
                </Button>

                <Button alone={true} onClick={onCancel}>Отмена</Button>
            </form>
        </div>
    )
}