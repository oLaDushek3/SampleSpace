import buttonClasses from './Button.module.css'
import React from "react";

interface ButtonProps extends React.ComponentProps<'button'> {
    isPrimary?: boolean;
    alone?: boolean
}

export default function Button({isPrimary = false, alone = false, ...pops}: ButtonProps) {
    let classes: string = `${buttonClasses.button}`;
    
    if(isPrimary) classes += ` ${buttonClasses.primary}`;
    
    if(alone) classes += ` ${buttonClasses.alone}`;
    else classes += ` ${buttonClasses.notAlone}`;
    
    return (
        <button className={classes}
                {...pops} />
    )
}