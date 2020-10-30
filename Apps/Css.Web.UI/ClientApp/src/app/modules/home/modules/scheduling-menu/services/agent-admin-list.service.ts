import { Injectable } from '@angular/core';
import { AgentAdmin } from '../models/agent-admin.model';

@Injectable()
export class AgentAdminListService {

  agentListSize: number;
  agents: AgentAdmin[] = [];

  constructor() {
    this.createAgentAdmins();
  }

  createAgentAdmins() {
    for (let i = 1; i <= 9; i++) {
      const agent = new AgentAdmin();
      agent.id = i;
      agent.employeeId = i;
      agent.sso = 'test.user' + i + '@gmail.com';
      agent.firstName = 'Sample User ' + i;
      agent.lastName = 'Name ' + i;
      agent.hireDate = '2020-09-1' + i;
      agent.agentSchedulingGroupName = 'Agent Scheduling Group ' + i;
      agent.createdDate = '2020-09-1' + i;
      agent.createdBy = 'User ' + i;
      agent.modifiedDate = '2020-09-1' + i;
      agent.modifiedBy = 'User ' + i;

      this.agents.push(agent);
    }
  }

  addAgentAdmin(agent: AgentAdmin) {
    agent.id = this.agents.length + 1;
    agent.createdDate = String(new Date());
    agent.createdBy = 'User ' + (this.agents.length + 1);
    this.agents.push(agent);
  }

  updateAgentAdmin(agent: AgentAdmin) {
    agent.modifiedDate = String(new Date());
    agent.modifiedBy = 'User ' + (this.agents.length + 1);
    const agentIndex = this.agents.findIndex(x => x.id === agent.id);
    if (agentIndex !== -1) {
      this.agents[agentIndex] = agent;
    }
  }

  deleteAgentAdmin(employeeId: number) {
    const agentIndex = this.agents.findIndex(x => x.employeeId === employeeId);
    if (agentIndex !== -1) {
      this.agents.splice(agentIndex, 1);
    }
  }

  getAgentAdmins() {
    return this.agents;
  }

}
