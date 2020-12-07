import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddClientLobGroup } from '../models/add-client-lob-group.model';
import { ClientLOBGroupDetails } from '../models/client-lob-group-details.model';
import { ClientLobGroupQueryParameters } from '../models/client-lob-group-query-parameters.model';
import { UpdateClientLobGroup } from '../models/update-client-lob-group.model';

@Injectable()
export class ClientLobGroupService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.setupService;
  }

  getClientLOBGroups(clientLobGroupQueryParameters: ClientLobGroupQueryParameters) {
    const url = `${this.baseURL}/ClientLOBGroups`;

    return this.http.get<ClientLOBGroupDetails>(url, {
      params: this.convertToHttpParam(clientLobGroupQueryParameters),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  getClientLOBGroup(clientLOBGroupId: string) {
    const url = `${this.baseURL}/ClientLOBGroups/${clientLOBGroupId}`;

    return this.http.get<ClientLOBGroupDetails>(url)
      .pipe(catchError(this.handleError));
  }

  addClientLOBGroup(clientLOBGroup: AddClientLobGroup) {
    const url = `${this.baseURL}/ClientLOBGroups`;

    return this.http.post<ApiResponseModel>(url, clientLOBGroup)
      .pipe(catchError(this.handleError));
  }

  updateClientLOBGroup(clientLOBGroupId: number, clientLOBGroup: UpdateClientLobGroup) {
    const url = `${this.baseURL}/ClientLOBGroups/${clientLOBGroupId}`;

    return this.http.put<ApiResponseModel>(url, clientLOBGroup)
      .pipe(catchError(this.handleError));
  }

  deleteClientLOBGroup(clientLOBGroupId: number) {
    const url = `${this.baseURL}/ClientLOBGroups/${clientLOBGroupId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
