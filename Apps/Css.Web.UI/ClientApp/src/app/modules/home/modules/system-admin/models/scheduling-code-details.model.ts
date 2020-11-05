import { SchedulingCodeType } from './scheduling-code-type.model';
import { SchedulingCode } from './scheduling-code.model';
import { SchedulingIcon } from './scheduling-icon.model';

export class SchedulingCodeDetails extends SchedulingCode {
    icon: SchedulingIcon;
    schedulingTypeCode: SchedulingCodeType[];
}
