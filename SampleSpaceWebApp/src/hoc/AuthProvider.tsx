import React, {createContext, useState} from "react";
import IUser from "../dal/models/IUser.ts"

interface AuthContextType {
    user: IUser | null,
    signIn: Function,
    signOut: Function
}

export const AuthContext = createContext<AuthContextType>({
    user: null,
    signIn: () => undefined,
    signOut: () => undefined
});

interface AuthProviderProps {
    children: React.ReactNode;
}

export default function AuthProvider({children}: AuthProviderProps) {
    const [user, setUser] = useState<IUser | null>(null)

    const signIn = (loginUser: IUser) => {
        setUser(loginUser)
    }
    const signOut = () => {
        setUser(null)
    }

    const value= {user, signIn, signOut}

    return <AuthContext.Provider value={value}>
        {children}
    </AuthContext.Provider>
}