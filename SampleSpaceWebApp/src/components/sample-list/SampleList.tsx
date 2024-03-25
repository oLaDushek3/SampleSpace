import ISample from "../../dal/models/ISample.ts";
import Sample from "../sample/Sample.tsx";
import sampleListClasses from "./SampleList.module.css"

interface SampleListProps {
    samples: ISample[]
}

export default function SampleList({samples}: SampleListProps) {
    return (
        <div className={sampleListClasses.list}>
            {!samples || samples.length ?
                samples.map(sample => <Sample sample={sample} key={sample.sampleGuid.toString()}/>) :
                <h1>Ничего нет :c</h1>
            }
        </div>
    )
}