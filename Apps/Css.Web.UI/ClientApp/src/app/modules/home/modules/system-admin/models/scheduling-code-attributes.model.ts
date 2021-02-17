import { SchedulingCodeType } from './scheduling-code-type.model';

export class SchedulingCodeAttributes {
     description: string;
     timeOffCode: boolean;
     priorityNumber: number;
     schedulingTypeCode: SchedulingCodeType[];
     iconId: number;
}
