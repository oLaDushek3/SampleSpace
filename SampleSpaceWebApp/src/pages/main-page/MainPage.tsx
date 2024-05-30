import {useEffect, useState} from "react";
import ISample from '../../dal/entities/ISample.ts';
import SampleList from "../../components/sample-list/SampleList.tsx";
import useSampleApi from "../../dal/api/sample/useSampleApi.ts";

export default function MainPage() {
    const {getAllSamples} = useSampleApi();
    const [samples, setSamples] = useState<ISample[] | null>(null)

    async function fetchSamples() {
        const response = await getAllSamples();
        setSamples(response);
    }

    useEffect(() => {
        void fetchSamples();
    }, [])
    
    return(
        <SampleList samples={samples}/>
    )
}