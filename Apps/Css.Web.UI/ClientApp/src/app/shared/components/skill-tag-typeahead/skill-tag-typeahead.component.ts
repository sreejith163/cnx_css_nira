import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { SkillTagBase } from 'src/app/modules/home/modules/setup-menu/models/skill-tag-base.model';
import { SkillTagQueryParams } from 'src/app/modules/home/modules/setup-menu/models/skill-tag-query-params.model';
import { SkillTagService } from 'src/app/modules/home/modules/setup-menu/services/skill-tag.service';

@Component({
  selector: 'app-skill-tag-typeahead',
  templateUrl: './skill-tag-typeahead.component.html',
  styleUrls: ['./skill-tag-typeahead.component.scss']
})
export class SkillTagTypeaheadComponent implements OnInit, OnDestroy, OnChanges {

  pageNumber = 1;
  skillTagItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  loading = false;

  skillTagItemsBuffer: SkillTagBase[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getSkillTagsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];


  @Input() clientId: number;
  @Input() clientLobGroupId: number;
  @Input() skillGroupId: number;
  @Input() skillTagId: number;
  @Output() skillTagSelected = new EventEmitter();

  constructor(
    private skillTagService: SkillTagService,
  ) { }

  ngOnInit(): void {
    this.subscribeToSkillTags();
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
    if (this.skillGroupId) {
      this.pageNumber = 1;
      this.subscribeToSkillTags();
    } else {
      this.skillTagItemsBuffer = [];
      this.totalItems = 0;
    }
  }

  onSkillTagScrollToEnd() {
    this.fetchMoreSkillTags();
  }

  onSkillTagScroll({ end }) {
    if (this.loading || this.skillTagItemsBufferSize <= this.skillTagItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.skillTagItemsBuffer.length) {
      this.fetchMoreSkillTags();
    }
  }

  onSkillTagChange(event: SkillTagBase) {
    this.skillTagSelected.emit(event?.id);
  }

  clearSkillTagValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToSkillTags();
  }

  private fetchMoreSkillTags() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToSkillTags(true);
    }
  }

  private subscribeToSkillTags(needBufferAdd?: boolean) {
    this.loading = true;
    this.getSkillTagsSubscription = this.getSkillTags().subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.skillTagItemsBuffer = needBufferAdd ? this.skillTagItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getSkillTagsSubscription);
  }

  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getSkillTags(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.skillTagItemsBuffer = response.body;
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
    const queryParams = new SkillTagQueryParams();
    queryParams.clientId = this.clientId ?? undefined;
    queryParams.clientLobGroupId = this.clientLobGroupId ?? undefined;
    queryParams.skillGroupId = this.skillGroupId ?? undefined;
    queryParams.pageSize = this.skillTagItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.skipPageSize = true;
    queryParams.orderBy = undefined;
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getSkillTags(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    return this.skillTagService.getSkillTags(queryParams);
  }

}
