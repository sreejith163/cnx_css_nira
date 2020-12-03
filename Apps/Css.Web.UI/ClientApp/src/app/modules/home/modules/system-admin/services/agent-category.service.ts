import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddAgentCategory } from '../models/add-agent-category.model';
import { AgentCategoryDetails } from '../models/agent-category-details.model';
import { AgentCategoryQueryParams } from '../models/agent-category-query-params.model';
import { UpdateAgentCategory } from '../models/update-agent-category.model';

@Injectable()
export class AgentCategoryService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.adminService;
  }

  getAgentcategories(agentCategoryQueryParams: AgentCategoryQueryParams) {
    const url = `${this.baseURL}/AgentCategories`;

    return this.http.get<AgentCategoryDetails>(url,
      { params: this.convertToHttpParam(agentCategoryQueryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getAgentCategory(agentCategoryId: string) {
    const url = `${this.baseURL}/AgentCategories/${agentCategoryId}`;

    return this.http.get<AgentCategoryDetails>(url)
      .pipe(catchError(this.handleError));
  }

  addAgentcategory(agentCategory: AddAgentCategory) {
    const url = `${this.baseURL}/AgentCategories`;

    return this.http.post<ApiResponseModel>(url, agentCategory)
      .pipe(catchError(this.handleError));
  }

  updateAgentCategory(agentCategoryId: number, agentCategory: UpdateAgentCategory) {
    const url = `${this.baseURL}/AgentCategories/${agentCategoryId}`;

    return this.http.put<ApiResponseModel>(url, agentCategory)
      .pipe(catchError(this.handleError));
  }

  deleteAgentCategory(agentCategoryId: number) {
    const url = `${this.baseURL}/AgentCategories/${agentCategoryId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
