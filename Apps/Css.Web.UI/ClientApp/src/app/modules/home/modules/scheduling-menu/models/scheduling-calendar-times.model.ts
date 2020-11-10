export class SchedulingCalendarTime {
    meridiem: 'am' | 'pm';
    from: any;
    to: any;
    icon: string;

    constructor(meridiem: 'am' | 'pm', from: any, to: any, icon: string) {
        this.meridiem = meridiem;
        this.from = from;
        this.to = to;
        this.icon = icon;
    }
}
