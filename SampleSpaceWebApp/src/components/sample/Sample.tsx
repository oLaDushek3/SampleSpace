import {useEffect, useState} from "react";
import {useSound} from "use-sound";
import {AiFillPlayCircle, AiFillPauseCircle} from "react-icons/ai";
import {IconContext} from "react-icons";
import ISample from "../../dal/models/ISample.ts";
import classes from "./Sample.module.css";

interface SampleProps {
    sample: ISample;
}

export default function Sample({sample}: SampleProps) {
    const [isPlaying, setIsPlaying] = useState(false);
    const [time, setTime] = useState({min: 0, sec: 0});
    const [currTime, setCurrTime] = useState({min: 0, sec: 0,});

    const [seconds, setSeconds] = useState();

    const [play, {pause, duration, sound}] = useSound(sample.samplePath);

    useEffect(() => {
        if (duration) {
            const sec = duration / 1000;
            const min = Math.floor(sec / 60);
            const secRemain = Math.floor(sec % 60);
            setTime({min: min, sec: secRemain});
        }
    }, [isPlaying]);

    useEffect(() => {
        const interval = setInterval(() => {
            if (sound) {
                setSeconds(sound.seek([]));
                const min = Math.floor(sound.seek([]) / 60);
                const sec = Math.floor(sound.seek([]) % 60);
                setCurrTime({min, sec});
            }
        }, 1000);
        return () => clearInterval(interval);
    }, [sound]);

    function playingButton() {
        if (isPlaying) {
            pause();
            setIsPlaying(false);
        } else {
            play();
            setIsPlaying(true);
        }
    }

    return (
        <div className={classes.sample}>

            <img className={classes.cover} src={sample.coverPath} alt={sample.name}/>

            <div style={{margin: "0 1rem"}}>
                <h3 className={classes.name}>{sample.name}</h3>
                <p className={classes.artist}>{sample.artist}</p>

                <div>
                    <div>
                        <div className={classes.time}>
                            <p>{currTime.min}:{currTime.sec}</p>
                            <p>{time.min}:{time.sec}</p>
                        </div>
                        <input className={classes.timeline}
                               type="range"
                               min="0"
                               max={duration! / 1000}
                               value={seconds}
                               defaultValue="0"
                               onChange={e => {
                                   sound.seek([e.target.value]);
                               }}
                        />
                    </div>

                    {!isPlaying ? (
                        <button className={classes.playButton} onClick={playingButton}>
                            <IconContext.Provider value={{size: "3em", color: "#759"}}>
                                <AiFillPlayCircle/>
                            </IconContext.Provider>
                        </button>
                    ) : (
                        <button className={classes.playButton} onClick={playingButton}>
                            <IconContext.Provider value={{size: "3em", color: "#759"}}>
                                <AiFillPauseCircle/>
                            </IconContext.Provider>
                        </button>
                    )}
                </div>

            </div>
        </div>
    )
}