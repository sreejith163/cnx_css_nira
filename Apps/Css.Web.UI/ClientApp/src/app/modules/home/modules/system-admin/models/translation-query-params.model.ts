import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class TranslationQueryParams extends QueryStringParameters {
    languageId: number;
    menuId: number;
    variableId: number;
}
