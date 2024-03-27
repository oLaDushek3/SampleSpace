import ISample from "../../dal/models/ISample.ts";
import statisticsModalClasses from "./StatisticsModal.module.css";
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
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import Button from "../button/Button.tsx";

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
    samples?: Array<ISample>;
    onClose: Function;
}

export default function StatisticsModal({samples = [], onClose}: StatisticsModalProps) {
    
    async function getWordFile() {
        await SampleApi.generateWord("fed85493-ccc8-42e9-9b17-1b770ac06393");
    }

    async function getExcelFile() {
        await SampleApi.generateExcel("fed85493-ccc8-42e9-9b17-1b770ac06393");
    }
    
    return (
        <div className={statisticsModalClasses.statisticsPanel}>
            {samples?.length > 0 ?
                <Bar data={{
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


            <div className={statisticsModalClasses.buttonPanel}>
                <Button isPrimary={true}
                        onClick={getWordFile}>
                    Скачать docx
                </Button>

                <Button isPrimary={true}
                        onClick={getExcelFile}>
                    Скачать xlsx
                </Button>
            </div>
            
            <p style={{textAlign: "center", margin: "0.5rem", cursor: "pointer"}}
               onClick={() => onClose()}>Назад</p>
        </div>

    )
}