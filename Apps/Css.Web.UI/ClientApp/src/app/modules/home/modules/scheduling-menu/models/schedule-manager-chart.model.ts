export class ScheduleManagerChart {
    startDateTime: string;
    endDateTime: string;
    schedulingCodeId: number;

    constructor(startTime: string, endTime: string, schedulingCodeId: number) {
        this.startDateTime = startTime;
        this.endDateTime = endTime;
        this.schedulingCodeId = schedulingCodeId;
    }
}

export class ScheduleManagerAgentChartResponse {
    id: string;
    employeeId: number;
    firstName: string;
    lastName: string;
    agentSchedulingGroupId: number;
    date: Date;
    charts: ScheduleManagerChart[];

    constructor() {
        this.charts = [];
    }
}

export class ScheduleManagerChartDisplay {
    time: string;
    date: string;
    startDateTime: string;
    endDateTime: string;
    schedulingCodeId: number;
    
    constructor(startTime: string, endTime: string, schedulingCodeId: number) {
        this.startDateTime = startTime;
        this.endDateTime = endTime;
        this.schedulingCodeId = schedulingCodeId;
    }
}

export class ScheduleManagerGridChartDisplay {
    id: string;
    employeeId: number;
    firstName: string;
    lastName: string;
    agentSchedulingGroupId: number;
    date: Date;
    charts: ScheduleManagerChartDisplay[];

    constructor() {
        this.charts = [];
    }
}


export class ScheduleManagerChartUpdate {
    scheduleManagers: ScheduleManagerAgentData[];
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;
    isImport: boolean;

    constructor() {
        this.scheduleManagers = [];
    }
}

export class ScheduleManagerAgentData {
    employeeId: number;
    agentScheduleManagerCharts: ScheduleManagerAgentCharts[];

    constructor() {
        this.agentScheduleManagerCharts = [];
    }
}


export class ScheduleManagerAgentCharts {
    date: Date;
    charts: ScheduleManagerChart[];

    constructor() {
        this.charts = [];
    }
}

export class OpenTimeData{
    time: string;
    startDateTime: string;
    endDateTime: string;
    date: string;
    dateHeader: string;
  }
  