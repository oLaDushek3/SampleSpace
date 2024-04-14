import sampleClasses from "./Sample.module.css"
import ISample from "../../dal/models/ISample.ts";
import {ChangeEvent, useEffect, useRef, useState} from "react";
import Button, {ButtonVisualType} from "../button/Button.tsx";
import { IoPlay , IoPause } from "react-icons/io5";

interface SampleProps {
    sample: ISample;
}

export default function Sample({sample}: SampleProps) {
    const audioRef = useRef<HTMLAudioElement>(null);
    const [isPlaying, setIsPlaying] = useState(false);
    const [currentTime, setCurrentTime] = useState(0);
    const [duration, setDuration] = useState(0);

    const handlePlay = () => {
        audioRef.current!.play();
        setIsPlaying(true);
    };

    const handlePause = () => {
        audioRef.current!.pause();
        setIsPlaying(false);
    };

    const handlePlayPause = () => {
        if (isPlaying) {
            handlePause();
        } else {
            handlePlay();
        }
    };

    const handleTimeUpdate = () => {
        setCurrentTime(audioRef.current!.currentTime);
        setDuration(audioRef.current!.duration);
        
        if(audioRef.current!.currentTime === audioRef.current!.duration) {
            handlePause();
            audioRef.current!.currentTime = 0;
        }
    };

    const handleSeek = (e: ChangeEvent<HTMLInputElement>) => {
        audioRef.current!.currentTime = +e.currentTarget.value;
        setCurrentTime(+e.currentTarget.value);
    };

    function formatDuration(durationSeconds: number) {
        const minutes = Math.floor(durationSeconds / 60);
        const seconds = Math.floor(durationSeconds % 60);
        const formattedSeconds = seconds.toString().padStart(2, "0");
        return `${minutes}:${formattedSeconds}`;
    }

    useEffect(() => {
        audioRef.current!.addEventListener("timeupdate", handleTimeUpdate);
        return () => {
            audioRef.current!.removeEventListener("timeupdate", handleTimeUpdate);
        };
    }, []);

    return (
        <div className={sampleClasses.sample + " horizontalPanel"}>
            <audio ref={audioRef} src={sample.samplePath} onCanPlayThrough={handleTimeUpdate}/>
            <img className={sampleClasses.cover} src={sample.coverPath} alt="Cover image"/>

            <div className={sampleClasses.mainSpace + " verticalPanel"}>
                <div>
                    <h3>{sample.name}</h3>
                    <p>{sample.artist}</p>
                </div>
                
                <div className={sampleClasses.track + " horizontalPanel"}>
                    <input type="range"
                           min="0"
                           max={duration}
                           step="0.001"
                           value={currentTime}
                           onChange={handleSeek}/>

                    <p>{currentTime !== 0 ? formatDuration(currentTime) : formatDuration(duration)}</p>
                </div>

                <Button visualType={ButtonVisualType.icon}
                        isPrimary={true}
                        onClick={handlePlayPause}>
                    {isPlaying ? <IoPause/> : <IoPlay/>}
                </Button>
            </div>
        </div>
    )
}