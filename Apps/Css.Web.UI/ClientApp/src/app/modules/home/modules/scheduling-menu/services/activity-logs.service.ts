import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { ActivityLogsQueryParams } from '../models/activity-logs-query-params.model';
import { ActivityLogsBase } from '../models/activity-logs-base.model';
import { ActivityLogsResponse } from '../models/activity-logs-response.model';
import { of } from 'rxjs';
import { AgentScheduleManagerChart } from '../models/agent-schedule-manager-chart.model';

@Injectable()
export class ActivityLogsService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getActivityLogs(queryParams: ActivityLogsQueryParams) {
    const response = Array<ActivityLogsResponse>();
    const data = new ActivityLogsResponse();
    data.id = '1';
    data.employeeId = 1;
    data.executedBy = 'Sree';
    data.day = 0;
    data.origin = 'Css';
    data.status = 'Updated';
    data.timeStamp = new Date('2021-01-03');
    data.agentScheduleManagerCharts = [];
    const dataChart = new AgentScheduleManagerChart();
    dataChart.date = new Date('2021-01-03');
    dataChart.charts = [{
      schedulingCodeId: 5,
      startTime: '08:00 am',
      endTime: '10:00 am'
    }];
    data.agentScheduleManagerCharts.push(dataChart);
    response.push(data);

    return of(response);
  //   const url = `${this.baseURL}/activityLogs`;

  //   return this.http.get<ActivityLogsBase>(url, {
  //     params: this.convertToHttpParam(queryParams),
  //     observe: 'response'
  //   }).pipe(catchError(this.handleError));
  }
}
