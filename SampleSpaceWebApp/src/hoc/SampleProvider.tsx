import {ChangeEvent, createContext, ReactNode, useEffect, useRef, useState} from "react";
import useLocalStorageState from 'use-local-storage-state'
import ISamplePlayer from "../models/ISamplePlayer.ts";
import {useLocation} from 'react-router-dom';
import useAuth from "../hook/useAuth.ts";
import useSampleApi from "../dal/api/sample/useSampleApi.ts";

export enum ActionAtTheEnd {
    pause,
    playSkipForward,
    repeat
}

export interface SamplePlayerContextType {
    samplePlayerList: ISamplePlayer[] | null;
    handleSamplePlayerList: (samplePlayerList: ISamplePlayer[]) => void;
    playingSamplePlayer: ISamplePlayer | null;
    handlePlayingSamplePlayer: (samplePlayer: ISamplePlayer) => void;
    handlePlayPause: () => void;
    handleSeek: (e: ChangeEvent<HTMLInputElement>) => void;
    currentTime: number;
    handleActionAtTheEnd: (action: ActionAtTheEnd) => void,
    currentActionAtTheEnd: ActionAtTheEnd,
    handleVolume: (e: ChangeEvent<HTMLInputElement>) => void;
    currentVolume: number,
    isPlaying: boolean;
    handlePlaySkipForward: () => void;
    handlePlaySkipPrevious: () => void;
}

export const SamplePlayerContext = createContext<SamplePlayerContextType>({
    samplePlayerList: null,
    handleSamplePlayerList: () => {
    },
    playingSamplePlayer: null,
    handlePlayingSamplePlayer: () => {
    },
    handlePlayPause: () => {
    },
    handleSeek: () => {
    },
    currentTime: 0,
    handleActionAtTheEnd: () => {
    },
    currentActionAtTheEnd: ActionAtTheEnd.pause,
    handleVolume: () => {
    },
    currentVolume: 0,
    isPlaying: false,
    handlePlaySkipForward: () => {
    },
    handlePlaySkipPrevious: () => {
    }
});

interface SamplePlayerProviderProps {
    children: ReactNode;
}

