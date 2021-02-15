

export class EntityHierarchyModel {
    agentSchedulingGroups: EntityAgentSchedulingGroupDetails [] = [];
    client: EntityClientDetails;
}

export class EntityAgentSchedulingGroupDetails {
    clientLOBName: string;
    clientName: string;
    id: number;
    name: string;
    skillGroupName: string;
    skillTagName: string;
}

export class EntityClientDetails {
    clientLOBs: EntityClientLOBDetails [] = [];
    id: "5"
    name: "IEX"
}

export class EntityClientLOBDetails {
    id: number;
    name: string;
    skillGroups: EntitySkillGroupDetails [] = [];
}

export class EntitySkillGroupDetails {
    id: number;
    name: string;
    skillTags: EntitySkillTagDetails [] = [];
}

export class EntitySkillTagDetails{
    id: number;
    name: string;
}