import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { environment } from 'src/environments/environment';
import { AgentMyScheduleResponse } from '../models/agent-myschedule-response.model';
import { SchedulingCodeQueryParams } from '../models/scheduling-code-query-params.model';

@Injectable()
export class AgentMyScheduleService extends HttpBaseService {
  private baseURL = '';

  constructor(
    private http: HttpClient,
    private authService: AuthService) {
    super();
    this.baseURL = environment.services.gatewayService;
}

    getAgentMySchedule(employeeId, startDate, endDate, agentSchedulingGroupId?){
        const url = `${this.baseURL}/AgentScheduleManagers/${employeeId}/myschedule?StartDate=${startDate}&EndDate=${endDate}&AgentSchedulingGroupId=${agentSchedulingGroupId ?? 0}`;

        return this.http.get<AgentMyScheduleResponse>(url)
            .pipe(catchError(this.handleError));
    }

    getSchedulingIcons(schedulingCodeId) {
        const url = `${this.baseURL}/schedulingCodes/${schedulingCodeId}`;
    
        return this.http.get<KeyValue>(url)
          .pipe(catchError(this.handleError));
    }

    getSchedulingCodes(schedulingCodeQueryParams: SchedulingCodeQueryParams) {
        const url = `${this.baseURL}/schedulingCodes`;
    
        return this.http.get<any>(url, {
          params: this.convertToHttpParam(schedulingCodeQueryParams),
          observe: 'response'
        }).pipe(catchError(this.handleError));
      }


}
