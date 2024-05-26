import ProfileHeaderPanel from "./profile-header-panel/ProfileHeaderPanel.tsx";
import PlayerHeaderPanel from "./player-header-panel/PlayerHeaderPanel.tsx";
import useAuth from "../../hook/useAuth.ts";
import Search from "../search/Search.tsx";
import AuthHeaderPanel from "./AuthHeaderPanel.tsx";
import headerClasses from "./Header.module.css"
import {useNavigate, useSearchParams} from "react-router-dom";
import logo from "/logo.png"
import Button, {ButtonVisualType} from "../button/Button.tsx";
import {IoSettingsOutline} from "react-icons/io5";
import {useRef, useState} from "react";
import useClickOutside from "../../hook/useClickOutside.ts";
import CreateSampleHeaderPanel from "./CreateSampleHeaderPanel.tsx";

export default function Header() {
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search-query")
    const {loginUser} = useAuth()
    const [playerPanelIsActive, setPlayerPanelIsActive] = useState<boolean>(false)
    const playerPanelContainerRef = useRef(null);
    useClickOutside(playerPanelContainerRef, () => {setPlayerPanelIsActive(false)});

    const handleSearch = (searchQuery: string) => {
        navigate({pathname: "/search", search: `?search-query=${searchQuery}`});
    }

    return (
        <header className={headerClasses.headerPanel + " horizontalPanel"}>
            <img src={logo}
                 alt="Sample Space"
                 style={{height: "25px", cursor: "pointer"}}
                 onClick={() => navigate("/")}/>

            {loginUser && <CreateSampleHeaderPanel/>}

            <Search searchQuery={searchQuery ? searchQuery : ""} callBack={handleSearch}/>

            <div ref={playerPanelContainerRef} className={headerClasses.playerPanelContainer}>
                <Button visualType={ButtonVisualType.withIcon}
                        onClick={() => setPlayerPanelIsActive(prevState => !prevState)}>
                    <IoSettingsOutline/>
                </Button>
                <PlayerHeaderPanel isActive={playerPanelIsActive}/>
            </div>

            {loginUser ? <ProfileHeaderPanel user={loginUser}/> : <AuthHeaderPanel/>}
        </header>
    )
}