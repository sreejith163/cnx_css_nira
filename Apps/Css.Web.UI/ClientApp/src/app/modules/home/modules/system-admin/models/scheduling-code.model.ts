import { KeyValue } from 'src/app/shared/models/key-value.model';

export class SchedulingCode {
    id: number;
    refId?: number;
    description: string;
    timeOffCode: boolean;
    priorityNumber: number;
    schedulingTypeCode: KeyValue[];
    icon: KeyValue;
    createdBy: string;
    createdDate: Date;
    modifiedBy: string;
    modifiedDate: Date;
}
