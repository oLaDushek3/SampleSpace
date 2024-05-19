import statisticsModalClasses from "./StatisticsModal.module.css";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
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
    const {user} = useAuth();
    
    async function getWordFile() {
        await SampleApi.generateWord(user!.userGuid);
    }

    async function getExcelFile() {
        await SampleApi.generateExcel(user!.userGuid);
    }
    
    return (
        <div className={statisticsModalClasses.statisticsPanel + " verticalPanel"}>
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