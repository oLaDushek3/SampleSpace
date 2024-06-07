import resetPasswordPageClasses from "./ResetPasswordPage.module.css"
import ErrorMessage from "../../components/error-message/ErrorMessage.tsx";
import Button from "../../components/button/Button.tsx";
import Modal from "../../components/modal/Modal.tsx";
import InformModal from "../../components/dialog/inform/InformModal.tsx";
import React, {useState} from "react";
import useUserApi from "../../dal/api/user/useUserApi.ts";
import {useNavigate, useSearchParams} from "react-router-dom";
import usePasswordValidation from "../../hook/usePasswordValidation.ts";

export default function ResetPasswordPage() {
    const [informIsOpen, setInformIsOpen] = useState(false);
    const [informMessage, setInformMessage] = useState("")

    const navigate = useNavigate()
    const {resetPassword} = useUserApi();
    
    const [searchParams] = useSearchParams()
    const token = searchParams.get("token")
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const {validation} = usePasswordValidation();
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (password.trim().length === 0) {
            setError("Не все поля заполнены");
            return;
        }

        if(password !== confirmPassword){
            setError("Пароли не совпадают");
            return;
        }

        let validationError: string = validation(password);
        
        if(validationError.trim.length !== 0){
            setError(validationError);
            return;
        }
        
        const response = await resetPassword(token, password);

        console.log(response);
        if(response === 403){
            setInformMessage("Ссылка для смены пароля не действителен");
            setInformIsOpen(true);
            return;
        }            
        
        setInformMessage("Пароль успешно изменен");
        setInformIsOpen(true);
    }

    const handleInformOnClose = () => {
        setInformIsOpen(false);
        navigate("/");
    }

    return (
        <div className={"centered"}>
            <div className={resetPasswordPageClasses.resetPasswordPage + " verticalPanel"}>
                
                <p style={{fontSize: "24px", fontWeight: "bold"}}>Смена пароля</p>
                
                <form className={"verticalPanel"}
                      onSubmit={handleSubmit}>

                    <label htmlFor="password">Новый пароль</label>
                    <input id="password"
                           className="text-input"
                           placeholder="Введите новый пароль"
                           type="password"
                           value={password}
                           onChange={e => setPassword(e.target.value)}/>

                    <label htmlFor="confirmPassword">Подтверждение пароля</label>
                    <input id="confirmPassword"
                           className="text-input"
                           placeholder="Введите подтверждение пароля"
                           type="password"
                           value={confirmPassword}
                           onChange={e => setConfirmPassword(e.target.value)}/>

                    {error && <ErrorMessage error={error} setError={setError}/>}

                    <Button primary={true}
                            alone={true}
                            onClick={handleSubmit}>
                        Сохранить
                    </Button>
                </form>

                <Modal open={informIsOpen}>
                    <InformModal message={informMessage}
                                 onClose={handleInformOnClose}/>
                </Modal>
            </div>
        </div>
    )
}