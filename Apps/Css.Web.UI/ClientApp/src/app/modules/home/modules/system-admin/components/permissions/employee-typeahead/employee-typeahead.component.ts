import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Constants } from 'src/app/shared/util/constants.util';
import { Permission } from '../../../models/permission.model';
import { PermissionsService } from '../../../services/permissions.service';

@Component({
  selector: 'app-employee-typeahead',
  templateUrl: './employee-typeahead.component.html',
  styleUrls: ['./employee-typeahead.component.scss']
})
export class EmployeeTypeAheadComponent implements OnInit {
  loading = false;
  pageNumber = 1;
  employeeItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  totalItems: number;
  totalPages: number;
  searchKeyWord = '';

  employeeItemsBuffer: Permission[] = [];
  translationValues = Constants.permissionsTranslationValues;
  typeAheadInput$ = new Subject<string>();

  @Input() permissionForm: FormGroup;
  @Input() updatePermissionData: Permission[];

  @Output() employeeSelected = new EventEmitter();

  constructor(
    private permissionService: PermissionsService
  ) { }

  ngOnInit(): void {
    this.subscribeToEmployeesWithoutPermissions();
    this.subscribeToSearching();
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
    const employee = new Permission();
    employee.employeeId = event?.employeeId;
    employee.firstName = event?.firstName;
    this.employeeSelected.emit(employee);
  }

  clearSelectedValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToEmployeesWithoutPermissions();
  }

  getItems() {
    if (this.updatePermissionData.length > 0) {
      return this.updatePermissionData;
    } else {
      return this.employeeItemsBuffer;
    }
  }

  private fetchMoreEmployees() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToEmployeesWithoutPermissions(true);
    }
  }

  private subscribeToEmployeesWithoutPermissions(needBufferAdd?: boolean) {
    this.loading = true;
    this.getEmployees().subscribe(
      response => {
        if (response) {
          this.employeeItemsBuffer = needBufferAdd ? this.employeeItemsBuffer.concat(response) : response;
          this.totalItems = this.employeeItemsBuffer.length;
        }
        this.loading = false;
      }, err => this.loading = false);

  }

  private subscribeToSearching() {
    this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getEmployees(term))
    ).subscribe(response => {
      if (response) {
        this.employeeItemsBuffer = response;
      }
    }, (error) => {
      console.log(error);
    });

  }

  private getEmployees(searchKeyword?: string) {
    return this.permissionService.getEmployees(searchKeyword);
  }

}
