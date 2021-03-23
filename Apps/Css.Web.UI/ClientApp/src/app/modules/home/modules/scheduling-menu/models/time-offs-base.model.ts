import { WeekDay } from '@angular/common';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { DeSelectedTime } from '../enums/deSelected-time.enum';
import { AgentAccess } from './agent-access.model';
import { DeselectedTime } from './deselected-time.model';
import { FullWeeks } from './full-weeks.model';

export class TimeOffsBase {
    id: number;
    description: string;
    schedulingCodeId: number;
    startDate: Date;
    endDate: Date;
    allowDayRequest: number[];
    fTEDayLength: string;
    firstDayOfWeek: WeekDay;
    viewAllotments: boolean;
    viewWaitLists: boolean;
    timeOffs: boolean;
    addNotes: boolean;
    showPastDays: boolean;
    forceOffDaysBeforeWeek: number;
    forceOffDaysAfterWeek: number;
    allowFullWeekRequest: number;
    deSelectedTime: DeSelectedTime;
    deselectSavedDays: boolean;

}
