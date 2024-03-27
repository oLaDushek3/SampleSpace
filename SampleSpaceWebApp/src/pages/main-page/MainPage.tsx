import {useEffect, useState} from "react";
import ISample from "../../dal/models/ISample.ts";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import SampleList from "../../components/sample-list/SampleList.tsx";

export default function MainPage() {
    const [samples, setSamples] = useState<ISample[]>([])

    async function fetchSamples() {
        const response = await SampleApi.getAllSamples();
        setSamples(response);
    }

    useEffect(() => {
        fetchSamples();
    }, [])
    
    return(
        <SampleList samples={samples}/>
    )
}