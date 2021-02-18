import { TimeOffsBase } from './time-offs-base.model';

export class TimeOffResponse extends TimeOffsBase {
    modifiedDate: Date;
    modifiedBy: string;
    createdDate: Date;
    createdBy: string;
}
