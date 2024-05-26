import {Navigate} from "react-router-dom";
import * as React from "react";
import useAuth from "../hook/useAuth.ts";

interface RequireAuthProps {
    children: React.ReactNode
}

export default function RequireAuth({children}: RequireAuthProps) {
    const {loginUser} = useAuth();

    if (!loginUser) return <Navigate to="/"/>

    return children;
}