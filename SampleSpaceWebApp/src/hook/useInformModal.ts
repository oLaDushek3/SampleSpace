import {useContext} from "react";
import {InformModalContext} from "../hoc/InformModalProvider.tsx";

export default function useInformModal() {
    return useContext(InformModalContext);
}