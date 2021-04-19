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
  momentCalendar:CalendarModel = new CalendarModel();

  constructor(
    private datePipe: DatePipe,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    private agentMyScheduleService: AgentMyScheduleService
  ) {
    
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(){ 
      this.getMonthlySchedule();
  }

  jumpToMonth(_month){
    // const [year, month] = [this.date.getFullYear(), _month];
    // this.date = new Date(year, month);

    this.date = new Date(moment(this.date).month(_month).toString());
    
    this.getMonthlySchedule(this.date);
  }

  setMonth(inc) {
    const [year, month] = [this.date.getFullYear(), this.date.getMonth()];
    this.date = new Date(year, month + inc, 1);
    this.getMonthlySchedule(this.date);
  }
  
  isSameMonth(scheduleDate) {
    return moment(scheduleDate).isSame(this.date.toUTCString(), 'month');
  }

  isNow(scheduleDate) {
    return this.datePipe.transform(scheduleDate,'yyyy-MM-dd') === this.datePipe.transform(new Date(),'yyyy-MM-dd');
  }

  private range(start, end, length = end - start + 1) {
    return Array.from({ length }, (_, i) => start + i)
  }

  private changeToUTCDate(date) {
    return new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000");
  }


  getMonthlySchedule(dateInput = new Date){

    const startDay = moment(dateInput.toUTCString()).clone().startOf('month').startOf('week');
    // const endDay = moment(this.date.toUTCString()).clone().endOf('month').endOf('week');

    let date = startDay.clone().subtract(1, 'day');
    this.momentCalendar = new CalendarModel();

    for(var i=1; i<=6; i++) {
      //loop for 6 weeks
      let daysArray: CalendarDay[] = [];
      for(var d=1; d<=7; d++){
        //loop for 7 days
        const dt = this.changeToUTCDate(date.add(1, 'day').clone().toString())
          daysArray.push({
            firsStartTime: '',
            lastEndTime: '',
            date: dt
          })
      }
      this.momentCalendar.weeks.push({
        days: daysArray
      });
    }

    console.log(this.momentCalendar)

    const startMonthDate = new Date(this.momentCalendar?.weeks[0]?.days[0].date);
    const endMonthDate =  new Date(this.momentCalendar?.weeks[5]?.days[6].date);
      
    this.spinnerService.show(this.calendarSpinner, SpinnerOptions);
    this.agentMyScheduleService.getAgentMySchedule(this.LoggedUser.employeeId, startMonthDate.toISOString(), endMonthDate.toISOString()).subscribe((resp)=>{
        resp.agentMySchedules.map(x =>
          x.date = x.date.substr(0,10)
        )
        this.myScheduleMonthly = resp.agentMySchedules;
        this.spinnerService.hide(this.calendarSpinner);
      },error => {
        this.spinnerService.hide(this.calendarSpinner);
      }
    );
  }

}


export class CalendarModel{
  weeks: CalendarWeek[] = [];
}

export class CalendarWeek{
  days: CalendarDay[] = [];
}

export class CalendarDay{
  date: string;
  firsStartTime: string;
  lastEndTime: string;
}
