import {RefObject, useEffect} from "react";

export default function useOutsideAlerter(ref:  RefObject<any> | null, callBack: Function) {

    function handleClickOutside(event: MouseEvent) {
        if (ref !== null && ref.current && !ref.current.contains(event.target)) {
            callBack();
        }
    }
    
    useEffect(() => {
        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [ref]);
}