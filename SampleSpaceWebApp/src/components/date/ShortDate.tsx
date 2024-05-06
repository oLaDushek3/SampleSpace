import moment from "moment";
import "moment/dist/locale/ru";
import {useEffect, useState} from "react";
import shortDateClasses from "./ShortDate.module.css"

interface DateProps {
    date: Date;
}

export default function ShortDate({date}: DateProps) {
    const [shortDate, setShortDate] = useState("")


    useEffect(() => {
        let format;

        if(new Date(date).getFullYear() >= new Date().getFullYear())
            format = 'D MMMM';
        else
            format = moment(date).format('D MMMM Y');
        
        setShortDate(moment(date).format(format));
    }, []);
    
    return (
        <p className={shortDateClasses.shortDate}>{shortDate}</p>
    )
}