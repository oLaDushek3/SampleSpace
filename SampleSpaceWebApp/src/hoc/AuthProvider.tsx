import React, {createContext} from "react";
import IUser from "../dal/entities/IUser.ts"
import useLocalStorageState from 'use-local-storage-state'

interface AuthContextType {
    loginUser: IUser | null,
    setUser: Function,
    delUser: Function
}

export const AuthContext = createContext<AuthContextType>({
    loginUser: null,
    setUser: () => undefined,
    delUser: () => undefined,
});

interface AuthProviderProps {
    children: React.ReactNode;
}

export default function AuthProvider({children}: AuthProviderProps) {
    const [loginUser, setLoginUser] = useLocalStorageState<IUser | null>('user', {defaultValue: null})

    const setUser = (loginUser: IUser) => {
        setLoginUser(loginUser);
    }
    
    const delUser = () => {
        setLoginUser(null);
    }
    
    const value= {loginUser, setUser, delUser}

    return <AuthContext.Provider value={value}>
        {children}
    </AuthContext.Provider>
}