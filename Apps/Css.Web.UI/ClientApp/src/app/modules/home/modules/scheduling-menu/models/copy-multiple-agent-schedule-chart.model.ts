import { CopyChartBase } from './copy-chart-base.model';
import { CopyScheduleDateRangeModel } from './copy-schedule-date-range.model';
import { ScheduleDateRangeBase } from './schedule-date-range-base.model';

export class CopyMultipleAgentScheduleChart {
    dateFrom: string;
    dateTo: string;
    employeeIds: number[];
    selectedDateRanges: CopyScheduleDateRangeModel[];
    agentSchedulingGroupId: number;
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;
}
