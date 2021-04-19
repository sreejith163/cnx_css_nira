import { AgentCategoryValueDetailsResponse } from "./agent-category-value-details-response.model";


export class AgentCategoryValueResponse {
    employeeId: string;
    firstName: string;
    lastName: string;
    agentCategoryValues: AgentCategoryValueDetailsResponse[]; 
}
