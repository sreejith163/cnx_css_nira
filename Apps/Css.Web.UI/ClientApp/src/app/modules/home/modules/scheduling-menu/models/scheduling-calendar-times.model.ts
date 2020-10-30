export class SchedulingCalendarTime {
    meridiem: 'am' | 'pm';
    time: any;
    icon: string;

    constructor(meridiem: 'am' | 'pm', time: any, icon: string) {
        this.meridiem = meridiem;
        this.time = time;
        this.icon = icon;
    }
}
