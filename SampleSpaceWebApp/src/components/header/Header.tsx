import logo from '/logo.png'
import './Header.module.css'
import AuthHeaderPanel from "../auth/AuthHeaderPanel.tsx";

export default function Header() {

    
    return (
        <header>
            <img src={logo} alt="Result" height="25"/>
            
            <AuthHeaderPanel/>
        </header>
    )
}