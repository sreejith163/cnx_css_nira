import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { SchedulingIcon } from '../models/scheduling-icon.model';

@Injectable()
export class SchedulingCodeIconsService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.schedulingService;
  }
  getSchedulingIcons() {
    const url = `${this.baseURL}/schedulingCodeIcons`;

    return this.http.get<SchedulingIcon>(url)
      .pipe(catchError(this.handleError));
  }
}
