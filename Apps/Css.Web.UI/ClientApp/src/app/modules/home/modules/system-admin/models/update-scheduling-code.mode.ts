import { SchedulingCode } from './scheduling-code.model';

export class UpdateSchedulingCode extends SchedulingCode{
    iconId: number;
    codeTypes: number[];
    modifiedBy: string;
}