export default function SamplePlayerProvider({children}: SamplePlayerProviderProps) {
    const [samplePlayerList, setSamplePlayerList] = useState<ISamplePlayer[] | null>(null)
    const [playingSamplePlayer, setPlayingSamplePlayer] = useState<ISamplePlayer | null>(null)
    const audioRef = useRef<HTMLAudioElement>(null);
    const [isPlaying, setIsPlaying] = useState(false);
    const [currentTime, setCurrentTime] = useState(0);
    const [currentActionAtTheEnd, setCurrentActionAtTheEnd] = useLocalStorageState<ActionAtTheEnd>("actionAtTheEnd", {defaultValue: ActionAtTheEnd.playSkipForward});
    const [currentVolume, setCurrentVolume] = useLocalStorageState<number>("volume", {defaultValue: 0.5});
    const location = useLocation();

    const {loginUser} = useAuth();
    const {addAnListensToSample} = useSampleApi();
    const [listenTimeout, setListenTimeout] = useState<ReturnType<typeof setTimeout> | null>(null)
    const [listen, setListen] = useState(false)
    
    useEffect(() => {
        setPlayingSamplePlayer(null);
        setSamplePlayerList(null);
    }, [location]);

    const handleSamplePlayerList = (samplePlayerList: ISamplePlayer[]) => {
        setSamplePlayerList(samplePlayerList);
    }

    useEffect(() => {
        handlePause();
    }, [samplePlayerList]);

    const handlePlayingSamplePlayer = (samplePlayer: ISamplePlayer) => {        
        if (playingSamplePlayer)
            playingSamplePlayer.isActive = false;

        //Attaching a timestamp to the URL to avoid caching 
        samplePlayer.sample.sampleLink += '?' + new Date().getTime();
        samplePlayer.isActive = true;
        setPlayingSamplePlayer(samplePlayer);
        setIsPlaying(true);

        if(listenTimeout){
            setListen(false);
            clearTimeout(listenTimeout);
        }
        
        if(loginUser?.userGuid != samplePlayer.sample.userGuid){
            setListenTimeout(setTimeout(() => handleListen(samplePlayer.sample.sampleGuid), 5000));
        }
    }

    useEffect(() => {
        audioRef.current?.addEventListener("timeupdate", handleTimeUpdate);
        return () => {
            audioRef.current?.removeEventListener("timeupdate", handleTimeUpdate);
        };
    }, [playingSamplePlayer, currentActionAtTheEnd]);

    const handleListen = (sampleGuid: string) => {
        setListen(true);
        addAnListensToSample(sampleGuid);
    }
    
    const handlePlay = () => {
        if(loginUser?.userGuid != playingSamplePlayer?.sample.userGuid && !listen){
            setListenTimeout(setTimeout(() => handleListen(playingSamplePlayer!.sample.sampleGuid), 5000));
        }
        
        void audioRef.current!.play();
        setIsPlaying(true);
    };

    const handlePause = () => {
        if(listenTimeout)
            clearTimeout(listenTimeout);
        
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

        if (audioRef.current!.currentTime === audioRef.current!.duration) {
            console.log(ActionAtTheEnd[currentActionAtTheEnd]);
            if (currentActionAtTheEnd === ActionAtTheEnd.pause) {
                handlePause();
                return
            }

            if (currentActionAtTheEnd === ActionAtTheEnd.playSkipForward) {
                handlePlaySkipForward();
                return
            }

            if (currentActionAtTheEnd === ActionAtTheEnd.repeat) {
                audioRef.current!.currentTime = 0;
                handlePlay();
                return;
            }
        }
    };

    const handleSeek = (e: ChangeEvent<HTMLInputElement>) => {
        if(listenTimeout)
            clearTimeout(listenTimeout);
        
        audioRef.current!.currentTime = +e.currentTarget.value;
        setCurrentTime(+e.currentTarget.value);
    };

    const handleActionAtTheEnd = (action: ActionAtTheEnd) => {
        setCurrentActionAtTheEnd(action);
    }

    const handlePlaySkipForward = () => {
        const index = samplePlayerList?.indexOf(playingSamplePlayer!)

        if (index && index + 1 === samplePlayerList!.length) {
            handlePause();
            return;
        }

        handlePlayingSamplePlayer(samplePlayerList![samplePlayerList!.indexOf(playingSamplePlayer!) + 1]);
    };

    const handlePlaySkipPrevious = () => {
        const index = samplePlayerList?.indexOf(playingSamplePlayer!)

        if (index === 0) {
            handlePause();
            return;
        }

        handlePlayingSamplePlayer(samplePlayerList![samplePlayerList!.indexOf(playingSamplePlayer!) - 1]);
    };

    const handleVolume = (e: ChangeEvent<HTMLInputElement>) => {
        setCurrentVolume(+e.currentTarget.value);
    }

    useEffect(() => {
        audioRef.current!.volume = +currentVolume;
    }, [currentVolume]);

    const value = {
        samplePlayerList: samplePlayerList,
        handleSamplePlayerList,
        playingSamplePlayer: playingSamplePlayer,
        handlePlayingSamplePlayer,
        handlePlayPause,
        handleSeek,
        currentTime,
        handleActionAtTheEnd,
        currentActionAtTheEnd,
        handleVolume,
        currentVolume,
        isPlaying,
        handlePlaySkipForward,
        handlePlaySkipPrevious
    }

    return <SamplePlayerContext.Provider value={value}>
        <audio ref={audioRef} src={playingSamplePlayer?.sample.sampleLink}
               autoPlay={true}/>
        {children}
    </SamplePlayerContext.Provider>
}