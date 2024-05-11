import iconClasses from "./Icon.module.css"
import {ReactNode} from "react";

interface IconProps {
    height?: number;
    isPrimary?: boolean;
    children: ReactNode;
}

export default function Icon({height = 24, isPrimary = true, children}: IconProps) {
    let classes: string = iconClasses.icon;
    
    if(isPrimary){
        classes += ` ${iconClasses.primary}`
    }
    
    return (
        <div style={{fontSize: height}} className={classes}>
            {children}
        </div>
    )
}