import {useContext} from "react";
import {SamplePlayerContext} from "../hoc/SampleProvider.tsx";

export default function useSamplePlayerContext() {
    return useContext(SamplePlayerContext);
}