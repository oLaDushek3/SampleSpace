import IUser from "../../../dal/entities/IUser.ts";
import {Link} from "react-router-dom";
import profileHeaderPanelClasses from "./ProfileHeaderPanel.module.css"

interface ProfileHeaderPanelProps {
    user: IUser
}

export default function ProfileHeaderPanel({user}: ProfileHeaderPanelProps) {
    return (
        <Link to={`/${user.nickname}`}>
            <img className={profileHeaderPanelClasses.avatar} src={user.avatarPath} alt={"avatar"}/>
        </Link>
    )
}