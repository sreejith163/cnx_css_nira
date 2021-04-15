import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ActivityLogsService } from '../../../services/activity-logs.service';
import { ActivityLogsResponse } from '../../../models/activity-logs-response.model';
import { ActivityLogsQueryParams } from '../../../models/activity-logs-query-params.model';
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ExcelService } from 'src/app/shared/services/excel.service';

@Component({
  selector: 'app-activity-logs',
  animations: [
    trigger(
      'enterAnimation', [
      state('true', style({ opacity: 1, height: 'auto' })),
      state('void', style({ opacity: 0, height: 0 })),
      transition(':enter', animate('400ms ease-in-out')),
      transition(':leave', animate('400ms ease-in-out'))
    ]
    )
  ],
  templateUrl: './activity-logs.component.html',
  styleUrls: ['./activity-logs.component.scss']
})
export class ActivityLogsComponent implements OnInit {

  activityLogsData: ActivityLogsResponse[] = [];
  activityLogDetails: ActivityLogsResponse;
  exportFileName = 'AgentAdmin_ActivityLogs_';
  @Input() employeeId: string;
  @Input() activityType: ActivityType;
  @Input() employeeName: string;
  searchKeyword: string;
  totalRecord: number;
  activityOrigin = ActivityOrigin;
  activityStatus = ActivityStatus;

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  paginationSize = Constants.paginationSize;
  maxLength = Constants.DefaultTextMaxLength;
  orderBy = 'TimeStamp';
  sortBy = 'desc';
  spinner = "agentActivityLogsSpinner";
  headerPaginationValues: HeaderPagination;

  getActivityLogsSubscription: ISubscription;
  getFieldsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];


  columnList: Array<string> = [];
  hiddenColumnList: Array<string> = [];

  constructor(
    private activityLogService: ActivityLogsService,
    private spinnerService: NgxSpinnerService,
    public activeModal: NgbActiveModal,
    private excelService: ExcelService,
  ) { }

  ngOnInit(): void {
    this.columnList = ['Timestamp', 'Executed By', 'Origin', 'Status'];
    this.loadActivityLogs();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;
    if (sortBy === 'asc') {
      this.activityLogsData.sort((a, b): number => {
        if (a[columnName] < b[columnName]) {
          return -1;
        } else if (a[columnName] > b[columnName]) {
          return 1;
        }
        else {
          return 0;
        }
      });

    } else {
      this.activityLogsData.sort((a, b): number => {
        if (a[columnName] > b[columnName]) {
          return -1;
        } else if (a[columnName] < b[columnName]) {
          return 1;
        }
        else {
          return 0;
        }
      });
    }

  }


  sortFieldData(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;
    if (sortBy === 'asc') {
      this.activityLogDetails.fieldDetails.sort((a, b): number => {
        if (a[columnName] < b[columnName]) {
          return -1;
        } else if (a[columnName] > b[columnName]) {
          return 1;
        }
        else {
          return 0;
        }
      });

    } else {
      this.activityLogDetails.fieldDetails.sort((a, b): number => {
        if (a[columnName] > b[columnName]) {
          return -1;
        } else if (a[columnName] < b[columnName]) {
          return 1;
        }
        else {
          return 0;
        }
      });
    }

  }

  public closeModal() {
    this.activeModal.close(false);
  }

  toggleDetails(activityLogId) {
    if (this.activityLogDetails?.id === activityLogId) {
      this.activityLogDetails = undefined;
    } else {
      this.getExpandedDetails(activityLogId);
    }
  }

  private getExpandedDetails(activityLogId) {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getFieldsSubscription = this.activityLogService.getActivityLogById(activityLogId)
      .subscribe((response) => {
        if (response) {
          this.activityLogDetails = response[0];
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getFieldsSubscription);
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadActivityLogs();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadActivityLogs();
  }

  search() {
    this.loadActivityLogs();
  }


  exportActivityLogs() {
      const today = new Date();
      const year = String(today.getFullYear());
      const month = String((today.getMonth() + 1)).length === 1 ?
        ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
      const day = String(today.getDate()).length === 1 ?
        ('0' + String(today.getDate())) : String(today.getDate());

      const date = year + month + day;

      let exportData: any[] = [];
      this.activityLogsData.forEach((x)=>{
        x.fieldDetails.forEach((field)=>{
          exportData.push({
            "Employee ID": x.employeeId,
            "Timestamp": x.timeStamp,
            "Executed By": x.executedBy,
            "Field": field.name,
            "Old Value": field.oldValue ? field.oldValue : "NONE",
            "New Value": field.newValue,
            "Origin": ActivityOrigin[x.activityOrigin],
            "Status": ActivityStatus[x.activityStatus],
          })
        })
      });
      
      this.excelService.exportAsExcelFile(exportData, this.exportFileName + date);
    }

    hasColumnHidden(column: string) {
      return this.hiddenColumnList?.findIndex(x => x === column) === -1;
    }
  
    hasHiddenColumnSelected(column: string) {
      return this.hiddenColumnList?.findIndex(x => x === column) !== -1;
    }
  
    onCheckColumnsToHide(e) {
      if (e.target.checked) {
        const item = e.target.value;
        this.hiddenColumnList.push(item);
      } else {
        const index = this.hiddenColumnList.findIndex(x => x === e.target.value);
        this.hiddenColumnList.splice(index, 1);
      }
    }

  private loadActivityLogs() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getActivityLogsSubscription = this.activityLogService.getActivityLogs(queryParams)
      .subscribe((response) => {
        this.activityLogsData = response.body;
        this.spinnerService.hide(this.spinner);
        let headerPaginationValues = new HeaderPagination();
        headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        this.totalRecord = headerPaginationValues.totalCount;
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getActivityLogsSubscription);
  }

  private getQueryParams() {
    const queryParams = new ActivityLogsQueryParams();
    queryParams.activityType = this.activityType;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = undefined;
    queryParams.employeeId = this.employeeId;
    queryParams.skipPageSize = true;

    return queryParams;
  }

}
