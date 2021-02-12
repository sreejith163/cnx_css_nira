import { Component, OnInit } from '@angular/core';
import { NgbCalendar, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { Constants } from 'src/app/shared/util/constants.util';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SkillTagDetails } from '../../../../setup-menu/models/skill-tag-details.model';
import { SkillTagQueryParams } from '../../../../setup-menu/models/skill-tag-query-params.model';
import { SkillTagService } from '../../../../setup-menu/services/skill-tag.service';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';

@Component({
  selector: 'app-view-ou-screen-filter',
  templateUrl: './view-ou-screen-filter.component.html',
  styleUrls: ['./view-ou-screen-filter.component.scss']
})
export class ViewOuScreenFilterComponent implements OnInit {
  skillGroupId: number;
  currentPage = 1;
  pageSize = 10;
  currentDate: string;
  characterSplice = 25;
  paginationSize = Constants.paginationSize;
  translationValues: TranslationDetails[] = [];
  maxLength = Constants.DefaultTextMaxLength;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'skillTags';
  getSkillTagsSubscription: ISubscription;
  getSkillTagSubscription: ISubscription;
  clientId: number;
  clientLobGroupId: number;
  skillTags: SkillTagDetails[] = [];
  totalSkillTagsRecord: number;
  searchKeyword: string;
  headerPaginationValues: HeaderPagination;
  subscriptions: ISubscription[] = [];
  tabIndex: number;
  totalSchedulingGridData: AgentSchedulesResponse[] = [];

  startDate: any = this.calendar.getToday();

  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  constructor(
    private calendar: NgbCalendar,
    private spinnerService: NgxSpinnerService,
    private skillTagSevice: SkillTagService,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
  ) {
   }

   ngOnInit(): void {
    this.tabIndex = AgentScheduleType.Scheduling;
  }
  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
    this.loadSkillTags();
  }
  private getQueryParams() {
    const skillTagQueryParams = new SkillTagQueryParams();
    skillTagQueryParams.clientId = this.clientId;
    skillTagQueryParams.clientLobGroupId = this.clientLobGroupId;
    skillTagQueryParams.skillGroupId = this.skillGroupId;
    skillTagQueryParams.pageNumber = this.currentPage;
    skillTagQueryParams.pageSize = this.pageSize;
    skillTagQueryParams.searchKeyword = this.searchKeyword ?? '';
    skillTagQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    skillTagQueryParams.fields = '';

    return skillTagQueryParams;
  }
  private loadSkillTags() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillTagsSubscription = this.skillTagSevice.getSkillTags(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.skillTags = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalSkillTagsRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSkillTagsSubscription);
  }

}
