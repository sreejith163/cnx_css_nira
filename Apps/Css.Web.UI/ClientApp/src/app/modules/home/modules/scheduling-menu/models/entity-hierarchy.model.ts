

export class EntityHierarchyModel {
    agentSchedulingGroups: EntityAgentSchedulingGroupDetails[] = [];
    client: EntityClientDetails;
}

export class EntityAgentSchedulingGroupDetails {
    id: number;
    refId?: number;
    name: string;
    ClientId: number;
    ClientRefId?: number;
    clientName: string;
    ClientLOBId: number;
    ClientLOBRefId?: number;
    clientLOBName: string;
    SkillGroupId: number;
    SkillGroupRefId?: number;
    skillGroupName: string;
    SkillTagId: number;
    SkillTagRefId?: number;
    skillTagName: string;
}

export class EntityClientDetails {
    clientLOBs: EntityClientLOBDetails[] = [];
    id: number;
    refId?: number;
    name: string;
}

export class EntityClientLOBDetails {
    id: number;
    name: string;
    refId?: number;
    skillGroups: EntitySkillGroupDetails[] = [];
}

export class EntitySkillGroupDetails {
    id: number;
    name: string;
    refId?: number;
    skillTags: EntitySkillTagDetails[] = [];
}

export class EntitySkillTagDetails {
    id: number;
    name: string;
    refId?: number;
}
