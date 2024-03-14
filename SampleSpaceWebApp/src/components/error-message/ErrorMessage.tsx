import classes from "./ErrorMessage.module.css"

interface ErrorMessageProps {
    error: string;
}

export default function ErrorMessage({error}: ErrorMessageProps) {
    return (
        <p className={classes.errorMessage}>{error}</p>
    )
}