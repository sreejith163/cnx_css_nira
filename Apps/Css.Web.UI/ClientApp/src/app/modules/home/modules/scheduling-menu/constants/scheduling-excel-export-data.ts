import { ExcelData } from '../models/excel-data.model';

export const SchedulingExcelExportData: ExcelData[] = [
    {
        EmployeeId: 1,
        StartDate: '2020/12/02',
        EndDate: '2020/12/02',
        ActivityCode: 'open',
        StartTime: '08:00 am',
        EndTime: '12:00 pm'
    },
    {
        EmployeeId: 2,
        StartDate: '2020/12/02',
        EndDate: '2020/12/02',
        ActivityCode: 'lunch',
        StartTime: '01:00 pm',
        EndTime: '02:00 pm'
    },
    {
        EmployeeId: 1,
        StartDate: '2020/12/02',
        EndDate: '2020/12/02',
        ActivityCode: 'open',
        StartTime: '02:00 pm',
        EndTime: '04:00 pm'
    },
    {
        EmployeeId: 3,
        StartDate: '2020/12/02',
        EndDate: '2020/12/02',
        ActivityCode: 'Tea break',
        StartTime: '04:00 pm',
        EndTime: '04:30 pm'
    },
    {
        EmployeeId: 1,
        StartDate: '2020/12/02',
        EndDate: '2020/12/02',
        ActivityCode: 'open',
        StartTime: '04:30 pm',
        EndTime: '06:00 pm'
    }
];
