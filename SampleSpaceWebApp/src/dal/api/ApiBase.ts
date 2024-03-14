import axios from "axios";

export default class ApiBase {
    static baseAddress = "http://localhost:5133/"

    constructor() {
        axios.defaults.withCredentials = true
    }
}