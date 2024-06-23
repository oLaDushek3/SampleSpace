import userAvatarClasses from "./UserAvatar.module.css";
import {CiUser} from "react-icons/ci";
import {useEffect, useState} from "react";
import { IconContext } from "react-icons";

interface UserAvatarProps {
    src: string;
    height: number;
}

export default function UserAvatar({src, height}: UserAvatarProps) {
    const [imageSource, setImageSource] = useState(src);
    const [correctSrc, setCorrectSrc] = useState(false);
    const [classes, setClasses] = useState(userAvatarClasses.userAvatar);
    
    useEffect(() => {
        setImageSource(src += '?' + new Date().getTime());
        if(src !== null)
            setCorrectSrc(true);
        else
            setCorrectSrc(false);
    }, [src]);

    useEffect(() => {
        if(!correctSrc)
            setClasses(`${userAvatarClasses.userAvatar} ${userAvatarClasses.invalidSrc}`)
        else
            setClasses(`${userAvatarClasses.userAvatar}`)
    }, [correctSrc]);
    
    return (
        <div style={{height: height ? height : "100%"}} 
             className={classes}>
            {correctSrc ?
                <img src={imageSource} alt={"avatar"}
                     onError={() => setCorrectSrc(false)}/> :
                <IconContext.Provider value={{size: "100%" }}>
                    <CiUser/>
                </IconContext.Provider>
            }
        </div>
    )
}