import classes from "./ErrorMessage.module.css"
import {Dispatch, SetStateAction, useEffect, useState} from "react";

interface ErrorMessageProps {
    error : string;
    setError: Dispatch<SetStateAction<string>>;
}

export default function ErrorMessage({error, setError}: ErrorMessageProps) {
    const [resetErrorTimeout, setResetErrorTimeout] = useState<ReturnType<typeof setTimeout> | null>(null)
    
    useEffect(() => {
        if(resetErrorTimeout)
            clearTimeout(resetErrorTimeout);
        
        setError(error);
        setResetErrorTimeout(setTimeout(() => setError(""), 3500));
    }, [error]);
    
    return (
        <p className={classes.errorMessage}>{error}</p>
    )
}