export class QueryStringParameters {
    pageSize: number;
    pageNumber: number;
    searchKeyword: string;
    skipPageSize: boolean;
    sortBy: 'asc' | 'desc';
    orderBy: string;
    fields: string;
}
