import {useState} from "react";
import Button from "../button/Button.tsx";
import Modal from "../modal/Modal.tsx";
import SignInModal from "../auth/sign-in/SignInModal.tsx";
import SignUpModal from "../auth/sign-up/SignUpModal.tsx";

export default function AuthHeaderPanel() {

    const [sigUpIsOpen, setSigUpIsOpen] = useState(false)
    const [sigInIsOpen, setSigInIsOpen] = useState(false)

    return (
        <>
            <div className={"horizontalPanel"}>
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