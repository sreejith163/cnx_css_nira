import { DataType } from '../enum/data-type.enum';

export class AgentCategoryBase {
    id: number;
    name: string;
    dataTypeId: DataType;
    dataTypeLabel: string;
    dataTypeMinValue: string;
    dataTypeMaxValue: string;
}
