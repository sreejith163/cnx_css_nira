import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { SkillGroupDetails } from 'src/app/modules/home/modules/setup-menu/models/skill-group-details.model';
import { SkillGroupQueryParameters } from 'src/app/modules/home/modules/setup-menu/models/skill-group-query-parameters.model';
import { SkillGroupService } from 'src/app/modules/home/modules/setup-menu/services/skill-group.service';

@Component({
  selector: 'app-skill-group-typeahead',
  templateUrl: './skill-group-typeahead.component.html',
  styleUrls: ['./skill-group-typeahead.component.scss']
})
export class SkillGroupTypeaheadComponent implements OnInit, OnDestroy, OnChanges {

  pageNumber = 1;
  skillGroupItemsBufferSize = 100;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  dropdownSearchKeyWord = '';
  loading = false;

  skillGroupItemsBuffer: SkillGroupDetails[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getSkillGroupsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() clientId: number;
  @Input() clientLobGroupId: number;
  @Input() skillGroupId: number;
  @Input() hierarchy: boolean;
  @Output() skillGroupSelected = new EventEmitter();

  constructor(
    private skillGroupService: SkillGroupService,
  ) { }

  ngOnInit(): void {
    if (!this.hierarchy) {
      this.subscribeToSkillGroups();
    } else {
      this.skillGroupItemsBuffer = [];
      this.totalItems = 0;
    }
    this.subscribeToSearching();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  ngOnChanges() {
    if (this.clientLobGroupId) {
      this.pageNumber = 1;
      this.subscribeToSkillGroups();
    } else {
      this.skillGroupItemsBuffer = [];
      this.totalItems = 0;
    }
  }

  onSkillGroupScrollToEnd() {
    this.fetchMoreSkillGroups();
  }

  onSkillGroupScroll({ end }) {
    if (this.loading || this.skillGroupItemsBufferSize <= this.skillGroupItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.skillGroupItemsBuffer.length) {
      this.fetchMoreSkillGroups();
    }
  }

  onSkillGroupChange(event: SkillGroupDetails) {
    this.skillGroupSelected.emit(event?.id);
  }

  clearSkillGroupValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToSkillGroups();
  }

  private fetchMoreSkillGroups() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToSkillGroups(true);
    }
  }

  private subscribeToSkillGroups(needBufferAdd?: boolean) {
    this.loading = true;
    this.getSkillGroupsSubscription = this.getSkillGroups(this.dropdownSearchKeyWord).subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.skillGroupItemsBuffer = needBufferAdd ? this.skillGroupItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getSkillGroupsSubscription);
  }

  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getSkillGroups(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.skillGroupItemsBuffer = response.body;
      }
    }, (error) => {
      console.log(error);
    });

    this.subscriptions.push(this.typeAheadValueSubscription);
  }

  private setPaginationValues(response: any) {
    const paging = JSON.parse(response.headers.get('x-pagination'));
    if (paging) {
      this.totalItems = paging.totalCount;
      this.totalPages = paging.totalPages;
    }
  }

  private getQueryParams(searchkeyword?: string) {
    const queryParams = new SkillGroupQueryParameters();
    queryParams.clientId = this.clientId ?? undefined;
    queryParams.clientLobGroupId = this.clientLobGroupId ?? undefined;
    queryParams.pageSize = this.skillGroupItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.skipPageSize = false;
    queryParams.orderBy = undefined;
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getSkillGroups(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    if(this.dropdownSearchKeyWord !== queryParams.searchKeyword) {
      this.pageNumber = 1;
      queryParams.pageNumber = 1;
    }
    this.dropdownSearchKeyWord = queryParams.searchKeyword;
    return this.skillGroupService.getSkillGroups(queryParams);
  }

}
