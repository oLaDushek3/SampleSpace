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

export default function Header() {
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search_query")
    const {user} = useAuth()
    const [playerPanelIsActive, setPlayerPanelIsActive] = useState<boolean>(false)
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, () => {setPlayerPanelIsActive(false)});

    const handleSearch = (searchQuery: string) => {
        navigate({pathname: "/search", search: `?search_query=${searchQuery}`});
    }

    return (
        <header className={"panel " + headerClasses.headerPanel}>
            <img src={logo}
                 alt="Sample Space"
                 style={{height: "25px", cursor: "pointer"}}
                 onClick={() => navigate("/")}/>

            <Search searchQuery={searchQuery ? searchQuery : ""} callBack={handleSearch}/>

            <div ref={wrapperRef} className={headerClasses.playerPanelContainer}>
                <Button visualType={ButtonVisualType.icon}
                        onClick={() => {
                            setPlayerPanelIsActive(prevState => !prevState)
                        }}>
                    <IoSettingsOutline/>
                </Button>
                <PlayerHeaderPanel isActive={playerPanelIsActive}/>
            </div>

            {user ? <ProfileHeaderPanel user={user}/> : <AuthHeaderPanel/>}
        </header>
    )
}