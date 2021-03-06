import { Permission } from './permission.model';
import { UserRole } from './user-role.model';

export class PermissionDetails extends Permission {
    roles: UserRole[];
    createdBy: string;
    createdDate: Date;
    modifiedBy: string;
    modifiedDate: Date;
}
