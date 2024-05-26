import React, {useRef, useState} from "react";
import useClickOutside from "../../../hook/useClickOutside.ts";
import useAuth from "../../../hook/useAuth.ts";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import Button from "../../button/Button.tsx";
import createPlaylistModalClasses from "./CreatePlaylistModal.module.css";
import usePlaylistApi from "../../../dal/api/playlist/usePlaylistApi.ts";

interface CreatePlaylistModalProps {
    onClose: () => void;
    onCreate: () => void;
}

export default function CreatePlaylistModal({onClose, onCreate}: CreatePlaylistModalProps) {
    const {createPlaylist} = usePlaylistApi();
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onClose);
    const [name, setName] = useState("");
    const [error, setError] = useState("");
    const {loginUser} = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (name.trim().length === 0) {
            setError("Не корректные данные");
            return
        }

        const response = await createPlaylist(loginUser!.userGuid, name);

        if (response) {
            onClose();
            onCreate();
        } else {
            setError("Ошибка создания");
            return
        }
    }
    
    return (
        <div ref={wrapperRef}
             className={createPlaylistModalClasses.createPlaylist + " verticalPanel"}>
            <p style={{fontSize: "24px", fontWeight: "bold"}}>Создание плейлиста</p>

            <form className={"verticalPanel"}
                  onSubmit={handleSubmit}>
                <label htmlFor="name">Название</label>
                <input id="name"
                       className="text-input"
                       placeholder="Введите название плейлиста"
                       type="text"
                       maxLength={75}
                       value={name}
                       onChange={e => setName(e.target.value)}/>

                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button primary={true}
                        alone={true} 
                        onClick={handleSubmit}>
                    Создать
                </Button>

                <Button alone={true} onClick={onClose}>Назад</Button>
            </form>
        </div>
    )
}