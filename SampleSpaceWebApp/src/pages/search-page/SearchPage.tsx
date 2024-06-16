import SampleList from "../../components/sample-list/SampleList.tsx";
import {useSearchParams} from "react-router-dom";
import useSampleApi from "../../dal/api/sample/useSampleApi.ts";

export default function SearchPage() {
    const {searchSamples} = useSampleApi();
    const [searchParams] = useSearchParams()
    const searchQuery = searchParams.get("search-query")

    return(
        <SampleList fetchFunction={(umberOfPage: number) => searchSamples(searchQuery!, 10, umberOfPage)}/>
    )
}