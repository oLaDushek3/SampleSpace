import playerHeaderPanelClasses from "./PlayerHeaderPanel.module.css"
import {useEffect, useState} from "react";
import {IoPause, IoPlay, IoPlaySkipBack, IoPlaySkipForward, IoVolumeHigh, IoPlayForward} from "react-icons/io5";
import {FaRepeat} from "react-icons/fa6";
import Button, {ButtonVisualType} from "../../button/Button.tsx";
import useSamplePlayer from "../../../hook/useSamplePlayer.ts";
import Icon from "../../icon/Icon.tsx";
import {ActionAtTheEnd} from "../../../hoc/SampleProvider.tsx";

interface PlayerHeaderPanelProps {
    isActive: boolean;
}

export default function PlayerHeaderPanel({isActive = false}: PlayerHeaderPanelProps) {
    const [classes, setClasses] = useState(playerHeaderPanelClasses.playerPanel)
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
    } = useSamplePlayer()

    useEffect(() => {
        if (isActive)
            setClasses(playerHeaderPanelClasses.playerPanel + " " + playerHeaderPanelClasses.active);
        else
            setClasses(playerHeaderPanelClasses.playerPanel);

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
                <div className={playerHeaderPanelClasses.samplePanel + " horizontalPanel"}>
                    <img className={playerHeaderPanelClasses.cover} src={playingSamplePlayer.sample?.coverLink}
                         alt="Cover image"/>

                    <div className={playerHeaderPanelClasses.samplePanel + " verticalPanel"}>
                        <div>
                            <h3 className={"singleLineText"}>{playingSamplePlayer.sample?.name}</h3>
                            <p className={"singleLineText"}>{playingSamplePlayer.sample?.artist}</p>
                        </div>
                    </div>
                </div>}

            <div className={playerHeaderPanelClasses.playbackControlPanel + " horizontalPanel"}>
                <Button isActive={playingSamplePlayer?.sample != null && !playingSampleIsFirst}
                        visualType={ButtonVisualType.withIcon}
                        isPrimary={true}
                        onClick={handlePlaySkipPrevious}>
                    <IoPlaySkipBack/>
                </Button>

                <Button isActive={playingSamplePlayer?.sample != null}
                        visualType={ButtonVisualType.withIcon}
                        isPrimary={true}
                        onClick={handlePlayPause}>
                    {isPlaying && isPlaying ? <IoPause/> : <IoPlay/>}
                </Button>

                <Button isActive={playingSamplePlayer?.sample != null && !playingSampleIsLast}
                        visualType={ButtonVisualType.withIcon}
                        isPrimary={true}
                        onClick={handlePlaySkipForward}>
                    <IoPlaySkipForward/>
                </Button>

                <Button visualType={ButtonVisualType.withIcon}
                        isPrimary={true}
                        onClick={handleActionAtTheEndButton}>
                    {currentActionAtTheEnd === ActionAtTheEnd.pause ?
                        <IoPause/> : currentActionAtTheEnd === ActionAtTheEnd.playSkipForward ? <IoPlayForward/> :
                            <FaRepeat/>}
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