import {useEffect, useState} from "react";
import ISample from "../../dal/entities/ISample.ts";
import SampleList from "../../components/sample-list/SampleList.tsx";
import {useSearchParams} from "react-router-dom";
import useSampleApi from "../../dal/api/sample/useSampleApi.ts";

export default function SearchPage() {
    const {searchSamples} = useSampleApi();
    const [samples, setSamples] = useState<ISample[]>([])
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search-query")
    
    async function fetchSamples() {
        const response = await searchSamples(searchQuery!);
        setSamples(response);
    }

    useEffect(() => {
        fetchSamples();
    }, [searchQuery])

    return(
        <SampleList samples={samples}/>
    )
}