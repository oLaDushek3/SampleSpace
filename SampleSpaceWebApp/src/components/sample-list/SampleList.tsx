import ISample from "../../dal/entities/ISample.ts";
import Sample from "../sample/Sample.tsx";
import sampleListClasses from "./SampleList.module.css"
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import useSamplePlayerContext from "../../hook/useSamplePlayerContext.ts";
import {useEffect, useState} from "react";

interface SampleListProps {
    samples?: ISample[]
}

export default function SampleList({samples = []}: SampleListProps) {
    let samplePlayer = useSamplePlayerContext();
    const [samplePlayerList, setSamplePlayerList] = useState<ISamplePlayer[]>([]);

    useEffect(() => {
        const samplePlayerListBuffer = samples?.map(sample => ({sample: sample, isActive: false}))
        setSamplePlayerList(samplePlayerListBuffer);
        samplePlayer.handleSamplePlayerList(samplePlayerListBuffer);
    }, [samples]);

    return (
        <div className={sampleListClasses.list}>
            {samplePlayer ? samplePlayerList?.map(samplePlayer => <Sample samplePlayer={samplePlayer}
                                                                          key={samplePlayer.sample.sampleGuid.toString()}/>) :
                <h1>Ничего нет :c</h1>
            }
        </div>
    )
}