import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddAgentAdmin } from '../models/add-agent-admin.model';
import { AgentAdminDetails } from '../models/agent-admin-details.model';
import { AgentAdminQueryParameter } from '../models/agent-admin-query-parameter.model';
import { AgentAdminResponse } from '../models/agent-admin-response.model';
import { UpdateAgentAdmin } from '../models/update-agent-admin.model';

@Injectable()
export class AgentAdminService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getAgentAdmins(agentAdminsQueryParams: AgentAdminQueryParameter) {
    const url = `${this.baseURL}/agentadmins`;

    return this.http.get<AgentAdminDetails>(url, {
      params: this.convertToHttpParam(agentAdminsQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  getAgentAdmin(agentAdminId: number) {
    const url = `${this.baseURL}/agentadmins/${agentAdminId}`;

    return this.http.get<AgentAdminResponse>(url)
      .pipe(catchError(this.handleError));
  }

  addAgentAdmin(agentAdmin: AddAgentAdmin) {
    const url = `${this.baseURL}/agentadmins`;

    return this.http.post<ApiResponseModel>(url, agentAdmin)
      .pipe(catchError(this.handleError));
  }

  updateAgentAdmin(agentAdminId: number, agentAdmin: UpdateAgentAdmin) {
    const url = `${this.baseURL}/agentadmins/${agentAdminId}`;

    return this.http.put<ApiResponseModel>(url, agentAdmin)
      .pipe(catchError(this.handleError));
  }

  deleteAgentAdmin(agentAdminId: number) {
    const url = `${this.baseURL}/agentadmins/${agentAdminId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
