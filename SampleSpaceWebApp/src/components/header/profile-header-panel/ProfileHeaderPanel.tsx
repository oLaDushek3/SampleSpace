import IUser from "../../../dal/entities/IUser.ts";
import {Link} from "react-router-dom";
import profileHeaderPanelClasses from "./ProfileHeaderPanel.module.css"
import UserAvatar from "../../user-avatar/UserAvatar.tsx";

interface ProfileHeaderPanelProps {
    user: IUser
}

export default function ProfileHeaderPanel({user}: ProfileHeaderPanelProps) {
    return (
        <div className={profileHeaderPanelClasses.avatar}>
            <Link to={`/${user.nickname}`}>
                <UserAvatar height={43} src={user.avatarPath}/>
            </Link>
        </div>
    )
}