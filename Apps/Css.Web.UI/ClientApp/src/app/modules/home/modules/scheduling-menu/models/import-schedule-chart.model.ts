import { ImportScheduleData } from './import-schedule-data.model';

export class ImportShceduleChart {
    importAgentScheduleCharts: ImportScheduleData[];
    modifiedBy: string;

    constructor() {
        this.importAgentScheduleCharts = [];
    }
}
