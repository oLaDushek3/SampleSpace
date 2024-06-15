import sampleClasses from "./Sample.module.css"
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import Button, {ButtonVisualType} from "../button/Button.tsx";
import {IoPlay, IoPause} from "react-icons/io5";
import {MdSpatialAudioOff} from "react-icons/md";
import useSamplePlayer from "../../hook/useSamplePlayer.ts";
import {useNavigate} from "react-router-dom";
import {CiTrash} from "react-icons/ci";
import Icon from "../icon/Icon.tsx";
import {useState} from "react";
import useAuth from "../../hook/useAuth.ts";

interface SampleProps {
    samplePlayer: ISamplePlayer;
    onDelete?: (sampleGuid: string) => void;
}

export default function Sample({samplePlayer, onDelete}: SampleProps) {
    const {loginUser} = useAuth()
    const [toolButtonsClasses, setToolButtonsClasses] = useState(sampleClasses.toolButtons)
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

    const handleOpenSample = () => {
        navigate({pathname: `/sample/${samplePlayer.sample.sampleGuid}`})
    }

    const sampleMouseEnter = () => {
        setToolButtonsClasses(`${sampleClasses.toolButtons} ${sampleClasses.active}`);
    }

    const sampleMouseLeave = () => {
        setToolButtonsClasses(sampleClasses.toolButtons);
    }

    return (
        <div className={sampleClasses.sample + " horizontalPanel"}
             onMouseEnter={sampleMouseEnter}
             onMouseLeave={sampleMouseLeave}>
            <img className={sampleClasses.cover} src={samplePlayer.sample.coverLink += '?' + new Date().getTime()}
                 alt="Cover image"
                 onClick={handleOpenSample}/>

            <div className={sampleClasses.mainSpace + " verticalPanel"}>
                <div className={"horizontalPanel"}>
                    <div className={sampleClasses.sampleInfo}>
                        <p className={sampleClasses.sampleName + " singleLineText"}
                           onClick={handleOpenSample}>
                            {samplePlayer.sample.name}
                        </p>
                        <p className={"singleLineText"}>{samplePlayer.sample.artist}</p>
                    </div>

                    {onDelete && (loginUser?.userGuid === samplePlayer.sample.sampleGuid || loginUser?.isAdmin) &&
                        <div className={toolButtonsClasses}>
                            <Button visualType={ButtonVisualType.icon}
                                    onClick={() => onDelete(samplePlayer.sample.sampleGuid)}>
                                <Icon>
                                    <CiTrash/>
                                </Icon>
                            </Button>
                        </div>}
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

                <div className={sampleClasses.numberOfListens}>
                    <Icon height={18}>
                        <MdSpatialAudioOff/>
                    </Icon>

                    <p>{samplePlayer.sample.numberOfListens}</p>
                </div>
            </div>
        </div>
    );
}