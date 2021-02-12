import { SchedulingCodeAttributes } from './scheduling-code-attributes.model';

export class UpdateSchedulingCode extends SchedulingCodeAttributes {
    refId?: number;
    modifiedBy: string;
}
