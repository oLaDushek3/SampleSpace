import {FormEvent, useEffect, useRef, useState} from "react";
import {IconContext} from "react-icons";
import {CiSearch} from "react-icons/ci";
import {IoCloseOutline} from "react-icons/io5";
import Button, {ButtonVisualType} from "../button/Button.tsx";
import searchClasses from "./Search.module.css"
interface SearchProps {
    searchQuery?: string;
    callBack: (searchQuery: string) => void;
}

export default function Search({searchQuery = "", callBack}: SearchProps) {
    const searchInput = useRef<HTMLInputElement>(null)
    const [inputValue, setInputValue] = useState(searchQuery)

    useEffect(() => {
        setInputValue(searchQuery);
    }, [searchQuery]);
    
    const handleSubmit = (event: FormEvent) => {
        event.preventDefault();

        if (inputValue !== "")
            callBack(inputValue)
    }

    return (
        <form className={searchClasses.search + " horizontalPanel"}
              onClick={() => searchInput.current!.focus()}
              onSubmit={handleSubmit}
              autoComplete="false"
              spellCheck="false">

            <IconContext.Provider value={{size: "1.5em"}}>
                <CiSearch/>
            </IconContext.Provider>

            <input ref={searchInput}
                   className={searchClasses.searchInput}
                   type={"text"}
                   placeholder={"Поиск"}
                   value={inputValue}
                   onChange={(event) => setInputValue(event.target!.value)}/>

            {inputValue?.trim() &&
                <Button type="button" 
                        visualType={ButtonVisualType.withIcon} 
                        onClick={() => setInputValue("")}>
                    <IoCloseOutline/>
                </Button>}
        </form>
    )
}