import sampleClasses from "./Sample.module.css"
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import Button, {ButtonVisualType} from "../button/Button.tsx";
import {IoPlay, IoPause} from "react-icons/io5";
import useSamplePlayerContext from "../../hook/useSamplePlayerContext.ts";
import Modal from "../modal/Modal.tsx";
import {useState} from "react";
import SampleModal from "../sample-modal/SampleModal.tsx";

interface SampleProps {
    samplePlayer: ISamplePlayer;
}

export default function Sample({samplePlayer}: SampleProps) {
    const [sampleModalIsOpen, setSampleModalIsOpen] = useState(false)
    const {
        handlePlayingSamplePlayer,
        handlePlayPause,
        handleSeek,
        currentTime,
        isPlaying
    } = useSamplePlayerContext()

    function formatDuration(durationSeconds: number) {
        const minutes = Math.floor(durationSeconds / 60);
        const seconds = Math.floor(durationSeconds % 60);
        const formattedSeconds = seconds.toString().padStart(2, "0");
        return `${minutes}:${formattedSeconds}`;
    }

    const handlePlayback = () => {
        if (!samplePlayer.isActive)
            handlePlayingSamplePlayer(samplePlayer);
        else
            handlePlayPause();
    }

    return (
        <div className={sampleClasses.sample + " horizontalPanel"}>
            <img className={sampleClasses.cover} src={samplePlayer.sample.coverPath} alt="Cover image"
                 onClick={() => {setSampleModalIsOpen(true)}}/>

            <div className={sampleClasses.mainSpace + " verticalPanel"}>
                <div>
                    <h3>{samplePlayer.sample.name}</h3>
                    <p>{samplePlayer.sample.artist}</p>
                </div>

                <div className={"horizontalPanel"}>
                    <input type="range"
                           min="0"
                           max={samplePlayer.sample.duration}
                           step="0.001"
                           value={samplePlayer.isActive ? currentTime : 0}
                           onChange={samplePlayer.isActive ? handleSeek : () => {
                           }}/>

                    <p>{samplePlayer.isActive
                        ? formatDuration(currentTime)
                        : formatDuration(samplePlayer.sample.duration)}</p>
                </div>

                <Button visualType={ButtonVisualType.icon}
                        isPrimary={true}
                        onClick={handlePlayback}>
                    {samplePlayer.isActive && isPlaying ? <IoPause/> : <IoPlay/>}
                </Button>
            </div>

            <Modal open={sampleModalIsOpen}>
                <SampleModal onClose={() => {setSampleModalIsOpen(false)}}/>
            </Modal>
        </div>
    )
}