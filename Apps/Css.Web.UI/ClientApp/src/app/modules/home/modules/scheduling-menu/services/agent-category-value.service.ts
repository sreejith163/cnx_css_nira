import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddAgentAdmin } from '../models/add-agent-admin.model';
import { AgentAdminBase } from '../models/agent-admin-base.model';
import { AgentAdminDetails } from '../models/agent-admin-details.model';
import { AgentAdminQueryParameter } from '../models/agent-admin-query-parameter.model';
import { AgentCategoryValueQueryParameter } from '../models/agent-category-value-query-parameter.model';
import { AgentCategoryValueResponse } from '../models/agent-category-value-response.model';
import { AgentInfo } from '../models/agent-info.model';
import { MoveAgentAdminParameters } from '../models/move-agent-params.model';
import { UpdateAgentAdmin } from '../models/update-agent-admin.model';

@Injectable()
export class AgentCategoryValueService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) { 
    super();
    this.baseURL = environment.services.gatewayService;
  }

  agentCategoriesUpdated = new BehaviorSubject<number>(undefined);

  getAgentCategorieValues(agentCategoryValueQueryParams: AgentCategoryValueQueryParameter) {
    const url = `${this.baseURL}/agentCategoryValues`;

    return this.http.get<AgentCategoryValueResponse>(url, {
      params: this.convertToHttpParam(agentCategoryValueQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }
  importAgentCategoryValue(agentCategoryList : any){
    const url = `${this.baseURL}/agentCategoryValues/import`;
    return this.http.put<ApiResponseModel>(url, agentCategoryList)
      .pipe(catchError(this.handleError));
  }
 
}
