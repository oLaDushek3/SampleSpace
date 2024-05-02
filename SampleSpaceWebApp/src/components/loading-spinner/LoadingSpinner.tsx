import loadingSpinnerClasses from "./LoadingSpinner.module.css"

export default function LoadingSpinner() {
    return (
        <div className={loadingSpinnerClasses.spinner}>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
            <div className={loadingSpinnerClasses.wave}></div>
        </div>
    )
}