import { WeekDay } from '@angular/common';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { AgentAccess } from './agent-access.model';
import { DeselectedTime } from './deselected-time.model';
import { FullWeeks } from './full-weeks.model';

export class TimeOffsBase {
    id: number;
    description: string;
    timeOffCode: number;
    startDate: Date;
    endDate: Date;
    allowDayRequestOn: number[];
    FTEDayLength: string;
    firstDayOfWeek: WeekDay;
    agentAccess: AgentAccess;
    fullWeeks: FullWeeks;
    deselectedTime: DeselectedTime;
}
