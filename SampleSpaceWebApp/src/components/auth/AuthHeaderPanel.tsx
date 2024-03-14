import {useState} from "react";
import Button from "../button/Button.tsx";
import Modal from "../modal/Modal.tsx";
import SignUpModal from "./SignUpModal.tsx";
import SignInModal from "./SignInModal.tsx";

export default function AuthHeaderPanel() {

    const [sigUpIsOpen, setSigUpIsOpen] = useState(false)
    const [sigInIsOpen, setSigInIsOpen] = useState(false)

    return (
        <>
            <div>
                <Button isPrimary={true}
                        onClick={() => setSigInIsOpen(true)}>
                    Войти
                </Button>

                <Button onClick={() => setSigUpIsOpen(true)}>
                    Регистрация
                </Button>
            </div>

            <Modal open={sigUpIsOpen || sigInIsOpen}>
                {sigUpIsOpen && <SignUpModal onClose={() => setSigUpIsOpen(false)}/>}
                {sigInIsOpen && <SignInModal onClose={() => setSigInIsOpen(false)}/>}
            </Modal>
        </>
    )
}