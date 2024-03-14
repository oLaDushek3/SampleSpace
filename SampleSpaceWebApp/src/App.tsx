import {useEffect, useState} from "react";
import Header from "./components/header/Header.tsx";
import ISample from "./dal/models/ISample.ts";
import SampleApi from "./dal/api/sample/SampleApi.ts";
import Sample from "./components/sample/Sample.tsx";

function App() {
    const [samples, setSamples] = useState<ISample[]>([])
    
    async function fetchSample() {
        const response = await SampleApi.getAllSamples();
        setSamples(response);
    }
    
    useEffect(() => {
        fetchSample();
    }, [])
    
    return (
        <>
            <Header/>

            <div style={{display: "flex", flexWrap: "wrap", justifyContent: "center"}}>
                { samples.map(sample => <Sample sample={sample} key={sample.sampleGuid.toString()}/>) }
            </div>
            
        </>
    )
}

export default App
