import { SchedulingCode } from './scheduling-code.model';

export class AddSchedulingCode extends SchedulingCode{
    iconId: number;
    codeTypes: number[];
    createdBy: string;
}
