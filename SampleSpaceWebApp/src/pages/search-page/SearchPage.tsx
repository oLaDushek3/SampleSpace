import {useEffect, useState} from "react";
import ISample from "../../dal/models/ISample.ts";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import SampleList from "../../components/sample-list/SampleList.tsx";
import {useSearchParams} from "react-router-dom";

export default function SearchPage() {
    const [samples, setSamples] = useState<ISample[]>([])
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search_query")
    
    async function fetchSamples() {
        console.log("fetchSamples")
        const response = await SampleApi.searchSamples(searchQuery!);
        console.log(response)
        setSamples(response);
    }

    useEffect(() => {
        console.log("useEffect")
        fetchSamples();
    }, [searchQuery])

    return(
        <SampleList samples={samples}/>
    )
}