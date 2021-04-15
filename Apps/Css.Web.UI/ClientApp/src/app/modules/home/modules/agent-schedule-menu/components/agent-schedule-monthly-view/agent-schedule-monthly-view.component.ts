import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from 'src/app/core/services/auth.service';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AgentMyScheduleDetails } from '../../models/agent-myschedule-details.model';
import { AgentMyScheduleResponse } from '../../models/agent-myschedule-response.model';
import { AgentMyScheduleService } from '../../services/agent-myschedule.service';
import { DatePipe } from '@angular/common';
import { DAY_MS } from '../../enums/agent-schedule.enum';
import * as moment from 'moment';

export enum Month {
  January = "January",
  February = "February",
  March = "March",
  April = "April", 
  May = "May",
  June = "June",
  July = "July",
  August = "August",
  September = "September",
  October = "October",
  November = "November",
  December = "December"
}


@Component({
  selector: 'my-schedule-monthly-view',
  templateUrl: './agent-schedule-monthly-view.component.html',
  styleUrls: ['./agent-schedule-monthly-view.component.scss']
})
export class AgentScheduleMonthlyViewComponent implements OnInit {
  LoggedUser;
  dates: Array<Date>;
  weeks: [];
  days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  months = [Month.January, Month.February, Month.March, Month.April, Month.May, Month.June, Month.July, Month.August, Month.September, Month.November, Month.December];
  date = new Date();
  @Output() selected = new EventEmitter();
  calendarSpinner = 'calendarSpinner';

  myScheduleMonthly: AgentMyScheduleDetails[];

  constructor(
    private datePipe: DatePipe,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    private agentMyScheduleService: AgentMyScheduleService
  ) {
    
    this.LoggedUser = this.authService.getLoggedUserInfo();

  }

  ngOnInit(){
    this.getMonthlySchedule(this.date);    
  }

  jumpToMonth(_month){
    const [year, month] = [this.date.getFullYear(), _month];
    this.date = new Date(year, month);
    this.getMonthlySchedule(this.date);
  }

  setMonth(inc) {
    const [year, month] = [this.date.getFullYear(), this.date.getMonth()];
    this.date = new Date(year, month + inc, 1);
    this.getMonthlySchedule(this.date);
  }
  
  isSameMonth(scheduleDate) {
    var date = new Date(scheduleDate);
    return date.getMonth() === this.date.getMonth();
  }

  isNow(scheduleDate) {
    var date = new Date(scheduleDate);
    return this.datePipe.transform(date,'yyyy-MM-dd') === this.datePipe.transform(new Date(),'yyyy-MM-dd');
  }

  private getCalendarStartDay(date = new Date) {
    const [year, month] = [date.getUTCFullYear(), date.getUTCMonth()];
    const firstDayOfMonth = new Date(year, month, 1).getTime();

    return this.range(1,7)
      .map(num => new Date(firstDayOfMonth - DAY_MS.util * num))
      .find(dt => dt.getUTCDay() === 0)
  }

  private range(start, end, length = end - start + 1) {
    return Array.from({ length }, (_, i) => start + i)
  }

  getMonthlySchedule(date = new Date){

    const calendarStartTime =  this.getCalendarStartDay(date).getTime();
    this.dates = this.range(0, 41)
    .map(num => new Date(calendarStartTime + DAY_MS.util * num));
    
    const startMonthDate =  new Date(this.dates[0].getFullYear(), this.dates[0].getMonth(), this.dates[0].getDate());
    const endMonthDate =  new Date(this.dates[41].getFullYear(), this.dates[41].getMonth(), this.dates[41].getDate());

    this.spinnerService.show(this.calendarSpinner, SpinnerOptions);
    this.agentMyScheduleService.getAgentMySchedule(this.LoggedUser.employeeId, startMonthDate.toISOString(), endMonthDate.toISOString()).subscribe((resp)=>{
        resp.agentMySchedules.map(x =>
          x.date = x.date.substr(0,10)
        )
        this.myScheduleMonthly = resp.agentMySchedules;
        this.spinnerService.hide(this.calendarSpinner);
      },error => {
        this.myScheduleMonthly = null;
        // console.log(error);
        this.spinnerService.hide(this.calendarSpinner);
      }
    );
  }

}


