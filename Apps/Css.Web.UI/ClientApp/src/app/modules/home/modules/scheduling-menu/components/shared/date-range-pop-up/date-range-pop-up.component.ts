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

@Component({
  selector: 'app-date-range-pop-up',
  templateUrl: './date-range-pop-up.component.html',
  styleUrls: ['./date-range-pop-up.component.scss']
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
  @Input() dateFrom: Date;
  @Input() dateTo: Date;
  @Input() operation: ComponentOperation;

  updateScheduleDateRangeSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    public formatter: NgbDateParserFormatter,
    public translate: TranslateService,
  ) {}

  ngOnInit(): void {
    this.startDate = this.dateFrom;
    this.endDate = this.dateTo;
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
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
      const newDate = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
      this.dateFrom = newDate;
    } else if (this.fromDate && !this.toDate && date && !date.before(this.fromDate)) {
      this.toDate = date;
      const newDate = new Date(this.toDate.year, this.toDate.month - 1, this.toDate.day, 0, 0, 0, 0);
      this.dateTo = newDate;
    } else {
      this.toDate = null;
      this.dateTo = null;
      this.fromDate = date;
      const newDate = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
      this.dateFrom = newDate;
    }
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
    if (this.operation === ComponentOperation.Edit) {
      if (this.startDate !== this.fromDate || this.endDate !== this.toDate) {
        this.updateScheduleDateRange();
      } else {
        this.activeModal.close({ needRefresh: false });
      }
    } else {
      const model = {
        fromDate: this.dateFrom,
        toDate: this.dateTo
      };
      this.activeModal.close(model);
    }
  }

  private updateScheduleDateRange() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const model = new UpdateScheduleDateRange();
    model.oldDateFrom = new Date(this.startDate);
    model.oldDateTo = new Date(this.endDate);
    model.newDateFrom = new Date(this.dateFrom);
    model.newDateTo = new Date(this.dateTo);
    model.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    this.updateScheduleDateRangeSubscription = this.agentSchedulesService.updateAgentScheduleRange(this.agentScheduleId, model)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        this.getModalPopup(ErrorWarningPopUpComponent, 'sm', error.message);
        console.log(error);
      });

    this.subscriptions.push(this.updateScheduleDateRangeSubscription);
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Error';
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private convertToNgbDate(date: Date) {
    if (date) {
      date = new Date(date);
      const newDate: NgbDate = new NgbDate(date.getUTCFullYear(), date.getUTCMonth() + 1, date.getUTCDate() + 1);
      return newDate ?? undefined;
    }
  }

}
