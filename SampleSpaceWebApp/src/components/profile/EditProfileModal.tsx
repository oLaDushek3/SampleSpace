import editProfileClasses from "./EditProfileModal.module.css"
import ErrorMessage from "../error-message/ErrorMessage.tsx";
import Button from "../button/Button.tsx";
import React, {RefObject, useEffect, useRef, useState} from "react";
import useUserApi from "../../dal/api/user/useUserApi.ts";
import FileInput, {FileInputAccept} from "../file-input/FileInput.tsx";
import UserAvatar from "../user-avatar/UserAvatar.tsx";
import useAuth from "../../hook/useAuth.ts";
import IUser from "../../dal/entities/IUser.ts";
import useClickOutside from "../../hook/useClickOutside.ts";
import Modal from "../modal/Modal.tsx";
import ConfirmModal from "../dialog/confirm/ConfirmModal.tsx";
import InformModal from "../dialog/inform/InformModal.tsx";

interface EditProfileModal {
    onCancel: () => void;
    onSuccess: (modifiedUser: IUser) => void;
    onDelete: () => void;
}

export default function EditProfileModal({onCancel, onSuccess, onDelete}: EditProfileModal) {
    const wrapperRef = useRef(null);
    const [clickOutsideRef, setClickOutsideRef] = useState<RefObject<any> | null>(wrapperRef)
    const [confirmIsOpen, setConfirmIsOpen] = useState(false);
    const [informIsOpen, setInformIsOpen] = useState(false);
    useClickOutside(clickOutsideRef, onCancel);

    const {editUser, deleteUser, forgotPassword} = useUserApi();
    const {loginUser, setUser} = useAuth();

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

    const handleDelete = async (e: React.FormEvent) => {
        e.preventDefault();

        setClickOutsideRef(null);
        setConfirmIsOpen(true);
    }

    const handleDeleteConfirm = async () => {
        setConfirmIsOpen(false);

        const response = await deleteUser(loginUser!.userGuid);

        if (response) {
            onDelete();
        } else {
            setError("Ошибка удаления");
            return
        }
    }

    const handleDeleteCancelled = async () => {
        setConfirmIsOpen(false)
        setClickOutsideRef(wrapperRef);
    }

    const handleResetPassword = async (e: React.FormEvent) => {
        e.preventDefault();

        void await forgotPassword(window.location.origin + "/reset-password", loginUser!.email);
        
        setClickOutsideRef(null);
        setInformIsOpen(true);
    }
    
    const handleInformOnClose = () => {
        setClickOutsideRef(wrapperRef);
        setInformIsOpen(false);
    }

    useEffect(() => {
        if (avatarBlob)
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

                <div className={"horizontalPanel"}>
                    <Button alone={true}
                            onClick={handleResetPassword}>
                        Сбросить пароль
                    </Button>

                    <Button warning={true}
                            alone={true}
                            onClick={handleDelete}>
                        Удалить аккаунт
                    </Button>
                </div>

                <Button alone={true} onClick={onCancel}>Отмена</Button>
            </form>

            <Modal open={confirmIsOpen || informIsOpen}>
                {confirmIsOpen && <ConfirmModal message={"Аккаунт будет удален"}
                                                onConfirm={handleDeleteConfirm}
                                                onCancel={handleDeleteCancelled}/>}
                {informIsOpen && <InformModal message={"На вашу почту отправленно письмо с сылкой на востановление пароля"}
                                 onClose={handleInformOnClose}/>}
            </Modal>

        </div>
    )
}