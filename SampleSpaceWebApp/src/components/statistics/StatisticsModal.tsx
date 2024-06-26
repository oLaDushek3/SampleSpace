import statisticsModalClasses from "./StatisticsModal.module.css";
import {RadioButton} from "../button/Button.tsx";
import ISample from "../../dal/entities/ISample.ts";
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
import SampleAdditionCalendar from "./sample-addition-calendar/SampleAdditionCalendar.tsx";
import useClickOutside from "../../hook/useClickOutside.ts";
import useUserApi from "../../dal/api/user/useUserApi.ts";
import ISampleAdditionStatistic from "../../dal/entities/ISampleAdditionStatistic.ts";
import useAuth from "../../hook/useAuth.ts";

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
    userGuid: string;
}

export enum StatisticPageType {
    numbersOfListensChartPage,
    sampleAdditionStatisticsPage
}

export default function StatisticsModal({onClose, userGuid}: StatisticsModalProps) {
    const {loginUser} = useAuth();
    const wrapperRef = useRef(null);
    useClickOutside(wrapperRef, onClose);

    const {getUserSamples} = useSampleApi();
    const {getSampleAdditionStatistics} = useUserApi();

    const [samples, setSamples] = useState<ISample[]>([])
    const chartRef = useRef<HTMLDivElement>(null);

    const [sampleAdditionStatistics, setSampleAdditionStatistics] = useState<ISampleAdditionStatistic[]>([])

    const numbersOfListensChartPage = (
        <div className={"verticalPanel"}>
            <div className={statisticsModalClasses.chartContainer}>
                <div ref={chartRef}>
                    <Bar height={300}
                         options={{
                             maintainAspectRatio: false,
                         }}
                         data={{
                             labels: samples!.reverse().map((sample) => sample.name),
                             datasets: [
                                 {
                                     label: "Количество прослушиваний",
                                     data: samples!.map((sample) => sample.numberOfListens),
                                     backgroundColor: "#759",
                                     borderRadius: 5
                                 }
                             ]
                         }}/>
                </div>
            </div>
        </div>
    )

    const sampleAdditionStatisticsPage = (
        <SampleAdditionCalendar data={sampleAdditionStatistics.reverse()}/>
    )

    const [selectedStatisticsPage, setSelectedStatisticsPage] = useState<StatisticPageType>()

    const setNumbersOfListensChartPage = async () => {
        setSelectedStatisticsPage(StatisticPageType.numbersOfListensChartPage);
        setSamples(await getUserSamples(userGuid, 0, 0) as ISample[]);
    }

    const setSampleAdditionStatisticsPage = async () => {
        setSelectedStatisticsPage(StatisticPageType.sampleAdditionStatisticsPage);
        setSampleAdditionStatistics(await getSampleAdditionStatistics(userGuid) as ISampleAdditionStatistic[]);
    }

    useEffect(() => {
        if (selectedStatisticsPage === StatisticPageType.numbersOfListensChartPage)
            if(samples.length > 6)
                chartRef!.current!.style.width = `${samples.length * 50}px`;
    }, [samples]);

    useEffect(() => {
        void setNumbersOfListensChartPage();
    }, [userGuid]);

    return (
        <div ref={wrapperRef}
             className={statisticsModalClasses.statisticsPanel + " verticalPanel"}>

            {loginUser?.isAdmin &&
                <div style={{justifyContent: "center"}} className={"horizontalPanel"}>
                    <RadioButton
                        withHorizontalScroll={true}
                        onSelected={setNumbersOfListensChartPage}
                        selected={selectedStatisticsPage === StatisticPageType.numbersOfListensChartPage}>
                        Прослушивания
                    </RadioButton>

                    <RadioButton
                        withHorizontalScroll={true}
                        onSelected={setSampleAdditionStatisticsPage}
                        selected={selectedStatisticsPage === StatisticPageType.sampleAdditionStatisticsPage}>
                        Активность
                    </RadioButton>
                </div>}

            {selectedStatisticsPage === StatisticPageType.numbersOfListensChartPage && numbersOfListensChartPage}

            {selectedStatisticsPage === StatisticPageType.sampleAdditionStatisticsPage && sampleAdditionStatisticsPage}

            <p style={{textAlign: "center", margin: "0.5rem", cursor: "pointer"}}
               onClick={() => onClose()}>Назад</p>
        </div>

    )
}