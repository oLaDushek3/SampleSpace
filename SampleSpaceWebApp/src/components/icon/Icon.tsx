import iconClasses from "./Icon.module.css"

export default function Icon({...props}) {
    return (
        <div className={iconClasses.icon} {...props}>
        </div>
    )
}