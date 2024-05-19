import {ChangeEvent, useEffect, useRef, useState} from "react";
import {useWavesurfer} from "@wavesurfer/react";
import Minimap from 'wavesurfer.js/dist/plugins/minimap.esm.js';
import Timeline from 'wavesurfer.js/dist/plugins/timeline.esm.js';
import Regions from 'wavesurfer.js/dist/plugins/regions.esm.js';
import LoadingSpinner from "../../../loading-spinner/LoadingSpinner.tsx";
import sampleWaveClasses from "./SampleWave.module.css"
import Button, {ButtonVisualType} from "../../../button/Button.tsx";
import {IoPause, IoPlay, IoVolumeHigh} from "react-icons/io5";
import {RiForward5Fill, RiReplay5Fill} from "react-icons/ri";
import {FiZoomIn, FiZoomOut} from "react-icons/fi";
import Icon from "../../../icon/Icon.tsx";

interface SampleWaveProps {
    sampleBlob: File,
    onRegionUpdate: (start: number, end: number) => void
}

export default function SampleWave({sampleBlob, onRegionUpdate}: SampleWaveProps) {
    const [sampleUrl, setSampleUrl] = useState("")
    const sampleWaveContainerRef = useRef<HTMLDivElement>(null);

    const [volumeValue, setVolumeValue] = useState(0.5);
    const [volumeClasses, setVolumeClasses] = useState(sampleWaveClasses.volume)

    const zoomMinimumValue = 5;
    const zoomMaximumValue = 1000;
    const [zoomValue, setZoomValue] = useState(zoomMinimumValue);

    const [wavesurferReady, setWavesurferReady] = useState(false);
    const {wavesurfer, isPlaying} = useWavesurfer({
        container: sampleWaveContainerRef,
        waveColor: '#759',
        progressColor: '#8d8d8d',
        barWidth: 2,
        barGap: 1,
        minPxPerSec: 100,
        autoCenter: false,
        hideScrollbar: false,
        autoScroll: true
    });

    useEffect(() => {
        setSampleUrl(URL.createObjectURL(sampleBlob));
    }, []);

    useEffect(() => {
        if (wavesurfer) {
            void wavesurfer.load(sampleUrl);
            wavesurfer.registerPlugin(
                Minimap.create({
                    height: 35,
                    waveColor: '#ddd',
                    progressColor: '#939393',
                    dragToSeek: true,
                })
            );

            wavesurfer.registerPlugin(
                Timeline.create()
            )

            const wavesurferRegions = wavesurfer.registerPlugin(Regions.create())

            wavesurfer.on('ready', () => {
                setWavesurferReady(true);
                wavesurfer.zoom(zoomValue);
                wavesurfer.setVolume(volumeValue);

                wavesurferRegions.addRegion({
                    start: 0,
                    end: 25,
                    content: "Cемпл",
                    color: "rgba(137, 125, 148, 0.5)",
                    minLength: 15,
                    maxLength: 45
                })
            })

            let regionActive = false;

            wavesurferRegions.on('region-out', (region) => {
                if (regionActive) {
                    wavesurfer.setTime(region.start);
                    void wavesurfer.playPause();
                }
            })

            wavesurferRegions.on('region-clicked', (region, e) => {
                regionActive = true;
                e.stopPropagation()
                region.play();
                void wavesurfer.playPause();
            })

            wavesurferRegions.on('region-created', (region) => onRegionUpdate(region.start, region.end));

            wavesurferRegions.on('region-updated', (region) => onRegionUpdate(region.start, region.end));

            wavesurfer.on('interaction', () => {
                regionActive = false;
            })
        }
    }, [sampleUrl]);

    const handlePlayPause = () => {
        wavesurfer && wavesurfer.playPause();
    }

    const handlePlaySkipForward = (seconds: number) => {
        wavesurfer!.skip(seconds)
    }

    const handlePlaySkipPrevious = (seconds: number) => {
        wavesurfer!.skip(-seconds)
    }

    useEffect(() => {
        if (wavesurfer)
            wavesurfer.zoom(zoomValue)
    }, [zoomValue]);

    const handleSliderZoom = (e: ChangeEvent<HTMLInputElement>) => {
        console.log(+e.currentTarget.value);
        setZoomValue(+e.currentTarget.value);
    };

    const handleZoom = (magnification: number) => {
        if (magnification < 0 && zoomValue + zoomMinimumValue < -magnification) {
            console.log(zoomMinimumValue);
            setZoomValue(zoomMinimumValue);
            return;
        }

        if (magnification > 0 && zoomValue + magnification > zoomMaximumValue) {
            console.log(zoomMaximumValue);
            setZoomValue(zoomMaximumValue);
            return;
        }

        setZoomValue(prevState => prevState + magnification);
        console.log(zoomValue + magnification);
    };

    const handleVolumeMouseEnter = () => {
        setVolumeClasses(sampleWaveClasses.volume + " " + sampleWaveClasses.volumeActive);
    }

    const handleVolumeMouseLeave = () => {
        setVolumeClasses(sampleWaveClasses.volume);
    }

    const handleVolume = (e: ChangeEvent<HTMLInputElement>) => {
        setVolumeValue(+e.currentTarget.value);

        wavesurfer!.setVolume(volumeValue);
    };

    return (
        <>
            <div
                className={!wavesurferReady ? sampleWaveClasses.waveContainerInactive : sampleWaveClasses.waveContainerActive + " verticalPanel"}>
                <div ref={sampleWaveContainerRef}/>

                <div className={sampleWaveClasses.toolsPanel + " horizontalPanel"}>
                    <Button visualType={ButtonVisualType.withIcon}
                            primary={true}
                            onClick={() => handlePlaySkipPrevious(5)}>
                        <Icon isPrimary={false}>
                            <RiReplay5Fill/>
                        </Icon>
                    </Button>

                    <Button visualType={ButtonVisualType.withIcon}
                            primary={true}
                            onClick={handlePlayPause}>
                        {isPlaying ? <IoPause/> : <IoPlay/>}
                    </Button>

                    <Button visualType={ButtonVisualType.withIcon}
                            primary={true}
                            onClick={() => handlePlaySkipForward(5)}>
                        <Icon isPrimary={false}>
                            <RiForward5Fill/>
                        </Icon>
                    </Button>

                    <div className={sampleWaveClasses.volumePanel}
                         onMouseEnter={handleVolumeMouseEnter}
                         onMouseLeave={handleVolumeMouseLeave}>
                        <Icon>
                            <IoVolumeHigh/>
                        </Icon>
                        <input type="range" className={volumeClasses}
                               min="0" max="1" step="0.01"
                               value={volumeValue} onChange={handleVolume}/>
                    </div>

                    <div className={sampleWaveClasses.zoom}>
                        <Button visualType={ButtonVisualType.icon}
                                active={zoomValue > zoomMinimumValue}
                                onClick={() => handleZoom(-50)}>
                            <Icon>
                                <FiZoomOut/>
                            </Icon>
                        </Button>

                        <input type="range"
                               min={zoomMinimumValue} max={zoomMaximumValue} step="0.01"
                               value={zoomValue} onChange={handleSliderZoom}/>

                        <Button visualType={ButtonVisualType.icon}
                                active={zoomValue < zoomMaximumValue}
                                onClick={() => handleZoom(50)}>
                            <Icon>
                                <FiZoomIn/>
                            </Icon>
                        </Button>
                    </div>
                </div>
            </div>

            {!wavesurferReady && <LoadingSpinner/>}
        </>
    )
}