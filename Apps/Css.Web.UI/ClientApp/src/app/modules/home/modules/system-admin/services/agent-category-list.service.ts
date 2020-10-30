import { Injectable } from '@angular/core';
import { DataType } from '../enum/data-type.enum';
import { AgentCategory } from '../models/agent-category.model';
import { Range } from '../models/range.model';

@Injectable()
export class AgentCategoryListService {

  agentCategoryListSize: number;
  agentCategories: AgentCategory[] = [];

  constructor() {
    this.createAgentCategories();
  }

  createAgentCategories() {
    for (let i = 1; i <= 9; i++) {
      const agent = new AgentCategory();
      agent.employeeId = i;
      agent.descriptionOrName = 'Sample Description text ' + i;
      agent.dataType = i % 2 === 0 ? DataType.Numeric : DataType.Date;
      if (i % 2 === 0) {
        agent.range = new Range(100, 200);
      } else {
        agent.range = new Range('2020-09-23', '2020-10-29');
      }
      agent.createdDate = '2020-09-1' + i;
      agent.createdBy = 'User ' + i;
      agent.modifiedDate = '2020-09-1' + i;
      agent.modifiedBy = 'User ' + i;
      this.agentCategories.push(agent);
    }
  }

  addAgentCategory(agent: AgentCategory) {
    agent.employeeId = this.agentCategories.length + 1;
    agent.createdDate = String(new Date());
    agent.createdBy = 'User ' + (this.agentCategories.length + 1);
    this.agentCategories.push(agent);
  }

  updateAgentCategrory(agent: AgentCategory) {
    agent.modifiedDate = String(new Date());
    agent.modifiedBy = 'User ' + (this.agentCategories.length + 1);
    const agentIndex = this.agentCategories.findIndex(x => x.employeeId === agent.employeeId);
    if (agentIndex !== -1) {
      this.agentCategories[agentIndex] = agent;
    }
  }

  deletegentcategory(employeeId: number) {
    const agentIndex = this.agentCategories.findIndex(x => x.employeeId === employeeId);
    if (agentIndex !== -1) {
      this.agentCategories.splice(agentIndex, 1);
    }
  }

  getAgentCategories() {
    return this.agentCategories;
  }

}
