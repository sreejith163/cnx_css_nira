import { Injectable } from '@angular/core';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { EmployeeId} from '../models/employee-id.model';
import { AgentSchedulingGroupName } from '../models/agent-scheduling-group-name.model';

@Injectable()
export class AgentAdminDropdownsService {

  agentSchedulingGroupNames: AgentSchedulingGroupName[] = [];
  employeeIdList: EmployeeId[] = [];

  constructor() {
    this.createAgentSchedulingGroupNamesList();
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

  createAgentSchedulingGroupNamesList() {
    for (let i = 1; i <= 9; i++) {
      const agentSchedulingGroup = new AgentSchedulingGroupName();
      agentSchedulingGroup.id = i;
      agentSchedulingGroup.agentSchedulingGroupName = 'Agent Scheduling Group ' + i;

      this.agentSchedulingGroupNames.push(agentSchedulingGroup);
    }
  }

  getAgentSchedulingGroupNames() {
    return this.agentSchedulingGroupNames;
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
