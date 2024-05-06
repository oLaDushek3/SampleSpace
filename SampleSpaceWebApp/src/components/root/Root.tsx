import Header from "../header/Header.tsx";
import {Outlet} from "react-router-dom";

export default function Root() {
    return (
        <>
            <Header/>
            <Outlet/>
        </>
    )
}