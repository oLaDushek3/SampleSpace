import logo from "/logo.png"
import AuthHeaderPanel from "./AuthHeaderPanel.tsx";
import Search from "../search/Search.tsx";
import headerClasses from "./header.module.css"
import {Link, useNavigate, useSearchParams} from "react-router-dom";
import {useEffect} from "react";
export default function Header() {
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search_query")

    useEffect(() => {
        console.log(searchQuery ? searchQuery : "Бу");
    }, [searchQuery]);
    
    const handleSearch = (searchQuery: string) => {
        navigate({pathname: "/search", search: `?search_query=${searchQuery}`});
    }
    
    return (
        <header className={"panel " + headerClasses.headerPanel}>
            <img src={logo} 
                 alt="Sample Space" 
                 height="25" 
                 onClick={() => navigate("/")}/>
            
            <Search searchQuery={searchQuery ? searchQuery : ""} callBack={handleSearch}/>

            <Link to="/gg">ГГ</Link>
            
            <AuthHeaderPanel/>
        </header>
    )
}