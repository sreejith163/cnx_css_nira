import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddAgentSchedulingGroup } from '../../modules/home/modules/setup-menu/models/add-agent-scheduling-group.model';
import { AgentSchedulingGroupDetails } from '../../modules/home/modules/setup-menu/models/agent-scheduling-group-details.model';
import { AgentSchedulingGroupQueryParams } from '../../modules/home/modules/setup-menu/models/agent-scheduling-group-query-params.model';
import { AgentSchedulingGroupResponse } from '../../modules/home/modules/setup-menu/models/agent-scheduling-group-response.model';
import { UpdateAgentSchedulingGroup } from '../../modules/home/modules/setup-menu/models/update-agent-scheduling-group.model';

@Injectable()
export class AgentSchedulingGroupService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getAgentSchedulingGroups(agentSchedulingGroupsQueryParams: AgentSchedulingGroupQueryParams) {
    const url = `${this.baseURL}/agentSchedulingGroups`;

    return this.http.get<AgentSchedulingGroupDetails>(url, {
      params: this.convertToHttpParam(agentSchedulingGroupsQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  getAgentSchedulingGroup(agentSchedulingGroupId: number) {
    const url = `${this.baseURL}/agentSchedulingGroups/${agentSchedulingGroupId}`;

    return this.http.get<AgentSchedulingGroupResponse>(url)
      .pipe(catchError(this.handleError));
  }

  addAgentSchedulingGroup(agentSchedulingGroup: AddAgentSchedulingGroup) {
    const url = `${this.baseURL}/agentSchedulingGroups`;

    return this.http.post<ApiResponseModel>(url, agentSchedulingGroup)
      .pipe(catchError(this.handleError));
  }

  updateAgentSchedulingGroup(agentSchedulingGroupId: number, agentSchedulingGroup: UpdateAgentSchedulingGroup) {
    const url = `${this.baseURL}/agentSchedulingGroups/${agentSchedulingGroupId}`;

    return this.http.put<ApiResponseModel>(url, agentSchedulingGroup)
      .pipe(catchError(this.handleError));
  }

  deleteAgentSchedulingGroup(agentSchedulingGroupId: number) {
    const url = `${this.baseURL}/agentSchedulingGroups/${agentSchedulingGroupId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
