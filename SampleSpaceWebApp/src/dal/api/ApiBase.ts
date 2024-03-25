import axios from "axios";

export default class ApiBase {
    static baseAddress = "http://212.111.84.182/api/"

    constructor() {
        axios.defaults.withCredentials = true
    }
}