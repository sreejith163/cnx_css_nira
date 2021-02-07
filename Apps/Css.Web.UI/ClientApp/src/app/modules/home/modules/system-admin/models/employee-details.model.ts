import { Employee } from './employee.model';

export class EmployeeDetails extends Employee {
    userRoleId: number;
    roleIndex: number;
    createdDate: Date;
    modifiedDate: Date;
    createdBy: string;
    modifiedBy: string;
}
