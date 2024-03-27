import headerClasses from "./Header.module.css"
import {useNavigate, useSearchParams} from "react-router-dom";
import logo from "/logo.png"
import Search from "../search/Search.tsx";
import AuthHeaderPanel from "./AuthHeaderPanel.tsx";
import useAuth from "../../hook/useAuth.ts";
import ProfileHeaderPanel from "./ProfileHeaderPanel.tsx";

export default function Header() {
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search_query")
    const {user} = useAuth()
    
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

            {user ? <ProfileHeaderPanel user={user}/> : <AuthHeaderPanel/>}
        </header>
    )
}