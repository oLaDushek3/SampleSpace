import React, {RefObject, useRef, useState} from "react";
import useClickOutside from "../../../hook/useClickOutside.ts";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import Button from "../../button/Button.tsx";
import editPlaylistModalClasses from "./EditPlaylistModal.module.css";
import Modal from "../../modal/Modal.tsx";
import ConfirmModal from "../../dialog/confirm/ConfirmModal.tsx";
import IPlaylist from "../../../dal/entities/IPlaylist.ts";
import usePlaylistApi from "../../../dal/api/playlist/usePlaylistApi.ts";

interface EditPlaylistModalProps {
    playlist: IPlaylist;
    onClose: () => void;
    onEdit: () => void;
}

export default function EditPlaylistModal({playlist, onClose, onEdit}: EditPlaylistModalProps) {
    const wrapperRef = useRef(null);
        const [clickOutsideRef, setClickOutsideRef] = useState<RefObject<any> | null>(wrapperRef)
    const [confirmIsOpen, setConfirmIsOpen] = useState(false);
    useClickOutside(clickOutsideRef, onClose);
    
    const {deletePlaylist, editPlaylist} = usePlaylistApi();
    const [name, setName] = useState(playlist.name);
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (name.trim().length === 0) {
            setError("Не корректные данные");
            return
        }

        const response = await editPlaylist(playlist.playlistGuid, name);

        if (response) {
            onClose();
            onEdit();
        } else {
            setError("Ошибка сохранения");
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

        const response = await deletePlaylist(playlist.playlistGuid);
        
        if (response) {
            onClose();
            onEdit();
        } else {
            setError("Ошибка удаления");
            return
        }
    }

    const handleDeleteCancelled = async () => {
        setConfirmIsOpen(false)
        setClickOutsideRef(wrapperRef);
    }

    return (
        <div ref={wrapperRef}
             className={editPlaylistModalClasses.editPlaylist + " verticalPanel"}>
            <p style={{fontSize: "24px", fontWeight: "bold"}}>Редактирование плейлиста</p>

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
                    Сохранить
                </Button>

                <Button warning={true} 
                        alone={true}
                        onClick={handleDelete}>
                    Удалить плейлист
                </Button>

                <Button alone={true} onClick={onClose}>Назад</Button>
            </form>

            <Modal open={confirmIsOpen}>
                {confirmIsOpen && <ConfirmModal message={"Плейлист будет удален"} 
                                                onConfirm={handleDeleteConfirm}
                                                onCancel={handleDeleteCancelled}/>}
            </Modal>
        </div>
    )
}