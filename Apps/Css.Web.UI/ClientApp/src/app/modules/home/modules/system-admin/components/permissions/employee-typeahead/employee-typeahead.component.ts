import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { Permission } from '../../../models/permission.model';
import { PermissionsService } from '../../../services/permissions.service';

@Component({
  selector: 'app-employee-typeahead',
  templateUrl: './employee-typeahead.component.html',
  styleUrls: ['./employee-typeahead.component.scss']
})
export class EmployeeTypeAheadComponent implements OnInit, OnDestroy {

  loading = false;
  pageNumber = 1;
  employeeItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  totalPages = 1;
  totalItems: number;
  searchKeyWord = '';

  employeeItemsBuffer: Permission[] = [];
  typeAheadInput$ = new Subject<string>();

  getEmployeesSubscription: any;
  typeAheadSubscription: any;
  subscriptions: any[] = [];

  @Input() employeeId: number;
  @Input() translationValues: Translation[];
  @Output() employeeSelected = new EventEmitter();
  @Output() employeeCleared = new EventEmitter();

  constructor(
    private permissionService: PermissionsService
  ) { }

  ngOnInit(): void {
    this.subscribeToEmployees();
    this.subscribeToSearching();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  onEmployeeScrollToEnd() {
    this.fetchMoreEmployees();
  }

  onEmployeeScroll({ end }) {
    if (this.loading || this.employeeItemsBufferSize <= this.employeeItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.employeeItemsBuffer.length) {
      this.fetchMoreEmployees();
    }
  }

  onEmployeeChange(event: Permission) {
    this.employeeSelected.emit(event);
  }

  clearSelectedEmployee() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.totalPages = 1;
    this.subscribeToEmployees();
    this.employeeCleared.emit();
  }

  private fetchMoreEmployees() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToEmployees(true);
    }
  }

  private setPaginationValues(response) {
    this.totalItems = response.length;
    this.totalPages = this.totalItems / this.totalPages;
  }

  private subscribeToEmployees(needBufferAdd?: boolean) {
    this.loading = true;
    this.getEmployeesSubscription = this.getEmployees().subscribe(
      response => {
        if (response) {
          this.employeeItemsBuffer = needBufferAdd ? this.employeeItemsBuffer.concat(response) : response;
          this.setPaginationValues(this.employeeItemsBuffer);
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getEmployeesSubscription);
  }

  private subscribeToSearching() {
    this.typeAheadSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getEmployees(term))
    ).subscribe(response => {
      if (response) {
        this.setPaginationValues(response);
        this.employeeItemsBuffer = response;
      }
    }, (error) => {
      console.log(error);
    });
    this.subscriptions.push(this.typeAheadSubscription);
  }

  private getQueryParams(searchkeyword?: string) {
    const queryParams = new QueryStringParameters();
    queryParams.pageSize = this.employeeItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.orderBy = null;
    queryParams.fields = 'id, firstName, lastName';

    return queryParams;
  }

  private getEmployees(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    return this.permissionService.getEmployees(queryParams);
  }
}
