/** Outer dependencies */
import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

/** From '@ng-bootstrap/ng-bootstrap/util/util'; */
function isString(value: any): value is string {
  return typeof value === 'string';
}
function isInteger(value: any): value is number {
  return typeof value === 'number' && isFinite(value) && Math.floor(value) === value;
}

@Injectable()
export class NgbDateToIsoStringAdapter extends NgbDateAdapter<string> {
  /**
   * Converts user-model date into an NgbDateStruct for internal use in the library
   */
  public fromModel(dateString: string): NgbDateStruct | null {
    const date = new Date(dateString);
    const isValidDate = (!isNaN(date.valueOf()));

    if (!dateString || !isString(dateString) || !isValidDate) {
      return null;
    }

    return { year: date.getUTCFullYear(), month: date.getUTCMonth() + 1, day: date.getUTCDate() };
  }

  /**
   * Converts internal date value NgbDateStruct to user-model date
   * The returned type is supposed to be of the same type as fromModel() input-value param
   */
  public toModel(date: NgbDateStruct): string | null {
    if (date && isInteger(date.year) && isInteger(date.month) && isInteger(date.day)) {
      return new Date(Date.UTC(date.year, date.month - 1, date.day)).toISOString();
    }

    return null;
  }
}
