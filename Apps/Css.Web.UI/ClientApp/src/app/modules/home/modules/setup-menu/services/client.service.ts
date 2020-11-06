import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddClient } from '../models/add-client.model';
import { ClientDetails } from '../models/client-details.model';
import { ClientNameQueryParameters } from '../models/client-name-query-parameters.model';
import { UpdateClient } from '../models/update-client.model';

@Injectable()
export class ClientService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.schedulingService;
  }

  getClients(clientnameQueryParams: ClientNameQueryParameters) {
    const url = `${this.baseURL}/clients`;

    return this.http.get<ClientDetails>(url,
      { params: this.convertToHttpParam(clientnameQueryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getClient(clientId: string) {
    const url = `${this.baseURL}/clients/${clientId}`;

    return this.http.get<ClientDetails>(url)
      .pipe(catchError(this.handleError));
  }

  addClient(client: AddClient) {
    const url = `${this.baseURL}/clients`;

    return this.http.post<ApiResponseModel>(url, client)
      .pipe(catchError(this.handleError));
  }

  updateClient(clientId: number, client: UpdateClient) {
    const url = `${this.baseURL}/clients/${clientId}`;

    return this.http.put<ApiResponseModel>(url, client)
      .pipe(catchError(this.handleError));
  }

  deleteClient(clientId: number) {
    const url = `${this.baseURL}/clients/${clientId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
