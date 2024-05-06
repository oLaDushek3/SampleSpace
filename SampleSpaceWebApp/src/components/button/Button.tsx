import buttonClasses from './Button.module.css'
import React from "react";

export enum ButtonVisualType {
    simple,
    withIcon,
    icon
}

const styles = {
    [ButtonVisualType.simple]: buttonClasses.button,
    [ButtonVisualType.withIcon]: buttonClasses.button + ` ${buttonClasses.withIcon}`,
    [ButtonVisualType.icon]: buttonClasses.button + ` ${buttonClasses.icon}`,
}

interface ButtonProps extends React.ComponentProps<'button'> {
    isActive?: boolean;
    visualType?: ButtonVisualType;
    isPrimary?: boolean;
    alone?: boolean;
}

export default function Button({isActive = true, isPrimary = false, alone = false, visualType = ButtonVisualType.simple, ...pops}: ButtonProps) {
    let classes: string = styles[visualType];
    
    if(!isActive) classes += ` ${buttonClasses.inactive}`
    
    if(isPrimary) classes += ` ${buttonClasses.primary}`;
    
    if(alone) classes += ` ${buttonClasses.alone}`;
    
    return (
        <button className={classes}
                {...pops} />
    )
}