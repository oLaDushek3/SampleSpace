import IPlaylist from "../../../dal/entities/IPlaylist.ts";
import {useState} from "react";
import Button, {ButtonVisualType} from "../../button/Button.tsx";
import Icon from "../../icon/Icon.tsx";
import {MdAdd, MdEdit} from "react-icons/md";
import Modal from "../../modal/Modal.tsx";
import CreatePlaylistModal from "../create-playlist/CreatePlaylistModal.tsx";
import EditPlaylistModal from "../edit-playlist/EditPlaylistModal.tsx";

interface PlaylistToolsProfilePanelProps {
    selectedPlaylist: IPlaylist;
    onCreate: () => void;
    onEdit: () => void;
}

export default function PlaylistToolsProfilePanel({selectedPlaylist, onCreate, onEdit}: PlaylistToolsProfilePanelProps) {
    const [createPlaylistIsOpen, setCreatePlaylistIsOpen] = useState(false)
    const [editPlaylistIsOpen, setEditPlaylistIsOpen] = useState(false)
    
    return (
        <>
            <Button visualType={ButtonVisualType.withIcon}
                    onClick={() => setCreatePlaylistIsOpen(true)}>
                <Icon>
                    <MdAdd/>
                </Icon>
            </Button>

            {selectedPlaylist &&
                <Button visualType={ButtonVisualType.withIcon}
                        active={selectedPlaylist?.canBeModified}
                        onClick={() => setEditPlaylistIsOpen(true)}>
                    <Icon>
                        <MdEdit/>
                    </Icon>
                </Button>}

            <Modal open={createPlaylistIsOpen || editPlaylistIsOpen}>
                {createPlaylistIsOpen && <CreatePlaylistModal onClose={() => setCreatePlaylistIsOpen(false)}
                                                              onCreate={onCreate}/>}
                {editPlaylistIsOpen && <EditPlaylistModal playlist={selectedPlaylist!}
                                                          onClose={() => setEditPlaylistIsOpen(false)}
                                                          onEdit={onEdit}/>}
            </Modal>
        </>
    )
}