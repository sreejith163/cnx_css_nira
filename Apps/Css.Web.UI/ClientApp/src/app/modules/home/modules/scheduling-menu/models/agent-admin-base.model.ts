export class AgentAdminBase {
    id: string;
    employeeId: string;
    sso: string;
    skillTagId: number;
    agentSchedulingGroupId: number;
    clientId: number;
    clientLobGroupId: number;
    skillGroupId: number;
    firstName: string;
    lastName: string;
    hireDate: Date;
    supervisorId: string;
    supervisorName: string;
    supervisorSso: string;
    pto: {
        earned: string;
        credited: string;
        cofromlastyear: string;
        cofornextyear: string;
        planned: string;
        taken: string;
        debited: string;
        remaining: string;
    };
}
