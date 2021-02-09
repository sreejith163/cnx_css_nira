import { ImportScheduleData } from './import-schedule-data.model';

export class ImportShceduleChart {
    importAgentScheduleCharts: ImportScheduleData[];
    activityOrigin: number;
    modifiedBy: string;

    constructor() {
        this.importAgentScheduleCharts = [];
    }
}
