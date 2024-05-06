import userAvatarClasses from "./UserAvatar.module.css";
import { CiUser } from "react-icons/ci";
import {useEffect, useState} from "react";
import Icon from "../icon/Icon.tsx";

interface UserAvatarProps {
    src?: string;
    height: number;
}

export default function UserAvatar({src = "", height}: UserAvatarProps) {
    const [imageSource, setImageSource] = useState(src)
    const [correctSrc, setCorrectSrc] = useState(true)

    useEffect(() => {
        setImageSource(src);
        setCorrectSrc(true);
    }, [src]);
    
    return (
        <>
            {correctSrc ?
                <img style={{height: height}} className={userAvatarClasses.userAvatar} src={imageSource} alt={"avatar"} onError={() => setCorrectSrc(false)}/> :
                <Icon height={height}>
                    <CiUser/>
                </Icon>
            }
        </>
    )
}