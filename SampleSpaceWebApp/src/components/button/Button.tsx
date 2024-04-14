import buttonClasses from './Button.module.css'
import React from "react";

export enum ButtonVisualType {
    simple,
    icon,
}

const styles = {
    [ButtonVisualType.simple]: buttonClasses.button,
    [ButtonVisualType.icon]: buttonClasses.button + ` ${buttonClasses.icon}`,
}

interface ButtonProps extends React.ComponentProps<'button'> {
    visualType?: ButtonVisualType;
    isPrimary?: boolean;
    alone?: boolean;
}

export default function Button({isPrimary = false, alone = false, visualType = ButtonVisualType.simple, ...pops}: ButtonProps) {
    
    let classes: string = styles[visualType];
    
    if(isPrimary) classes += ` ${buttonClasses.primary}`;
    
    if(alone) classes += ` ${buttonClasses.alone}`;
    
    return (
        <button className={classes}
                {...pops} />
    )
}