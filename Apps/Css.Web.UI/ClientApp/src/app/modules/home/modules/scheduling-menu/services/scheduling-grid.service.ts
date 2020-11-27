import { Injectable } from '@angular/core';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { SchedulingCalendar } from '../models/scheduling-calendar.model';
import { SchedulingGrid } from '../models/scheduling-grid.model';

@Injectable()
export class SchedulingGridService {

  gridData: SchedulingGrid[] = [];

  constructor() {
    this.createSchedulingGridData();
  }

createSchedulingGridData() {
  let gridData = new SchedulingGrid();
  gridData.employeeId = 1;
  gridData.name = 'Employee 1';
  gridData.fromDate = '2020-09-10';
  gridData.toDate = '2020-09-20';
  gridData.status = 1;
  gridData.createdDate = '2020-09-10';
  gridData.createdBy = 'User 1';
  gridData.modifiedDate = '2020-08-10';
  gridData.modifiedBy = 'User 1';
  const calendar = new SchedulingCalendar();
  calendar.weekDays.push(
    {
      day: 0,
      times: [{
        from: '00:10 am',
        to: '00:35 am',
        icon: '1F3E7'
      },
      {
        from: '01:40 am',
        to: '02:35 am',
        icon: '1F384'
      }]
    },
    {
      day: 1,
      times: [{
        from: '00:10 am',
        to: '00:35 am',
        icon: '1F384'
      }]
    }
  );
  gridData.calendar = calendar;

  this.gridData.push(gridData);

  gridData = new SchedulingGrid();
  gridData.employeeId = 2;
  gridData.name = 'Employee 2';
  gridData.fromDate = '2020-09-10';
  gridData.toDate = '2020-09-20';
  gridData.status = 2;
  gridData.createdDate = '2020-09-10';
  gridData.createdBy = 'User 2';
  gridData.modifiedDate = '2020-08-10';
  gridData.modifiedBy = 'User 2';

  this.gridData.push(gridData);

  gridData = new SchedulingGrid();
  gridData.employeeId = 3;
  gridData.name = 'Employee 3';
  gridData.fromDate = '2020-09-10';
  gridData.toDate = '2020-09-20';
  gridData.status = 3;
  gridData.createdDate = '2020-09-10';
  gridData.createdBy = 'User 3';
  gridData.modifiedDate = '2020-08-10';
  gridData.modifiedBy = 'User 3';

  this.gridData.push(gridData);
}

addSchedulingGridData(agent: SchedulingGrid) {
  agent.createdDate = String(new Date());
  agent.createdBy = 'User ' + (this.gridData.length + 1);
  this.gridData.push(agent);
}

updateSchedulingGridData(agent: SchedulingGrid) {
  const agentIndex = this.gridData.findIndex(x => x.employeeId === agent.employeeId);
  if (agentIndex !== -1) {
    this.gridData[agentIndex] = agent;
  }
}

deleteSchedulingGridData(employeeId: number) {
  const gridDataIndex = this.gridData.findIndex(x => x.employeeId === employeeId);
  if (gridDataIndex !== -1) {
    this.gridData.splice(gridDataIndex, 1);
  }
}

getSchedulingGridData() {
  return this.gridData;
}

getTablePageSizeList() {
  const tablePageSize: PaginationSize[] = [
    {
      count: 5,
      text: '5/Page'
    },
    {
      count: 10,
      text: '10/Page'
    },
    {
      count: 15,
      text: '15/Page'
    }
  ];

  return tablePageSize;

}
}
