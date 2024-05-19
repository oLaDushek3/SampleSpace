import buttonClasses from './Button.module.css'
import React, {ReactNode} from "react";

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
    active?: boolean;
    visualType?: ButtonVisualType;
    primary?: boolean;
    warning?: boolean;
    alone?: boolean;
}

interface RadioButtonProps {
    active?: boolean;
    visualType?: ButtonVisualType;
    selected?: boolean;
    onSelected: () => void;
    children: ReactNode;
}

export default function Button({
                                   active = true,
                                   primary = false,
                                   warning = false,
                                   alone = false,
                                   visualType = ButtonVisualType.simple,
                                   ...pops
                               }: ButtonProps) {
    let classes: string = styles[visualType];

    if (!active) classes += ` ${buttonClasses.inactive}`

    if (primary) classes += ` ${buttonClasses.primary}`
    
    if (warning) classes += ` ${buttonClasses.warning}`;

    if (alone) classes += ` ${buttonClasses.alone}`;

    return (
        <button className={classes} type={"button"}
                {...pops} />
    )
}

export function RadioButton({
                                active = true,
                                visualType = ButtonVisualType.simple,
                                selected = false,
                                onSelected,
                                children
                            }: RadioButtonProps) {
    let classes: string = styles[visualType];

    if (!active) classes += ` ${buttonClasses.inactive}`

    if (selected) classes += ` ${buttonClasses.primary}`;

    return (
        <button className={classes} onClick={onSelected}>
            {children}
        </button>
    )
}