import React, {createContext, useRef, useState} from "react";
import Modal from "../components/modal/Modal.tsx";
import InformModal from "../components/dialog/inform/InformModal.tsx";
import useClickOutside from "../hook/useClickOutside.ts";

interface InformModalContextType {
    showInform: Function
}

export const InformModalContext = createContext<InformModalContextType>({
    showInform: () => undefined,
});

interface InformProviderProps {
    children: React.ReactNode;
}

export default function InformModalProvider({children}: InformProviderProps) {
    const wrapperRef = useRef(null);
    const [informIsOpen, setInformIsOpen] = useState(false);
    useClickOutside(wrapperRef, () => setInformIsOpen(false));

    const [message, setMessage] = useState("")
    
    const showInform = (message: string) => {
        setMessage(message);
        setInformIsOpen(true);
    }

    const value= {showInform}

    return <InformModalContext.Provider value={value}>
        <Modal open={informIsOpen}>
            <InformModal message={message}
                         onClose={() => setInformIsOpen(false)}/>
        </Modal>
        {children}
    </InformModalContext.Provider>
}