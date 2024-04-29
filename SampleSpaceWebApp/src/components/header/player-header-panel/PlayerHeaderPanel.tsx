import playerHeaderPanelClasses from "./PlayerHeaderPanel.module.css"
import {useEffect, useState} from "react";
import {IoPause, IoPlay, IoPlaySkipBack, IoPlaySkipForward, IoVolumeHigh, IoPlayForward} from "react-icons/io5";
import {FaRepeat} from "react-icons/fa6";
import Button, {ButtonVisualType} from "../../button/Button.tsx";
import useSamplePlayerContext from "../../../hook/useSamplePlayerContext.ts";
import Icon from "../../icon/Icon.tsx";
import {ActionAtTheEnd} from "../../../hoc/SampleProvider.tsx";

interface PlayerHeaderPanelProps {
    isActive: boolean;
}

export default function PlayerHeaderPanel({isActive = false}: PlayerHeaderPanelProps) {
    const [classes, setClasses] = useState(playerHeaderPanelClasses.settings)
    const [playingSampleIsFirst, setPlayingSampleIsFirst] = useState(false)
    const [playingSampleIsLast, setPlayingSampleIsLast] = useState(false)
    const [volumeClasses, setVolumeClasses] = useState(playerHeaderPanelClasses.volume)
    const {
        samplePlayerList,
        playingSamplePlayer,
        handlePlayPause,
        isPlaying,
        handleActionAtTheEnd,
        currentActionAtTheEnd,
        handlePlaySkipPrevious,
        handlePlaySkipForward,
        handleVolume,
        currentVolume
    } = useSamplePlayerContext()

    useEffect(() => {
        if (isActive)
            setClasses(playerHeaderPanelClasses.settings + " " + playerHeaderPanelClasses.active);
        else
            setClasses(playerHeaderPanelClasses.settings);

    }, [isActive]);

    const handleActionAtTheEndButton = () => {
        if (currentActionAtTheEnd === ActionAtTheEnd.pause) {
            handleActionAtTheEnd(ActionAtTheEnd.playSkipForward);
            return;
        }

        if (currentActionAtTheEnd === ActionAtTheEnd.playSkipForward) {
            handleActionAtTheEnd(ActionAtTheEnd.repeat);
            return;
        }

        if (currentActionAtTheEnd === ActionAtTheEnd.repeat) {
            handleActionAtTheEnd(ActionAtTheEnd.pause);
            return;
        }
    }

    const handleVolumeMouseEnter = () => {
        setVolumeClasses(playerHeaderPanelClasses.volume + " " + playerHeaderPanelClasses.active);
    }

    const handleVolumeMouseLeave = () => {
        setVolumeClasses(playerHeaderPanelClasses.volume);
    }

    useEffect(() => {
        const index = samplePlayerList?.indexOf(playingSamplePlayer!)

        if (index === 0) {
            setPlayingSampleIsFirst(true);
            setPlayingSampleIsLast(false);
            return
        }

        if (index && index + 1 === samplePlayerList!.length) {
            setPlayingSampleIsLast(true);
            setPlayingSampleIsFirst(false);
            return;
        }

        setPlayingSampleIsLast(false);
        setPlayingSampleIsFirst(false);

    }, [playingSamplePlayer]);

    return (
        <div className={classes + " verticalPanel"}>
            {playingSamplePlayer &&
                <div className={"horizontalPanel"}>
                    <img className={playerHeaderPanelClasses.cover} src={playingSamplePlayer.sample?.coverLink}
                         alt="Cover image"/>

                    <div className={"verticalPanel"}>
                        <div>
                            <h3>{playingSamplePlayer.sample?.name}</h3>
                            <p style={{overflow: "hidden"}}>{playingSamplePlayer.sample?.artist}</p>
                        </div>
                    </div>
                </div>}

            <div className={playerHeaderPanelClasses.playbackControlPanel + " horizontalPanel"}>
                <Button isActive={playingSamplePlayer?.sample != null && !playingSampleIsFirst}
                        visualType={ButtonVisualType.icon}
                        isPrimary={true}
                        onClick={handlePlaySkipPrevious}>
                    <IoPlaySkipBack/>
                </Button>

                <Button isActive={playingSamplePlayer?.sample != null}
                        visualType={ButtonVisualType.icon}
                        isPrimary={true}
                        onClick={handlePlayPause}>
                    {isPlaying && isPlaying ? <IoPause/> : <IoPlay/>}
                </Button>

                <Button isActive={playingSamplePlayer?.sample != null && !playingSampleIsLast}
                        visualType={ButtonVisualType.icon}
                        isPrimary={true}
                        onClick={handlePlaySkipForward}>
                    <IoPlaySkipForward/>
                </Button>

                <Button visualType={ButtonVisualType.icon}
                        isPrimary={true}
                        onClick={handleActionAtTheEndButton}>
                    {currentActionAtTheEnd === ActionAtTheEnd.pause ?
                        <IoPause/> : currentActionAtTheEnd === ActionAtTheEnd.playSkipForward ? <FaRepeat/> :
                            <IoPlayForward/>}
                </Button>

                <div className={"horizontalPanel"}
                     onMouseEnter={handleVolumeMouseEnter}
                     onMouseLeave={handleVolumeMouseLeave}>
                    <Icon>
                        <IoVolumeHigh/>
                    </Icon>
                    <input className={volumeClasses}
                           type="range"
                           min="0"
                           max="1"
                           step="0.01"
                           value={currentVolume}
                           onChange={handleVolume}/>
                </div>
            </div>
        </div>
    )
}