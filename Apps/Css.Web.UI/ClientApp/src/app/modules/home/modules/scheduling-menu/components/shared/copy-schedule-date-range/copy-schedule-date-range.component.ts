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
import { CopyScheduleDateRangeModel } from '../../../models/copy-schedule-date-range.model';

@Component({
  selector: 'app-copy-schedule-date-range',
  templateUrl: './copy-schedule-date-range.component.html',
  styleUrls: ['./copy-schedule-date-range.component.scss'],
  providers: [DatePipe, { provide: NgbDateAdapter, useClass: NgbDateToIsoStringAdapter }]
})
export class CopyScheduleDateRangeComponent implements OnInit, OnDestroy {

  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate: NgbDate | null = null;
  today = this.calendar.getToday();
  spinner = 'date-range';
  modalRef: NgbModalRef;
  startDate: any;
  endDate: any;

  @Input() dateFrom: Date;
  @Input() dateTo: Date;

  getShceduleDateRangeSubscription: ISubscription;
  updateScheduleDateRangeSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    private modalService: NgbModal,
    private datepipe: DatePipe,
    public formatter: NgbDateParserFormatter,
    public translate: TranslateService,
  ) { }

  ngOnInit(): void {
    this.startDate = this.getFormattedDate(this.dateFrom);
    this.endDate = this.getFormattedDate(this.dateTo);
  }

  ngOnDestroy() {

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
      const rangeModel = new CopyScheduleDateRangeModel();
      rangeModel.dateFrom = this.dateFrom.toString();
      rangeModel.dateTo = this.dateTo.toString();
      console.log(rangeModel)
      this.activeModal.close(rangeModel);
    }
  }

  // private changeToUTCDate(date){
  //   return new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000"));
  // }

  private changeToUTCDate(date) {
    return new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000");
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Error';
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.messageType = ContentType.String;
  }

}
