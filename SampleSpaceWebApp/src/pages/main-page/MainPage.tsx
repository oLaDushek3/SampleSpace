import useSampleApi from "../../dal/api/sample/useSampleApi.ts";
import SampleList from "../../components/sample-list/SampleList.tsx";

export default function MainPage() {
    const {getSortByDate} = useSampleApi();
    
    return(
        <SampleList fetchFunction={(numberOfPage: number) => getSortByDate(18, numberOfPage)}/>
    )
}