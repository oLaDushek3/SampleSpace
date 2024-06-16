import {useEffect, useState} from "react";
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import ISample from "../../dal/entities/ISample.ts";
import Sample from "../sample/Sample.tsx";
import sampleListClasses from "./SampleList.module.css";
import useSamplePlayer from "../../hook/useSamplePlayer.ts";
import LoadingSpinner from "../loading-spinner/LoadingSpinner.tsx";

interface TestSampleListProps {
    fetchFunction: (numberOfPage: number) => Promise<ISample[]>,
    onDelete?: (sampleGuid: string) => void;
}

export default function SampleList({fetchFunction, onDelete}: TestSampleListProps) {
    let samplePlayer = useSamplePlayer();

    const [samples, setSamples] = useState<ISample[]>([]);
    const [samplePlayerList, setSamplePlayerList] = useState<ISamplePlayer[]>([]);

    const [currentPage, setCurrentPage] = useState(1);
    const [fetching, setFetching] = useState(true);

    useEffect(() => {
        if (fetching) {
            fetchFunction(currentPage).then((response: ISample[]) => {
                setSamples([...samples, ...response])
                setCurrentPage(prevState => prevState + 1);
                updateSamplePlayers(response);
            })
                .finally(() => setFetching(false));
        }
    }, [fetching]);

    const updateSamplePlayers = (samples: ISample[]) => {
        const samplePlayerListBuffer = samples?.map(sample => ({sample: sample, isActive: false}));
        setSamplePlayerList([...samplePlayerList, ...samplePlayerListBuffer]);
        samplePlayer.handleSamplePlayerList(samplePlayerListBuffer, true);
    }

    useEffect(() => {
        document.addEventListener('scroll', scrollHandler)
        return () => {
            document.removeEventListener('scroll', scrollHandler)
        }
    }, [])

    const scrollHandler = (e: any) => {
        if (e.target!.documentElement.scrollHeight - (e.target!.documentElement.scrollTop + window.innerHeight) < 100) {
            setFetching(true);
        }
    }

    if (samples === null) {
        return <LoadingSpinner/>
    }

    return (
        <div className={sampleListClasses.list}>
            {samples?.length != 0 ? samplePlayerList?.map(samplePlayer => <Sample samplePlayer={samplePlayer}
                                                                                  onDelete={onDelete}
                                                                                  key={samplePlayer.sample.sampleGuid.toString()}/>) :
                <h1 className={"centeredWithHeader"}>Ничего нет :c</h1>
            }
        </div>
    )
}