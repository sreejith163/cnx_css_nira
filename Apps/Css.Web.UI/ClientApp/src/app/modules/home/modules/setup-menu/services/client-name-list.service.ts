import { Injectable } from '@angular/core';
import { ClientDetails } from '../models/client-details.model';

@Injectable()
export class ClientNameListService {
  clientListSize = 9;
  clients: ClientDetails[] = [];

  constructor() {
    this.createClientList();
  }

  createClientList() {
    for (let i = 1; i <= this.clientListSize; i++) {
      const client = new ClientDetails();
      client.id = i;
      client.refId = i;
      client.clientName = 'Name ' + i;
      client.createdDate = '2020-09-1' + i;
      client.createdBy = 'User ' + i;
      client.modifiedDate = '2020-09-1' + i;
      client.modifiedBy = 'User ' + i;

      this.clients.push(client);
    }
  }

  addClientDetails(client: ClientDetails) {
    client.id = this.clients.length + 1;
    client.refId = this.clients.length + 1;
    client.createdDate = String(new Date());
    client.createdBy = 'User ' + (this.clients.length + 1);
    this.clients.push(client);
  }

  updateClientDetials(client: ClientDetails) {
    client.modifiedDate = String(new Date());
    client.modifiedBy = 'User ' + (this.clients.length + 1);
    const clientIndex = this.clients.findIndex(x => x.id === client.id);
    if (clientIndex !== -1) {
      this.clients[clientIndex] = client;
    }
  }

  deleteClient(id: number) {
    const clientIndex = this.clients.findIndex(x => x.id === id);
    if (clientIndex !== -1) {
      this.clients.splice(clientIndex, 1);
    }
  }

  getClientDetails() {
    return this.clients;
  }

}
