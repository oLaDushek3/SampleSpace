import ISample from "../dal/entities/ISample.ts";

export default interface ISamplePlayer {
    sample: ISample;
    isActive: boolean;
}