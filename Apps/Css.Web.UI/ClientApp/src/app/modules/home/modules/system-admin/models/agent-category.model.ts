import { DataType } from '../enum/data-type.enum';
import { Range } from './range.model';

export class AgentCategory {
    employeeId: string;
    descriptionOrName: string;
    dataType: DataType;
    range: Range;
    createdDate: string;
    createdBy: string;
    modifiedBy: string;
    modifiedDate: string;
    hireDate?: string;
}
