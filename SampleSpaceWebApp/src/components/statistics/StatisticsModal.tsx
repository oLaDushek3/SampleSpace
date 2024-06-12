import statisticsModalClasses from "./StatisticsModal.module.css";
import Button from "../button/Button.tsx";
import ISample from "../../dal/entities/ISample.ts";
import useAuth from "../../hook/useAuth.ts";
import {Bar} from "react-chartjs-2";
import {
    Chart,
    ArcElement,
    LineElement,
    BarElement,
    PointElement,
    BarController,
    BubbleController,
    DoughnutController,
    LineController,
    PieController,
    PolarAreaController,
    RadarController,
    ScatterController,
    CategoryScale,
    LinearScale,
    LogarithmicScale,
    RadialLinearScale,
    TimeScale,
    TimeSeriesScale,
    Decimation,
    Filler,
    Legend,
    Title,
    Tooltip
} from 'chart.js';
import useSampleApi from "../../dal/api/sample/useSampleApi.ts";
import {useEffect, useRef, useState} from "react";

Chart.register(
    ArcElement,
    LineElement,
    BarElement,
    PointElement,
    BarController,
    BubbleController,
    DoughnutController,
    LineController,
    PieController,
    PolarAreaController,
    RadarController,
    ScatterController,
    CategoryScale,
    LinearScale,
    LogarithmicScale,
    RadialLinearScale,
    TimeScale,
    TimeSeriesScale,
    Decimation,
    Filler,
    Legend,
    Title,
    Tooltip
);

interface StatisticsModalProps {
    onClose: Function;
}

export default function StatisticsModal({onClose}: StatisticsModalProps) {
    const {getUserSamples} = useSampleApi();
    const [samples, setSamples] = useState<ISample[]>([])
    const chartRef = useRef<HTMLDivElement>(null);
    const {generateExcel, generateWord} = useSampleApi();
    const {loginUser} = useAuth();

    async function fetchUserSamples() {
        const response = await getUserSamples(loginUser?.userGuid);
        setSamples(response);
    }

    useEffect(() => {
        void fetchUserSamples();
    }, []);

    useEffect(() => {
        chartRef!.current!.style.width = `${samples.length * 50}px`;
    }, [samples]);

    async function getWordFile() {
        await generateWord(loginUser!.userGuid);
    }

    async function getExcelFile() {
        await generateExcel(loginUser!.userGuid);
    }

    return (
        <div className={statisticsModalClasses.statisticsPanel}>
            <div className={statisticsModalClasses.chartContainer}>
                <div ref={chartRef}>
                    {samples?.length > 0 ?
                        <Bar height={300}
                             options={{
                                 maintainAspectRatio: false,
                             }}
                             data={{
                                 labels: samples!.map((sample) => sample.name),
                                 datasets: [
                                     {
                                         label: "Количество прослушиваний",
                                         data: samples!.map((sample) => sample.numberOfListens),
                                         backgroundColor: "#759",
                                         borderRadius: 5
                                     }
                                 ]
                             }}/> :
                        <h1>Статистика отсутствует</h1>}
                </div>
            </div>


            <div className={statisticsModalClasses.buttonPanel + " horizontalPanel"}>
                <Button primary={true}
                        onClick={getWordFile}>
                    Скачать docx
                </Button>

                <Button primary={true}
                        onClick={getExcelFile}>
                    Скачать xlsx
                </Button>
            </div>

            <p style={{textAlign: "center", margin: "0.5rem", cursor: "pointer"}}
               onClick={() => onClose()}>Назад</p>
        </div>

    )
}