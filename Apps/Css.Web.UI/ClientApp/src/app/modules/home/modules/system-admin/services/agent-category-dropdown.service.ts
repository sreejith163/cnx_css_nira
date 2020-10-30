import { Injectable } from '@angular/core';

import { PaginationSize } from '../../../../../shared/models/pagination-size.model';
import { DataType } from '../enum/data-type.enum';

@Injectable()
export class AgentCategoryDropdownService {

  dataTypes = [];

  constructor() {
    this.createAgentCategoryDataTypes();
   }

  createAgentCategoryDataTypes() {
    this.dataTypes = Object.keys(DataType).filter(f => !isNaN(Number(f)));
  }

  getAgentCategoryDataTypes() {
    return this.dataTypes;
  }

  getTablePageSizeList() {
    const tablePageSize: PaginationSize[] = [
      {
        count: 5,
        sizeText: '5/Page'
      },
      {
        count: 10,
        sizeText: '10/Page'
      },
      {
        count: 15,
        sizeText: '15/Page'
      }
    ];

    return tablePageSize;

  }

}

