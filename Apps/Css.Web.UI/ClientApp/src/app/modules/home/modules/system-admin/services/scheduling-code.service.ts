import { Injectable } from '@angular/core';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { SchedulingCodeType } from '../models/scheduling-code-type.model';
import { SchedulingCode } from '../models/scheduling-code.model';

@Injectable()
export class SchedulingCodeService {

  schedulingCodesListSize: number;
  schedulingCodes: SchedulingCode[] = [];

  constructor() {
    this.createSchedulingCodes();
  }

  getSchedulingCodes() {
    return this.schedulingCodes;
  }

  addSchedulingCode(schedulingCode: SchedulingCode) {
    schedulingCode.id = this.schedulingCodes.length + 1;
    schedulingCode.refId = this.schedulingCodes.length + 1;
    this.schedulingCodes.push(schedulingCode);
  }

  updateSchedulingCode(schedulingCode: SchedulingCode) {
    const schedulingCodeIndex = this.schedulingCodes.findIndex(x => x.id === schedulingCode.id);
    if (schedulingCodeIndex !== -1) {
      this.schedulingCodes[schedulingCodeIndex] = schedulingCode;
    }
  }

  deleteSchedulingCode(id: number) {
    const schedulingCodeIndex = this.schedulingCodes.findIndex(x => x.id === id);
    if (schedulingCodeIndex !== -1) {
      this.schedulingCodes.splice(schedulingCodeIndex, 1);
    }
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

  private createSchedulingCodes() {
    for (let i = 1; i <= 9; i++) {
      const schedulingCode = new SchedulingCode();
      schedulingCode.id = i;
      schedulingCode.refId = i;
      schedulingCode.description = i % 2 === 0 ? 'Break' : 'Lunch';
      schedulingCode.priorityNo = i;
      schedulingCode.types = new Array<SchedulingCodeType>();

      let type = new SchedulingCodeType();
      type.id = 3;
      type.value = 'Work Hours';
      schedulingCode.types.push(type);

      type = new SchedulingCodeType();
      type.id = 4;
      type.value = 'Paid';
      schedulingCode.types.push(type);

      type = new SchedulingCodeType();
      type.id = 5;
      type.value = 'In Office';
      schedulingCode.types.push(type);

      schedulingCode.scheduleIcon = i % 2 === 0 ? '2708-FE0F' : '1F454';

      this.schedulingCodes.push(schedulingCode);
    }
  }
}
