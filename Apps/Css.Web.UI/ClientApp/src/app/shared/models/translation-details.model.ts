import { TranslationBase } from './translation-base.model';

export class TranslationDetails extends TranslationBase {
    languageName: string;
    menuName: string;
    variableName: string;
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
}
