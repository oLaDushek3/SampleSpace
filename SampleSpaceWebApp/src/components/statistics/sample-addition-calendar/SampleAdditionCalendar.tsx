import ActivityCalendar, {ThemeInput} from "react-activity-calendar";
import ISampleAdditionStatistic from "../../../dal/entities/ISampleAdditionStatistic.ts";
import {useEffect, useState} from "react";

interface SampleAdditionCalendarProps {
    data: ISampleAdditionStatistic[]
}

export default function SampleAdditionCalendar({data}: SampleAdditionCalendarProps) {
    const [activityCalendarData, setActivityCalendarData] = useState(data)
    
    useEffect(() => {
        data.unshift({date: "2024-01-01", count: 0, level: 0});
        setActivityCalendarData(data);
    }, [data]);
    
    const explicitTheme: ThemeInput = {
        light: ['#E2E2E2', '#BDA8D1', '#759']
    };

    const labels = {
        months: [
            'Янв',
            'Февр',
            'Март',
            'Aпр',
            'Май',
            'Июнь',
            'Июлб',
            'Авг',
            'Сент',
            'Окт',
            'Нояб',
            'Дек',
        ],
        weekdays: [
            'Пн',
            'Вт',
            'Сс',
            'Чт',
            'Пт',
            'Сб',
            'Вс',
        ],
        totalCount: '{{count}} семплов добавилено в {{year}}',
        legend: {
            less: 'Меньше',
            more: 'Больше',
        },
    };
    
    return (
        <ActivityCalendar colorScheme={"light"} theme={explicitTheme}
                          blockRadius={5} blockSize={14}
                          labels={labels}
                          maxLevel={2} data={activityCalendarData}/>
    )
}