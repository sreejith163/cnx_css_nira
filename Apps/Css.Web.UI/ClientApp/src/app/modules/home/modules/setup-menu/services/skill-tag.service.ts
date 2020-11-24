import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddSkillTag } from '../models/add-skill-tag.model';
import { SkillTagDetails } from '../models/skill-tag-details.model';
import { SkillTagQueryParams } from '../models/skill-tag-query-params.model';
import { SkillTagResponse } from '../models/skill-tag-response.model';
import { UpdateSkillTag } from '../models/update-skill-tag.model';

@Injectable()
export class SkillTagService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.setupService;
  }

  getSkillTags(skillTagsQueryParams: SkillTagQueryParams) {
    const url = `${this.baseURL}/skillTags`;

    return this.http.get<SkillTagDetails>(url,
      { params: this.convertToHttpParam(skillTagsQueryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getSkillTag(skillTagId: number) {
    const url = `${this.baseURL}/skillTags/${skillTagId}`;

    return this.http.get<SkillTagResponse>(url)
      .pipe(catchError(this.handleError));
  }

  addSkillTag(skillTag: AddSkillTag) {
    const url = `${this.baseURL}/skillTags`;

    return this.http.post<ApiResponseModel>(url, skillTag)
      .pipe(catchError(this.handleError));
  }

  updateSkillTag(skillTagId: number, skillTag: UpdateSkillTag) {
    const url = `${this.baseURL}/skillTags/${skillTagId}`;

    return this.http.put<ApiResponseModel>(url, skillTag)
      .pipe(catchError(this.handleError));
  }

  deleteSkillTag(skillTagId: number) {
    const url = `${this.baseURL}/skillTags/${skillTagId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
