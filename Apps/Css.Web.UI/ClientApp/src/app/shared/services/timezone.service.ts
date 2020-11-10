import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { TimeZone } from '../models/time-zone.model';

@Injectable()
export class TimezoneService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.schedulingService;
  }

  getTimeZones() {
    const url = `${this.baseURL}/TimeZones`;

    return this.http.get<TimeZone>(url)
      .pipe(catchError(this.handleError));
  }
}
