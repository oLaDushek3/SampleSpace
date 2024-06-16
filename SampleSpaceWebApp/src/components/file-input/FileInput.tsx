import Icon from "../icon/Icon.tsx";
import {MdOutlineCloudUpload} from "react-icons/md";
import {Dispatch, ReactNode, SetStateAction, useEffect, useRef} from "react";
import fileInputClasses from "./FileInput.module.css"

export enum FileInputAccept {
    image,
    audio
}

const accepts = {
    [FileInputAccept.image]: "image/png, image/jpeg",
    [FileInputAccept.audio]: "audio/mpeg, audio/wav, audio/flac",
}

interface FileInputPropsProps {
    dropMessage?: string;
    filePreview?: ReactNode;
    accept: FileInputAccept;
    onUpload: (file: File) => void;
    setError: Dispatch<SetStateAction<string>>;
    fileMaxSizeMb: number;
}

export default function FileInput({
                                      filePreview,
                                      dropMessage = `Перетащите файл сюда \n или выберите вручную`,
                                      accept,
                                      onUpload,
                                      setError,
                                      fileMaxSizeMb
                                  }: FileInputPropsProps) {
    const dropArea = useRef<HTMLLabelElement>(null);
    const inputFileRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        inputFileRef.current!.addEventListener("change", uploadFile);
        dropArea.current!.addEventListener("dragover", (e: DragEvent) => {
            e.preventDefault()
        });
        dropArea.current!.addEventListener("drop", (e: DragEvent) => {
            e.preventDefault();
            inputFileRef.current!.files = e.dataTransfer!.files;
            uploadFile();
        });
    }, []);
    
    const uploadFile = () => {
        if (!inputFileRef.current!.files)
            return;

        const selectedFile = inputFileRef.current!.files[0];
        
        if (!accepts[accept].includes(selectedFile.type)) {
            setError(`Не правильный формат файла`);
            return;
        }

        if (selectedFile.size > fileMaxSizeMb * 1024 * 1024) {
            setError(`Файл слишком большой. Максимальный размер: ${fileMaxSizeMb} мб`);
            return;
        }
        
        onUpload(selectedFile);
    }
    
    return (
        <label ref={dropArea} className={fileInputClasses.dropArea} htmlFor={"input-file"}>
            <input ref={inputFileRef} id={"input-file"}
                   type={"file"} accept={accepts[accept]} hidden/>

            {accept === FileInputAccept.image && filePreview ? filePreview :
                <div className={fileInputClasses.fileInput}>
                    <Icon height={125}>
                        <MdOutlineCloudUpload/>
                    </Icon>
                    {dropMessage?.trim() && <p style={{fontSize: "18px", whiteSpace: "pre-wrap"}}>{dropMessage}</p>}
                </div>}
        </label>
    )
}