import {useContext} from "react";
import {AuthContext} from "../hoc/AuthProvider.tsx";

export default function useAuth() {
    return useContext(AuthContext);
}