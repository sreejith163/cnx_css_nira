import { Injectable } from '@angular/core';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { EmployeeId } from '../models/employee-id.model';
import { ClientLOBGroup } from '../models/client-lob-group.model';
import { ClientNameList } from '../models/client-name-list.model';
import { SkillGroup } from '../models/skill-group.model';
import { SkillTag } from '../models/skill-tag.model';

@Injectable()
export class AgentAdminDropdownsService {

  clientNames: ClientNameList[] = [];
  clientLOB: ClientLOBGroup[] = [];
  skillGroup: SkillGroup[] = [];
  skillTag: SkillTag[] = [];
  employeeIdList: EmployeeId[] = [];

  constructor() {
    this.createClientLOBGroupList();
    this.createClientNameList();
    this.createSkillGroup();
    this.createSkillTag();
    this.createEmployeeIdList();
  }

  createEmployeeIdList() {
    for (let i = 1; i <= 10; i++) {
      const id = new EmployeeId();
      id.id = i;
      this.employeeIdList.push(id);
    }
  }

  getEmployeeIdList() {
    return this.employeeIdList;
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
      const clientLOB = new ClientLOBGroup();
      clientLOB.id = i;
      clientLOB.clientLOBGroup = 'ClientLOBGroup ' + i;

      this.clientLOB.push(clientLOB);
    }
  }

  getClientLOBGroupList() {
    return this.clientLOB;
  }

  createSkillGroup() {
    for (let i = 1; i <= 9; i++) {
      const skillGroup = new SkillGroup();
      skillGroup.id = i;
      skillGroup.skillGroup = 'Skill Group ' + i;

      this.skillGroup.push(skillGroup);
    }
  }

  getSkillGroup() {
    return this.skillGroup;
  }

  createSkillTag() {
    for (let i = 1; i <= 9; i++) {
      const skillTag = new SkillTag();
      skillTag.id = i;
      skillTag.skillTag = 'Skill Tag ' + i;

      this.skillTag.push(skillTag);
    }
  }

  getSkillTag() {
    return this.skillTag;
  }

  getTablePageSizeList() {
    const tablePageSize: PaginationSize[] = [
      {
        count: 5,
        text: '5/Page'
      },
      {
        count: 10,
        text: '10/Page'
      },
      {
        count: 15,
        text: '15/Page'
      }
    ];

    return tablePageSize;
  }
}
