interface usePasswordValidationType{
    validation: Function,
}

export default function usePasswordValidation(): usePasswordValidationType{
    
    const numberRegex = /[0-9]+/g;
    const upperCharRegex = /[A-ZА-Я]+/g;
    const minimum5CharsRegex = /.{5,}/g;
    
    const validation = (password: string): string => {
        if(!password.match(numberRegex)){
            return "Пароль должен содержать цифры";
        }

        if(!password.match(upperCharRegex)){
            return "Пароль должен содрежать заглавные буквы";
        }

        if(!password.match(minimum5CharsRegex)){
            return "Пароль должен содрежать не менее 5 символов";
        }
        
        return "";
    }
    
    return {validation};
}