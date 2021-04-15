import { ImportAgentScheduleRanges } from './import-agent-schedule-ranges.model';

export class ImportScheduleData {
    employeeId: string;
    ranges: ImportAgentScheduleRanges[];

    constructor() {
        this.ranges = [];
    }
}
