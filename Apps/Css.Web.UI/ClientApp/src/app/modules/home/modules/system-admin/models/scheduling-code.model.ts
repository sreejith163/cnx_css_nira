import { SchedulingCodeType } from './scheduling-code-type.model';

export class SchedulingCode {
    id: number;
    refId: number;
    description: string;
    priorityNo: number;
    types: SchedulingCodeType[];
    scheduleIcon: string;
}
