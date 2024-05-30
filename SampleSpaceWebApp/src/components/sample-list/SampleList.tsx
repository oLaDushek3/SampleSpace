import ISample from "../../dal/entities/ISample.ts";
import Sample from "../sample/Sample.tsx";
import sampleListClasses from "./SampleList.module.css"
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import useSamplePlayer from "../../hook/useSamplePlayer.ts";
import {useEffect, useState} from "react";
import LoadingSpinner from "../loading-spinner/LoadingSpinner.tsx";

interface SampleListProps {
    samples?: ISample[] | null
}

export default function SampleList({samples}: SampleListProps) {
    let samplePlayer = useSamplePlayer();
    const [samplePlayerList, setSamplePlayerList] = useState<ISamplePlayer[]>([]);

    useEffect(() => {
        console.log(samples);
        if(samples){
            const samplePlayerListBuffer = samples?.map(sample => ({sample: sample, isActive: false}))
            setSamplePlayerList(samplePlayerListBuffer);
            samplePlayer.handleSamplePlayerList(samplePlayerListBuffer);
        }
    }, [samples]);

    if(samples === null){
        return <LoadingSpinner/>        
    }
    
    return (
        <div className={sampleListClasses.list}>
            {samples!.length != 0 ? samplePlayerList?.map(samplePlayer => <Sample samplePlayer={samplePlayer}
                                                                             key={samplePlayer.sample.sampleGuid.toString()}/>) :
                <h1 className={"centeredWithHeader"}>Ничего нет :c</h1>
            }
        </div>
    )
}