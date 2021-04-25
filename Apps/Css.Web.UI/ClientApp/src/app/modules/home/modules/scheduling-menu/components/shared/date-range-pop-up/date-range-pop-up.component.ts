import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal, NgbCalendar, NgbDate, NgbDateAdapter, NgbDateParserFormatter, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ComponentOperation } from '../../../../../../../shared/enums/component-operation.enum';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { UpdateScheduleDateRange } from '../../../models/update-schedule-date-range.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ScheduleDateRangeBase } from '../../../models/schedule-date-range-base.model';
import { DateRangeQueryParms } from '../../../models/date-range-query-params.model';
import { DatePipe } from '@angular/common';
import { Constants } from 'src/app/shared/util/constants.util';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { NgbDateToIsoStringAdapter } from '../../../helpers/ngb-date-to-iso-string-adapter';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import * as moment from 'moment';

@Component({
  selector: 'app-date-range-pop-up',
  templateUrl: './date-range-pop-up.component.html',
  styleUrls: ['./date-range-pop-up.component.scss'],
  providers: [DatePipe, { provide: NgbDateAdapter, useClass: NgbDateToIsoStringAdapter }]
})
export class DateRangePopUpComponent implements OnInit, OnDestroy {

  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate: NgbDate | null = null;
  today = this.calendar.getToday();
  spinner = 'date-range';
  modalRef: NgbModalRef;
  startDate: any;
  endDate: any;

  @Input() agentScheduleId: string;
  @Input() isEditNewDateRange: boolean;
  @Input() dateFrom: Date;
  @Input() dateTo: Date;
  @Input() operation: ComponentOperation;
  @Input() el: AgentSchedulesResponse;

  getShceduleDateRangeSubscription: ISubscription;
  updateScheduleDateRangeSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private datepipe: DatePipe,
    public formatter: NgbDateParserFormatter,
    public translate: TranslateService,
  ) { }

  ngOnInit(): void {
    this.startDate = this.getFormattedDate(this.dateFrom);
    this.endDate = this.getFormattedDate(this.dateTo);
    if (this.dateFrom || this.dateTo && this.operation === ComponentOperation.Edit) {
      this.fromDate = this.convertToNgbDate(this.getFormattedDate(this.dateFrom)) ?? this.today;
      this.toDate = this.convertToNgbDate(this.getFormattedDate(this.dateTo)) ?? this.today;
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
    this.el = undefined;
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }




  onDateSelection(date: NgbDate) {
    const x = this.calendar.getWeekday(date);
    const preCount = x === 7 ? 0 : x;
    this.fromDate = this.calendar.getPrev(date, 'd', preCount);
    this.toDate = this.calendar.getNext(this.fromDate, 'd', 6);

    this.dateFrom = new Date(`${this.convertNgbToUTCString(this.fromDate)} 00:00`);
    this.dateTo = new Date(`${this.convertNgbToUTCString(this.toDate)} 00:00`);


  }


  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date?.toString().replace("Z", ""), 'yyyy-MM-dd');
    return new Date(`${transformedDate}`);
  }

  // private getFormattedDateString(date: Date) {
  //   return this.datepipe.transform(date.toString().replace("Z", ""), 'yyyy-MM-dd');
  // }

  private formatDateMoment(date){
    let dt = new Date(date).toUTCString();
    const transformedDate = moment(dt).format('YYYY-MM-DD');
    return transformedDate;
  }

  private formatStringToDateMoment(date){
    const transformedDate = moment(date).format('YYYY-MM-DD');
    return transformedDate;
  }




  convertNgbToUTCString(date) {
    const day = date.day < 10 ? '0' + date.day : date.day;
    const month = date.month < 10 ? '0' + date.month : date.month;

    return date.year + '-' + month + '-' + day;
  }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  }

  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const parsed = this.formatter.parse(input);
    return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
  }

  save() {
    if (!this.dateFrom || !this.dateTo) {
      this.getModalPopup(ErrorWarningPopUpComponent, 'sm', Constants.DateRangeRequiredMessage);
    } else {
      if (this.startDate !== this.dateFrom || this.endDate !== this.dateTo) {
        if (this.operation === ComponentOperation.Edit) {
          if (this.isEditNewDateRange) {
            this.addScheduleDateRange();
          } else {
            this.updateScheduleDateRange();
          }
        } else {
          this.addScheduleDateRange();
        }
      } else {
        this.activeModal.close({ needRefresh: false });
      }
    }
  } 
  
  private addScheduleDateRange() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const model = new DateRangeQueryParms();
    model.dateFrom = this.getFormattedDate(this.dateFrom).toISOString();
    model.dateTo = this.getFormattedDate(this.dateTo).toISOString();
    this.getShceduleDateRangeSubscription = this.agentSchedulesService.getAgentScheduleRange(this.agentScheduleId, model)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        if (!response.body) {

          let dtFrom = this.formatDateMoment(this.dateFrom);
          let dtTo = this.formatDateMoment(this.dateTo);

          // check if date range already exists in the list
          var isExisting = this.el?.ranges.find(x => this.formatStringToDateMoment(x.dateTo) == dtTo && this.formatStringToDateMoment(x.dateFrom) == dtFrom);

          console.log(this.el?.ranges, dtFrom, dtTo)

          if(isExisting !== undefined){
            this.getModalPopup(ErrorWarningPopUpComponent, 'sm', Constants.DateRangeConflictMessage);            
          }else{
            const rangeModel = new ScheduleDateRangeBase();
            rangeModel.dateFrom = dtFrom;
            rangeModel.dateTo = dtTo;
            this.activeModal.close(rangeModel);
          }
        } else {
          this.getModalPopup(ErrorWarningPopUpComponent, 'sm', Constants.DateRangeConflictMessage);
        }
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        this.getModalPopup(ErrorWarningPopUpComponent, 'sm', Constants.DateRangeConflictMessage);
        console.log(error);
      });
    this.subscriptions.push(this.getShceduleDateRangeSubscription);
  }

  private changeToUTCDate(date){
    return new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000"));
  }

  private updateScheduleDateRange() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const model = new UpdateScheduleDateRange();
    model.oldDateFrom = this.startDate;
    model.oldDateTo = this.endDate;
    model.newDateFrom = this.changeToUTCDate(this.dateFrom);
    model.newDateTo = this.changeToUTCDate(this.dateTo);

    console.log(model)

    model.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    this.updateScheduleDateRangeSubscription = this.agentSchedulesService.updateAgentScheduleRange(this.agentScheduleId, model)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        const rangeModel = new ScheduleDateRangeBase();
        rangeModel.dateFrom = this.formatDateMoment(this.dateFrom);
        rangeModel.dateTo = this.formatDateMoment(this.dateTo);
        this.activeModal.close(rangeModel);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.getModalPopup(ErrorWarningPopUpComponent, 'sm', Constants.DateRangeConflictMessage);
        }
        console.log(error);
      });

    this.subscriptions.push(this.updateScheduleDateRangeSubscription);
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Error';
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.messageType = ContentType.String;
  }

  private convertToNgbDate(date: Date) {
    if (date) {
      date = new Date(date);
      const newDate: NgbDate = new NgbDate(date.getUTCFullYear(),
        date.getUTCMonth() + 1, this.operation === ComponentOperation.Add ? date.getUTCDate() + 1 : date.getUTCDate());
      return newDate ?? undefined;
    }
  }

  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }

    const date = new Date(startDate);
    return date.toDateString();
  }

}
