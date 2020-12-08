
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddSkillGroup } from '../models/add-skill-group.model';
import { ClientLOBGroupDetails as skillGroupDetails } from '../models/client-lob-group-details.model';
import { SkillGroupDetails } from '../models/skill-group-details.model';
import { SkillGroupQueryParameters } from '../models/skill-group-query-parameters.model';
import { UpdateSkillGroup } from '../models/update-skill-group.model';

@Injectable()
export class SkillGroupService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getSkillGroups(skillGroupQueryParameters: SkillGroupQueryParameters) {
    const url = `${this.baseURL}/SkillGroups`;

    return this.http.get<SkillGroupDetails>(url, {
      params: this.convertToHttpParam(skillGroupQueryParameters),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  getSkillGroup(skillGroupId: number) {
    const url = `${this.baseURL}/SkillGroups/${skillGroupId}`;

    return this.http.get<skillGroupDetails>(url)
      .pipe(catchError(this.handleError));
  }

  addSkillGroup(skillGroup: AddSkillGroup) {
    const url = `${this.baseURL}/SkillGroups`;

    return this.http.post<ApiResponseModel>(url, skillGroup)
      .pipe(catchError(this.handleError));
  }

  updateSkillGroup(skillGroupId: number, skillGroup: UpdateSkillGroup) {
    const url = `${this.baseURL}/SkillGroups/${skillGroupId}`;

    return this.http.put<ApiResponseModel>(url, skillGroup)
      .pipe(catchError(this.handleError));
  }

  deleteSkillGroup(skillGroupId: number) {
    const url = `${this.baseURL}/SkillGroups/${skillGroupId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}

