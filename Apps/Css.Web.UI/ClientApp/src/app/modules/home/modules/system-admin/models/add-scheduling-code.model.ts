import { SchedulingCodeAttributes } from './scheduling-code-attributes.model';

export class AddSchedulingCode extends SchedulingCodeAttributes {
    refId?: number;
    createdBy: string;
}
