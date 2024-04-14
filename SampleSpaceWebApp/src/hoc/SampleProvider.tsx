import React, {createContext, Ref, RefObject, useEffect, useState} from "react";

interface SampleContextType {
    activeSample: RefObject<HTMLAudioElement> | null
}

export const SampleContext = createContext<SampleContextType>({
    activeSample: null,
});

interface SampleProviderProps {
    children: React.ReactNode;
}

export default function AuthProvider({children}: SampleProviderProps) {
    const [activeSample, setActiveSample] = useState<RefObject<HTMLAudioElement>>(null)

    useEffect(() => {
        activeSample.current?.pause()
    }, [activeSample]);
    
    const value= {activeSample}

    return <SampleContext.Provider value={value}>
        {children}
    </SampleContext.Provider>
}