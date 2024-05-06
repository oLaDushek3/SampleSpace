import {useContext} from "react";
import {SamplePlayerContext} from "../hoc/SampleProvider.tsx";

export default function useSamplePlayer() {
    return useContext(SamplePlayerContext);
}