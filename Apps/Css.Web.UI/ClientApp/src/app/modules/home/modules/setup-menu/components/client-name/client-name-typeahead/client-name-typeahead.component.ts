import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ClientBaseModel } from '../../../models/client-base.model';
import { ClientNameQueryParameters } from '../../../models/client-name-query-parameters.model';
import { ClientService } from '../../../services/client.service';

@Component({
  selector: 'app-client-name-typeahead',
  templateUrl: './client-name-typeahead.component.html',
  styleUrls: ['./client-name-typeahead.component.css']
})
export class ClientNameTypeAheadComponent implements OnInit, OnDestroy {

  pageNumber = 1;
  clientItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  loading = false;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  selectedClientId: any;

  clientItemsBuffer: ClientBaseModel[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getClientNamesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() clientId: number;
  @Output() clientSelected = new EventEmitter();

  constructor(
    private clientService: ClientService,
  ) { }

  ngOnInit(): void {
    this.subscribeToClients();
    this.subscribeToSearching();
    if (this.clientId)
    {
      this.selectedClientId = this.clientId;
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  onClientScrollToEnd() {
    this.fetchMoreClients();
  }

  onClientScroll({ end }) {
    if (this.loading || this.clientItemsBufferSize <= this.clientItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.clientItemsBuffer.length) {
      this.fetchMoreClients();
    }
  }

  onClientChange(event: ClientBaseModel) {
    this.clientSelected.emit(event?.id);
  }

  clearSelectedValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToClients();
  }

  private fetchMoreClients() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToClients(true);
    }
  }

  private subscribeToClients(needBufferAdd?: boolean) {
    this.loading = true;
    this.getClientNamesSubscription = this.getClients().subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.clientItemsBuffer = needBufferAdd ? this.clientItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getClientNamesSubscription);
  }

  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getClients(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.clientItemsBuffer = response.body;
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
    const queryParams = new ClientNameQueryParameters();
    queryParams.pageSize = this.clientItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.orderBy = null;
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getClients(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    return this.clientService.getClients(queryParams);
  }
}
