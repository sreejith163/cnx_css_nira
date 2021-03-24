import { Component, OnDestroy, OnInit } from '@angular/core';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { TimeOffsService } from '../../../services/time-offs.service';
import { TimeOffResponse } from '../../../models/time-offs-response.model';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { NgxSpinnerService } from 'ngx-spinner';
import { WeekDay } from '@angular/common';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { AddUpdateTimeOffsComponent } from '../add-update-time-offs/add-update-time-offs.component';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { DeSelectedTime } from '../../../enums/deSelected-time.enum';

@Component({
  selector: 'app-time-offs-list',
  templateUrl: './time-offs-list.component.html',
  styleUrls: ['./time-offs-list.component.scss']
})
export class TimeOffsListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  totalRecord: number;
  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'time-offs';

  modalRef: NgbModalRef;
  weekDay = WeekDay;
  paginationSize = Constants.paginationSize;
  maxLength = Constants.DefaultTextMaxLength;

  timeOffs: TimeOffResponse[] = [];
  schedulingCodes: SchedulingCode[] = [];

  deleteTimeOffSubscription: ISubscription;
  getTimeOffsSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private schedulingCodeService: SchedulingCodeService,
    private timeOffsService: TimeOffsService,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.loadSchedulingCodes();
    this.loadTimeOffs();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getDeselectedTime(value: number) {
    const deselectedTime = Constants.TimeOffDeselectedTimeOption;
    return deselectedTime.find(x => x.id === +value).value;
  }

  getTimeOffCode(id: number) {
    const item = this.schedulingCodes.find(x => x.id === +id);
    if (item) {
      const codePoints = item?.icon?.value.split('-').map(u => parseInt(`0x${u}`, 16));
      const icon = String.fromCodePoint(...codePoints);
      return icon + ' ' + item?.description;
    }
    return '';
  }

  getDayRequestOn(weekDays: number[]) {
    let days = '';
    for (const item of weekDays) {
      const day = WeekDay[item];
      days = days.concat(', ' + day);
    }

    return days.length > 1 ? days.substr(1) : '';
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadTimeOffs();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadTimeOffs();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadTimeOffs();
  }

  clearSearchText() {
    this.searchKeyword = undefined;
    this.loadTimeOffs();
  }

  addTimeOff() {
    this.getModalPopup(AddUpdateTimeOffsComponent, 'xl');
    this.modalRef.componentInstance.operation = ComponentOperation.Add;
    this.modalRef.componentInstance.schedulingCodes = this.schedulingCodes;

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.showSuccessPopUpMessage('The record has been added!');
    });
  }

  search() {
    this.loadTimeOffs();
  }

  editTimeOffs(timeOffData: TimeOffResponse) {
    this.getModalPopup(AddUpdateTimeOffsComponent, 'xl');
    this.modalRef.componentInstance.operation = ComponentOperation.Edit;
    this.modalRef.componentInstance.schedulingCodes = this.schedulingCodes;
    this.modalRef.componentInstance.timeOffCodeData = timeOffData;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.showSuccessPopUpMessage('The record has been updated!');
      } else {
        this.showSuccessPopUpMessage('No changes has been made!', false);
      }
    });
  }

  deleteTimeOffs(id: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.modalRef.componentInstance.deleteRecordIndex = id;
    this.modalRef.componentInstance.headingMessage = 'Are you sure?';
    this.modalRef.componentInstance.contentMessage = 'You won’t be able to revert this!';

    this.modalRef.result.then((result) => {
      if (result && result === id) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteTimeOffSubscription = this.timeOffsService.deleteTimeoff(id)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.showSuccessPopUpMessage('The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            if (error.status === 424) {
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.modalRef.componentInstance.headingMessage = 'Error?';
              this.modalRef.componentInstance.contentMessage = error.error;
            }
          });

        this.subscriptions.push(this.deleteTimeOffSubscription);
      }
    });
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.modalRef.componentInstance.headingMessage = 'Success';
    this.modalRef.componentInstance.contentMessage = contentMessage;

    if (needRefresh) {
      this.modalRef.result.then(() => {
        this.loadTimeOffs();
      });
    }
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private getQueryParams() {
    const queryParams = new QueryStringParameters();
    queryParams.pageNumber = this.currentPage;
    queryParams.pageSize = this.pageSize;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.skipPageSize = false;
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = undefined;

    return queryParams;
  }

  private loadTimeOffs() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.getTimeOffsSubscription = this.timeOffsService.getTimeOffs(queryParams)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        if (response) {
          this.timeOffs = response.body;
          let headerPaginationValues = new HeaderPagination();
          headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalRecord = headerPaginationValues.totalCount;
        }
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getTimeOffsSubscription);
  }

  private loadSchedulingCodes() {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = 'id, description, icon';

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
        }
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

}
