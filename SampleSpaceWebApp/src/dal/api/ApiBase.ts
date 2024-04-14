import axios from "axios";

export default class ApiBase {
    static baseAddress = "http://localhost:5000/api/"

    constructor() {
        axios.defaults.withCredentials = true
    }
}