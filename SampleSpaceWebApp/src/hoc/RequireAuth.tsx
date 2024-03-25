import {Navigate} from "react-router-dom";
import * as React from "react";
import useAuth from "../hook/useAuth.ts";

interface RequireAuthProps {
    children: React.ReactNode
}

export default function RequireAuth({children}: RequireAuthProps) {
    const {user} = useAuth();

    if (!user) return <Navigate to="/"/>

    return children;
}