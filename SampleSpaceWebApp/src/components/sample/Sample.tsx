import sampleClasses from "./Sample.module.css"
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import Button, {ButtonVisualType} from "../button/Button.tsx";
import {IoPlay, IoPause} from "react-icons/io5";
import useSamplePlayer from "../../hook/useSamplePlayer.ts";
import {useNavigate} from "react-router-dom";

interface SampleProps {
    samplePlayer: ISamplePlayer;
}

export default function Sample({samplePlayer}: SampleProps) {
    const navigate = useNavigate()
    const {
        handlePlayingSamplePlayer,
        handlePlayPause,
        handleSeek,
        currentTime,
        isPlaying
    } = useSamplePlayer()
    
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
            <img className={sampleClasses.cover} src={samplePlayer.sample.coverLink} alt="Cover image"
                 onClick={() => {navigate({pathname: `/sample/${samplePlayer.sample.sampleGuid}`})}}/>

            <div className={sampleClasses.mainSpace + " verticalPanel"}>
                <div>
                    <h3 className={"singleLineText"}>{samplePlayer.sample.name}</h3>
                    <p className={"singleLineText"}>{samplePlayer.sample.artist}</p>
                </div>

                <div className={sampleClasses.track + " horizontalPanel"}>
                    <Button visualType={ButtonVisualType.withIcon}
                            primary={true}
                            onClick={handlePlayback}>
                        {samplePlayer.isActive && isPlaying ? <IoPause/> : <IoPlay/>}
                    </Button>
                    
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
            </div>
        </div>
    )
}