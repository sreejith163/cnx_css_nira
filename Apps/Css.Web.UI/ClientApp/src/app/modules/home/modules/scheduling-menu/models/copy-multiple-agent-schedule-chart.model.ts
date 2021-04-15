import { CopyChartBase } from './copy-chart-base.model';
import { ScheduleDateRangeBase } from './schedule-date-range-base.model';

export class CopyMultipleAgentScheduleChart {
    dateFrom: Date;
    dateTo: Date;
    employeeIds: number[];
    selectedDateRanges: ScheduleDateRangeBase[];
    agentSchedulingGroupId: number;
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;
}
