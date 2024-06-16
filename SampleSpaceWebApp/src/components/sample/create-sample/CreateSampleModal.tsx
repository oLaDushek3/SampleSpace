import Button from "../../button/Button.tsx";
import createSampleModalClasses from "./CreateSampleModal.module.css";
import React, {useEffect, useRef, useState} from "react";
import useClickOutside from "../../../hook/useClickOutside.ts";
import ErrorMessage from "../../error-message/ErrorMessage.tsx";
import parse from 'id3-parser';
import {convertFileToBuffer} from 'id3-parser/lib/util';
import {IID3Tag} from "id3-parser/lib/interface";
import FileInput, {FileInputAccept} from "../../file-input/FileInput.tsx";
import SampleWave from "./sample-wave/SampleWave.tsx";
import useAuth from "../../../hook/useAuth.ts";
import useSampleApi from "../../../dal/api/sample/useSampleApi.ts";

interface CreateSampleModalProps {
    onClose: () => void;
}

interface SampleRegion {
    start: number,
    end: number
}

export default function CreateSampleModal({onClose}: CreateSampleModalProps) {
    const {createSample} = useSampleApi();
    const wrapperRef = useRef(null);
    const coverRef = useRef<HTMLImageElement>(null);
    useClickOutside(wrapperRef, onClose);
    const [uploadSample, setUploadSample] = useState<File | null>();
    const [coverFile, setCoverFile] = useState<File | null>();
    const [sampleRegion, setSampleRegion] = useState<SampleRegion>({start: 0, end: 0});
    const [name, setName] = useState("");
    const [artist, setArtist] = useState("");
    const [vkontakteLink, setVkontakteLink] = useState("");
    const [spotifyLink, setSpotifyLink] = useState("");
    const [soundcloudLink, setSoundcloudLink] = useState("");
    const [error, setError] = useState("");
    const {loginUser} = useAuth();
    const [submitLoading, setSubmitLoading] = useState(false)
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSubmitLoading(true);

        if (name.trim().length === 0 || artist.trim().length === 0 || !coverFile) {
            setError("Не все поля заполнены");
            setSubmitLoading(false);
            return
        }
        
        await createSample(uploadSample!, sampleRegion?.start, sampleRegion?.end, coverFile,
            name, artist, loginUser!.userGuid, vkontakteLink, spotifyLink, soundcloudLink);

        onClose();
    }

    const handleCancel = () => {
        if (uploadSample) {
            setUploadSample(null);
        } else
            onClose();
    }

    useEffect(() => {
        if (uploadSample) {
            convertFileToBuffer(uploadSample).then(parse).then(async tag => {
                const tags = (tag as IID3Tag);

                const cover = tags.image ? tags.image?.data as Uint8Array : null;
                const name = tags.title ? tags.title : null;
                const artist = tags.artist ? tags.artist : null;

                if (cover) {
                    const coverBlob = new Blob([cover])
                    setCoverFile(new File([coverBlob], "cover.jpg"));
                }

                if (name)
                    setName(name);

                if (artist)
                    setArtist(artist);
            });
        }
    }, [uploadSample]);

    useEffect(() => {
        if (coverFile)
            coverRef.current!.src = URL.createObjectURL(coverFile);
    }, [coverFile]);

    const handleUploadSample = (uploadSample: File) => {
        setUploadSample(uploadSample);
    }

    const handleSample = (start: number, end: number) => {
        setSampleRegion({start: start, end: end})
    }

    const coverPreview = (
        <img className={createSampleModalClasses.cover}
             alt={"Sample image"} ref={coverRef}/>)

    return (
        <div ref={wrapperRef}
             className={createSampleModalClasses.createSample + " verticalPanel"}>
            <form className={"verticalPanel"}
                  onSubmit={handleSubmit}>

                {!uploadSample ?
                    <FileInput accept={FileInputAccept.audio}
                               onUpload={handleUploadSample}
                               setError={setError}
                               fileMaxSizeMb={12.5}/> :
                    <div className={"verticalPanel"}>
                        <div className={createSampleModalClasses.samplePanel + " horizontalPanel"}>
                            <div className={createSampleModalClasses.cover}>
                                <FileInput dropMessage={""} filePreview={coverFile && coverPreview}
                                           accept={FileInputAccept.image}
                                           onUpload={(file) => setCoverFile(file)}
                                           setError={setError}
                                           fileMaxSizeMb={7.5}/>
                            </div>

                            <div className={"verticalPanel"} style={{alignContent: "center"}}>
                                <label htmlFor="name">Название</label>
                                <input id="name"
                                       className="text-input"
                                       placeholder="Введите название семпла"
                                       type="text"
                                       maxLength={75}
                                       value={name}
                                       onChange={e => setName(e.target.value)}/>

                                <label htmlFor="artist">Артист</label>
                                <input id="artist"
                                       className="text-input"
                                       placeholder="Введите имя артиста"
                                       type="text"
                                       maxLength={75}
                                       value={artist}
                                       onChange={e => setArtist(e.target.value)}/>
                            </div>
                        </div>

                        <SampleWave sampleBlob={uploadSample!}
                                    onRegionUpdate={(start, end) => {
                                        handleSample(start, end)
                                    }}/>

                        <label htmlFor="vkontakteLink">VK Музыка</label>
                        <input id="vkontakteLink"
                               className="text-input"
                               placeholder="Добавьте ссылку на VK Музыку"
                               type="text"
                               maxLength={75}
                               value={vkontakteLink}
                               onChange={e => setVkontakteLink(e.target.value)}/>

                        <label htmlFor="spotifyLink">Spotify</label>
                        <input id="spotifyLink"
                               className="text-input"
                               placeholder="Добавьте ссылку на Spotify"
                               type="text"
                               maxLength={75}
                               value={spotifyLink}
                               onChange={e => setSpotifyLink(e.target.value)}/>

                        <label htmlFor="soundcloudLink">Soundcloud</label>
                        <input id="soundcloudLink"
                               className="text-input"
                               placeholder="Добавьте ссылку на Soundcloud"
                               type="text"
                               maxLength={75}
                               value={soundcloudLink}
                               onChange={e => setSoundcloudLink(e.target.value)}/>
                    </div>}

                {error && <ErrorMessage error={error} setError={setError}/>}

                <Button active={!submitLoading} 
                        alone={true}
                        primary={true}
                        onClick={handleSubmit}>
                    Создать
                </Button>

                <Button alone={true} onClick={handleCancel}>Назад</Button>
            </form>
        </div>
    )
}