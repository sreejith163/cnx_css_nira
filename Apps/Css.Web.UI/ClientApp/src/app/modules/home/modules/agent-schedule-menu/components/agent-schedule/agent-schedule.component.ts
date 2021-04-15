import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { from, SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { AgentMyScheduleService } from '../../services/agent-myschedule.service';
import { AgentMyScheduleDetails } from '../../models/agent-myschedule-details.model';
import { AgentMyScheduleResponse } from '../../models/agent-myschedule-response.model';
import { AgentMyScheduleChart } from '../../models/agent-myschedule-chart.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { SchedulingCodeQueryParams } from '../../models/scheduling-code-query-params.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { WeekDay } from '@angular/common';
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { CookieService } from 'ngx-cookie-service';
import * as moment from 'moment';

export class DaysInWeek {
  dayName: String;
  date: Date;
}

@Component({
  selector: 'app-agent-schedule',
  templateUrl: './agent-schedule.component.html',
  styleUrls: ['./agent-schedule.component.scss']
})

export class AgentScheduleComponent implements OnInit {
  activeTab;

  selected: Date;

  days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  // NgbCalendar
  calendarModel: NgbDateStruct;
  hoveredDate: NgbDate | null = null;
  fromDate: NgbDate;
  toDate: NgbDate | null = null;
  currentWeek;
  currentDayDateNumber;

  // default dates
  dates: Array<Date>;

  todaySpinner = 'todayScheduleSpinner';
  weeklyViewSpinner = 'weeklyViewSpinner';
  currentLanguage: string;
  LoggedUser;
  dateToday;
  myScheduleToday: AgentMyScheduleDetails;
  myScheduleChartsToday: AgentMyScheduleChart[];

  myScheduleWeek: AgentMyScheduleDetails[];
  myScheduleMonthly: AgentMyScheduleDetails[];
  scheduleIcons = [];
  daysCurrentWeek: DaysInWeek[];

  firstDayOfWeek;
  lastDayOfWeek;

  getTranslationSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private calendar: NgbCalendar,
    private spinnerService: NgxSpinnerService,
    public translate: TranslateService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private languagePreferenceService: LanguagePreferenceService,
    private agentMyScheduleService: AgentMyScheduleService,

    ) {
      this.LoggedUser = this.authService.getLoggedUserInfo();
      this.activeTab = +localStorage.getItem('activeTab') ? +localStorage.getItem('activeTab') : 1;
    }

  ngOnInit() {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();

    this.loadSchedulingCodes();
    this.loadTabContent();
  }

  // NgBCalendar Functions //

  // onDateSelection(date: NgbDate) {
  //   if (!this.fromDate && !this.toDate) {
  //     this.fromDate = date;
  //   } else if (this.fromDate && !this.toDate && date.after(this.fromDate)) {
  //     this.toDate = date;
  //   } else {
  //     this.toDate = null;
  //     this.fromDate = date;
  //   }
  // }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  }

  private convertToNgbDate(date: Date) {
    if (date) {
      date = new Date(date);
      const newDate: NgbDate = new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate());
      return newDate ?? undefined;
    }
  }

  goToNextWeek(){
    this.currentDayDateNumber = this.currentDayDateNumber + 7;
    this.getCurrentWeek(this.currentDayDateNumber);
  }

  goToPrevWeek(){
    this.currentDayDateNumber = this.currentDayDateNumber - 7;
    this.getCurrentWeek(this.currentDayDateNumber);
  }

  // end of NgbCalendar //


  loadTabContent(){
    // call the methods for loading data based on what the active tab is
    switch(this.activeTab) { 
      case 1: { 
        // for daily view
        localStorage.setItem('activeTab', '1');
        this.getTodaySchedule();
        this.currentDayDateNumber = new Date().getDate();
        this.getCurrentWeek(this.currentDayDateNumber);
        break; 

      } 
      case 2: { 
        // for weekly view
        localStorage.setItem('activeTab', '2');
        this.getTodaySchedule();
        this.currentDayDateNumber = new Date().getDate();
        this.getCurrentWeek(this.currentDayDateNumber);
        break; 
      }
      
      case 3: {
        // for monthly view  
        localStorage.setItem('activeTab', '3');
        this.getTodaySchedule();
        break; 
      }
      
      default: { 
        this.getTodaySchedule();
        this.currentDayDateNumber = new Date().getDate();
        this.getCurrentWeek(this.currentDayDateNumber);
        break; 
      } 
    } 
  }

  getCurrentWeekSchedule(startDate, endDate){
    this.spinnerService.show(this.weeklyViewSpinner, SpinnerOptions);
    this.agentMyScheduleService.getAgentMySchedule(this.LoggedUser.employeeId, startDate, endDate).subscribe((resp: AgentMyScheduleResponse)=>{
        for (let index = 0; index < resp.agentMySchedules.length; index++) {
          const chartsPerDay = resp.agentMySchedules[index].charts;
          if (chartsPerDay != null){
            // sort charts by startDateTime
            chartsPerDay.sort((a, b) => a.startDateTime < b.startDateTime ? -1 : a.startDateTime > b.startDateTime ? 1 : 0);

            // map the dates to display time format only
            chartsPerDay.map(x =>{
              x.endDateTime = moment(x.endDateTime.replace('Z', '').replace('T', ' ')).format('hh:mm a'),
              x.startDateTime = moment(x.startDateTime.replace('Z', '').replace('T', ' ')).format('hh:mm a'),
              x.schedulingCodeId = x.schedulingCodeId
            });
          }
        }
        this.myScheduleWeek = resp.agentMySchedules;
        // sort daily schedules by day
        resp.agentMySchedules.sort((a, b) => a.day < b.day ? -1 : a.day > b.day ? 1 : 0);
        this.spinnerService.hide(this.weeklyViewSpinner);
      },error => {
        this.dates = this.weeklyDateRange(new Date(startDate), new Date(endDate));
        this.myScheduleWeek = null;
        // console.log(error);
        this.spinnerService.hide(this.weeklyViewSpinner);
      }
    );
  }


  private weeklyDateRange(start,end){
      for(var arr=[],dt=new Date(start); dt<=end; dt.setDate(dt.getDate()+1)){
          arr.push(new Date(dt));
      }
      return arr;
  }


  getTodaySchedule(){

    const today = new Date();
    const year = String(today.getFullYear());
    const month = String((today.getMonth() + 1)).length === 1 ?
      ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
    const day = String(today.getDate()).length === 1 ?
      ('0' + String(today.getDate())) : String(today.getDate());

    this.dateToday = `${year}-${month}-${day}`;

    const isoDate = new Date(today.getFullYear(), today.getMonth() + 1, today.getDate() + 1);
    var startDate = today.toISOString();
    var endDate = isoDate.toISOString();

    this.agentMyScheduleService.getAgentMySchedule(this.LoggedUser.employeeId, startDate, endDate).subscribe((resp: AgentMyScheduleResponse)=>{
      // get charts based on what day it is locally
      const chartsToday = resp.agentMySchedules.find(x => x.day == new Date().getDay()).charts;

      if ( chartsToday != null) {

        // sort charts by startDateTime
        chartsToday.sort((a, b) => a.startDateTime < b.startDateTime ? -1 : a.startDateTime > b.startDateTime ? 1 : 0);

        // map DateTime to Time format
        chartsToday.map(x =>{
          x.endDateTime = moment(x.endDateTime.replace('Z', '').replace('T', ' ')).format('hh:mm a'),
          x.startDateTime = moment(x.startDateTime.replace('Z', '').replace('T', ' ')).format('hh:mm a'),
          x.schedulingCodeId = x.schedulingCodeId
        });
      }
      
      this.myScheduleChartsToday = chartsToday;
      
    });

  }

  
  getCurrentWeek(currentDayDateNumber){
    // operation (next or prev)
    this.daysCurrentWeek = [
      this.setDayforWeek(WeekDay.Sunday, currentDayDateNumber),
      this.setDayforWeek(WeekDay.Monday, currentDayDateNumber),
      this.setDayforWeek(WeekDay.Tuesday, currentDayDateNumber),
      this.setDayforWeek(WeekDay.Wednesday, currentDayDateNumber),
      this.setDayforWeek(WeekDay.Thursday, currentDayDateNumber),
      this.setDayforWeek(WeekDay.Friday, currentDayDateNumber),
      this.setDayforWeek(WeekDay.Saturday, currentDayDateNumber),
    ]

    this.firstDayOfWeek = this.setDayforWeek(WeekDay.Sunday, currentDayDateNumber);
    this.lastDayOfWeek = this.setDayforWeek(WeekDay.Saturday, currentDayDateNumber);

    this.fromDate = this.convertToNgbDate(this.firstDayOfWeek.date.toISOString());

    this.toDate = this.calendar.getNext(this.fromDate, 'd', 6);

    const startDate =  new Date(this.firstDayOfWeek.date.getFullYear(), this.firstDayOfWeek.date.getMonth(), this.firstDayOfWeek.date.getDate());
    const endDate =  new Date(this.lastDayOfWeek.date.getFullYear(), this.lastDayOfWeek.date.getMonth(), this.lastDayOfWeek.date.getDate());
    
    this.getCurrentWeekSchedule(startDate.toISOString(), endDate.toISOString());

  }

  setDayforWeek(day, currentDayDateNumber){
    // get the day index
    // 0 - Sunday, 1 - Monday, 2 - Tuesday, 3 - Wednesday, 4 - Thursday, 5 - Friday, 6 - Saturday
    var arrayOfWeekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

    var curr = new Date; // get current date

    // get firstday of the week
    var currentDate = new Date(curr.setDate(currentDayDateNumber));
    var firstDayOfTheWeek = currentDate.getDate() - (currentDate.getDay());

    const selectedDay = firstDayOfTheWeek + day;
    const date = new Date(currentDate.setDate(selectedDay));
    const dayOfWeek = date.getDay()

    return { dayName: arrayOfWeekdays[dayOfWeek], date: date};
  }


  getSchedulingIcon(schedulingCodeId){
    const code = this.schedulingCodes.find(x => x.id === schedulingCodeId);
    return code ? this.unifiedToNative(code?.icon?.value) : '';
  }

  getSchedulingCode(schedulingCodeId){
    const code = this.schedulingCodes.find(x => x.id === schedulingCodeId);
    return code ? code?.description : '';
  }

  schedulingCodes = [];

  private loadSchedulingCodes() {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = 'id, description, icon';

    this.spinnerService.show(this.todaySpinner, SpinnerOptions);

    this.agentMyScheduleService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
          this.spinnerService.hide(this.todaySpinner);
        }
      }, (error) => {
        this.spinnerService.hide(this.todaySpinner);
        // console.log(error);
      });
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
  }

  private subscribeToTranslations() {
    this.getTranslationSubscription = this.languagePreferenceService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      });

    this.subscriptions.push(this.getTranslationSubscription);
  }

  private preLoadTranslations(){
    // Preload the user language //
    const browserLang = this.route.snapshot.data.languagePreference.languagePreference;
    this.currentLanguage = browserLang ? browserLang : 'en';
    this.translate.use(this.currentLanguage);
}

  private loadTranslations() {
    // load the user language from api //
    this.languagePreferenceService.getLanguagePreference(this.LoggedUser.employeeId).subscribe((langPref: LanguagePreference) => {
      this.currentLanguage = langPref.languagePreference ? langPref.languagePreference : 'en';
      this.translate.use(this.currentLanguage);
    });
  }
}
