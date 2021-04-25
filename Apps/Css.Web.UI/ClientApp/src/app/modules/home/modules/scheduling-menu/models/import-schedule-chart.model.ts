import { ImportScheduleData } from './import-schedule-data.model';

export class ImportScheduleChart {
    importAgentScheduleCharts: ImportScheduleData[];
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;

    constructor() {
        this.importAgentScheduleCharts = [];
    }
}

export class ImportScheduleGridData {

    employeeId: string;
    startDate: string;
    endDate: string;
    schedulingCodeId: number;
    startTime: string;
    endTime: string;
}


export class SchedulingGridExcelScheduleData{
    EmployeeId: string;
    StartDate: string;  
    EndDate: string; 
    startTime: string; 
    endTime: string; 
    ActivityCode: string;

}

export class ShedulingGridImportModel{
    agentScheduleImportData: ImportScheduleGridData[] = [];
    activityOrigin: number;
    modifiedBy: string;
}