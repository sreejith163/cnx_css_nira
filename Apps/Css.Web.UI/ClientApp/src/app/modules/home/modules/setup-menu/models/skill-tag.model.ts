import { WeekDay } from '@angular/common';

export class SkillTag {
    id: number;
    refId: number;
    skillTagName: string;
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
    details: SkillTagOpenHours[];
    clientName: string;
    clientLobGroup: string;
    skillGroup: string;
}

export class SkillTagOpenHours {
    day: WeekDay;
    open: string;
    from: string;
    to: string;
}
