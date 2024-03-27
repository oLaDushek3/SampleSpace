import IUser from "../../dal/models/IUser.ts";
import {Link} from "react-router-dom";
import profileHeaderPanelClasses from "./ProfileHeaderPanel.module.css"

interface ProfileHeaderPanel {
    user: IUser
}

export default function ProfileHeaderPanel({user}: ProfileHeaderPanel) {
    return (
        <Link to={`/${user.nickname}`} className={profileHeaderPanelClasses.link}>
            <img className={profileHeaderPanelClasses.avatar} src={user.avatarPath} alt={"avatar"}/>
        </Link>
    )
}