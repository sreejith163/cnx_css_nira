import { TranslationBase } from './translation-base.model';

export class TranslationDetails extends TranslationBase {
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
}
