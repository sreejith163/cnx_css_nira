import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal, NgbCalendar, NgbDate, NgbDateParserFormatter, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
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

@Component({
  selector: 'app-date-range-pop-up',
  templateUrl: './date-range-pop-up.component.html',
  styleUrls: ['./date-range-pop-up.component.scss'],
  providers: [DatePipe]
})
export class DateRangePopUpComponent implements OnInit, OnDestroy {

  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  today = this.calendar.getToday();
  spinner = 'date-range';
  modalRef: NgbModalRef;
  startDate: any;
  endDate: any;

  @Input() agentScheduleId: string;
  @Input() hasNewRangeSaved: boolean;
  @Input() dateFrom: Date;
  @Input() dateTo: Date;
  @Input() operation: ComponentOperation;

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
    this.startDate = this.dateFrom;
    this.endDate = this.dateTo;
    if (this.operation === ComponentOperation.Add) {
      this.dateFrom = new Date(this.today.year, this.today.month - 1, this.today.day, 0, 0, 0, 0);
    }
    if (this.dateFrom || this.dateTo) {
      this.fromDate = this.convertToNgbDate(this.dateFrom) ?? this.today;
      this.toDate = this.convertToNgbDate(this.dateTo) ?? this.today;
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  onDateSelection(date: NgbDate) {
    const x = this.calendar.getWeekday(date);
    const preCount = x === 7 ? 0 : x;
    this.fromDate = this.calendar.getPrev(date, 'd', preCount);
    this.toDate = this.calendar.getNext(this.fromDate, 'd', 6);
    this.dateFrom = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
    this.dateTo = new Date(this.toDate.year, this.toDate.month - 1, this.toDate.day, 0, 0, 0, 0);
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
          if (this.hasNewRangeSaved) {
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
    model.dateFrom = this.getDateInStringFormat(this.dateFrom);
    model.dateTo = this.getDateInStringFormat(this.dateTo);
    this.getShceduleDateRangeSubscription = this.agentSchedulesService.getAgentScheduleRange(this.agentScheduleId, model)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        if (!response.body) {
          const rangeModel = new ScheduleDateRangeBase();
          rangeModel.dateFrom = this.getFormattedDate(this.dateFrom);
          rangeModel.dateTo = this.getFormattedDate(this.dateTo);
          this.activeModal.close(rangeModel);
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

  private updateScheduleDateRange() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const model = new UpdateScheduleDateRange();
    model.oldDateFrom = this.getFormattedDate(this.startDate);
    model.oldDateTo = this.getFormattedDate(this.endDate);
    model.newDateFrom = this.getFormattedDate(this.dateFrom);
    model.newDateTo = this.getFormattedDate(this.dateTo);
    model.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    this.updateScheduleDateRangeSubscription = this.agentSchedulesService.updateAgentScheduleRange(this.agentScheduleId, model)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
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

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }
}
