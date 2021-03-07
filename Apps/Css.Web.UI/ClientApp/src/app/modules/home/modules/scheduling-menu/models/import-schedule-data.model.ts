import { ImportAgentScheduleRanges } from './import-agent-schedule-ranges.model';

export class ImportScheduleData {
    employeeId: number;
    ranges: ImportAgentScheduleRanges[];

    constructor() {
        this.ranges = [];
    }
}
