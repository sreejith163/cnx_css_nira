import { WeekDay } from '@angular/common';
import { DeSelectedTime } from '../enums/deSelected-time.enum';

export class TimeOffsBase {
    id: string;
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
