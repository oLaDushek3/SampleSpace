import {useEffect, useState} from "react";
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import SamplePageClasses from "./SamplePage.module.css";
import {IoPause, IoPlay} from "react-icons/io5";
import useSamplePlayerContext from "../../hook/useSamplePlayerContext.ts";
import Button, {ButtonVisualType} from "../../components/button/Button.tsx";
import {useParams} from "react-router-dom";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import {SlSocialSoundcloud, SlSocialSpotify, SlSocialVkontakte} from "react-icons/sl";
import Icon from "../../components/icon/Icon.tsx";

interface SampleModalProps {
}

export default function SamplePage({}: SampleModalProps) {
    const {sampleGuid} = useParams<{ sampleGuid: string }>();
    const [samplePlayer, setSamplePlayer] = useState<ISamplePlayer>()
    const {
        handlePlayingSamplePlayer,
        handlePlayPause,
        handleSeek,
        currentTime,
        isPlaying
    } = useSamplePlayerContext()

    async function fetchUser() {
        const response = await SampleApi.getSample(sampleGuid!);
        setSamplePlayer({sample: response, isActive: false});
    }

    useEffect(() => {
        fetchUser()
    }, [sampleGuid]);

    function formatDuration(durationSeconds: number) {
        const minutes = Math.floor(durationSeconds / 60);
        const seconds = Math.floor(durationSeconds % 60);
        const formattedSeconds = seconds.toString().padStart(2, "0");
        return `${minutes}:${formattedSeconds}`;
    }

    const handlePlayback = () => {
        if (!samplePlayer!.isActive)
            handlePlayingSamplePlayer(samplePlayer!);
        else
            handlePlayPause();
    }

    return (
        <div className={SamplePageClasses.sampleModal + " verticalPanel"}>
            
            <div className={SamplePageClasses.samplePanel + " horizontalPanel"}>
                <img className={SamplePageClasses.cover} src={samplePlayer?.sample.coverLink} alt="Cover image"/>

                <div className={SamplePageClasses.mainSpace + " verticalPanel"}>
                    <div>
                        <h3>{samplePlayer?.sample.name}</h3>
                        <p>{samplePlayer?.sample.artist}</p>
                    </div>

                    <div className={SamplePageClasses.track + " horizontalPanel"}>
                        <Button visualType={ButtonVisualType.icon}
                                isPrimary={true}
                                onClick={handlePlayback}>
                            {samplePlayer?.isActive && isPlaying ? <IoPause/> : <IoPlay/>}
                        </Button>

                        <input type="range"
                               min="0"
                               max={samplePlayer?.sample.duration}
                               step="0.001"
                               value={samplePlayer?.isActive ? currentTime : 0}
                               onChange={samplePlayer?.isActive ? handleSeek : () => {
                               }}/>

                        {/*Исправить*/}
                        {samplePlayer &&
                            <p>{samplePlayer?.isActive
                                ? formatDuration(currentTime)
                                : formatDuration(samplePlayer.sample.duration)}</p>}
                    </div>

                    <div className={"horizontalPanel"}>
                        <a href={samplePlayer?.sample.vkontakteLink} target="_blank">
                            <Icon>
                                <SlSocialVkontakte/>
                            </Icon>
                        </a>

                        <a href={samplePlayer?.sample.spotifyLink} target="_blank">
                            <Icon>
                                <SlSocialSpotify/>
                            </Icon>
                        </a>

                        <a href={samplePlayer?.sample.soundcloudLink} target="_blank">
                            <Icon>
                                <SlSocialSoundcloud/>
                            </Icon>
                        </a>
                    </div>
                </div>
            </div>
            
        </div>
    )
}