import {useState} from "react";

interface usePasswordValidationType{
    validation: Function,
    validationError: string
}

export default function usePasswordValidation(): usePasswordValidationType{
    const [validationError, setValidationError] = useState("");
    
    const numberRegex = /[0-9]+/g;
    const upperCharRegex = /[A-ZА-Я]+/g;
    const minimum5CharsRegex = /.{5,}/g;
    
    const validation = (password: string): boolean => {
        if(!password.match(numberRegex)){
            setValidationError("Пароль должен содержать цифры")
            return false;
        }

        if(!password.match(upperCharRegex)){
            setValidationError("Пароль должен содрежать заглавные буквы");
            return false;
        }

        if(!password.match(minimum5CharsRegex)){
            setValidationError("Пароль должен содрежать не менее 5 символов");
            return false;
        }
        
        return true;
    }
    
    return {validation, validationError};
}