import { Injectable } from '@angular/core';
import { ClientLOBGroupList } from '../models/client-lob-group-list.model';
import { ClientNameList } from '../models/client-name-list.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { ClientLOBName } from '../../scheduling-menu/models/client-lob-name.model';

@Injectable()
export class ClientLobGroupDropdownService {

  clientNames: ClientNameList[] = [];
  clientLOB: ClientLOBGroupList[] = [];
  clientLOBNames: ClientLOBName[] = [];

  constructor() {
    this.createClientLOBGroupList();
    this.createClientNameList();
    this.createClientLOBNameList();
   }

  createClientNameList() {
    for (let i = 1; i <= 9; i++) {
      const client = new ClientNameList();
      client.id = i;
      client.clientName = 'Client ' + i;

      this.clientNames.push(client);
    }
  }

  getClientNameList() {
    return this.clientNames;
  }

  createClientLOBGroupList() {
    for (let i = 1; i <= 9; i++) {
      const clientLOB = new ClientLOBGroupList();
      clientLOB.id = i;
      clientLOB.clientLOBGroup = 'ClientLOBGroup ' + i;

      this.clientLOB.push(clientLOB);
    }
  }

  getClientLOBGroupList() {
    return this.clientLOB;
  }

  createClientLOBNameList() {
    for (let i = 1; i <= 9; i++) {
      const clientLOB = new ClientLOBName();
      clientLOB.id = i;
      clientLOB.clientLOBName = 'ClientLOBGroup ' + i;

      this.clientLOBNames.push(clientLOB);
    }
  }

  getClientLOBNameList() {
    return this.clientLOBNames;
  }

  getTablePageSizeList() {
    const tablePageSize: PaginationSize[] = [
      {
        count: 5,
        sizeText: '5/Page'
      },
      {
        count: 10,
        sizeText: '10/Page'
      },
      {
        count: 15,
        sizeText: '15/Page'
      }
    ];

    return tablePageSize;

  }

}
