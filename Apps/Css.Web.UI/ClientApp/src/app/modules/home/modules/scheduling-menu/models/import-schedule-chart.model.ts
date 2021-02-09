import { ImportScheduleData } from './import-schedule-data.model';

export class ImportShceduleChart {
    importAgentScheduleCharts: ImportScheduleData[];
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;

    constructor() {
        this.importAgentScheduleCharts = [];
    }
}
