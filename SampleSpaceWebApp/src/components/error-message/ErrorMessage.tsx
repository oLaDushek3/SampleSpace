import classes from "./ErrorMessage.module.css"
import {Dispatch, SetStateAction, useEffect} from "react";

interface ErrorMessageProps {
    error : string;
    setError: Dispatch<SetStateAction<string>>
}

export default function ErrorMessage({error, setError}: ErrorMessageProps) {
    
    useEffect(() => {
        setError(error);
        setTimeout(() => setError(""), 3500);
    }, [error]);
    
    return (
        <p className={classes.errorMessage}>{error}</p>
    )
}