import Button from "../button/Button.tsx";
import {useState} from "react";
import Modal from "../modal/Modal.tsx";
import CreateSampleModal from "../sample/create-sample/CreateSampleModal.tsx";

export default function CreateSampleHeaderPanel() {
    const [createSampleIsOpen, setCreateSampleIsOpen] = useState(false)
    
    return (
        <div className={"horizontalPanel"}>
            <Button onClick={() => setCreateSampleIsOpen(true)}>
                Создать
            </Button>

            {createSampleIsOpen &&
                <Modal open={createSampleIsOpen}>
                    <CreateSampleModal onClose={() => setCreateSampleIsOpen(false)}/>
                </Modal>}
        </div>
    )
}