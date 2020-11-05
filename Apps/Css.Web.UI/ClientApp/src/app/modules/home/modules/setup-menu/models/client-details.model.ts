import { ClientBaseModel } from './client-base.model';

export class ClientDetails extends ClientBaseModel{
    refId: number;
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
}
