import { AbstractControl, ValidationErrors } from '@angular/forms';
import { Constants } from './constants.util';

export class CustomValidators {

    static isValidEmail(control: AbstractControl): ValidationErrors | null {
        const value = control.value;
        return (!Constants.EmailRegex.test(String(value).toLowerCase())) ? { invalid: true } : null;
    }

    static fromToDate(fromDateField: string, toDateField: string, errorName: string = 'dateRangeError') {
        return (formGroup: AbstractControl): { [key: string]: boolean } | null => {
            const startRange = formGroup.get(fromDateField) ? formGroup.get(fromDateField).value : null;
            const endRange = formGroup.get(toDateField) ? formGroup.get(toDateField).value : null;
            const fromDate = this.formattedDate(startRange);
            const toDate =  this.formattedDate(endRange);
            if ((fromDate !== null && toDate !== null)
             && (new Date(fromDate) > new Date(toDate))) {
                return {[errorName]: true};
            }
            return null;
        };
    }

    static rangeValidator(start: string, to: string, errorName: string = '') {
        return (formGroup: AbstractControl): { [key: string]: boolean } | null => {
            const startRange = formGroup.get(start) ? formGroup.get(start).value : null;
            const endRange = formGroup.get(to) ? formGroup.get(to).value : null;
            if (startRange !== null && endRange !== null) {
                if ( +startRange >= +endRange) {
                   return { rangeError: true };
                }
                return null;
            }
            return null;
        };
    }

    static cannotContainSpace(control: AbstractControl): ValidationErrors | null {
        const value = control.value;
        return (value && value.toString().trim().length === 0) ? { required: true } : null;
    }

    static formattedDate(date) {
        let formattedDate = null;
        if (date) {
            formattedDate = `${date.year}-${date.month}-${date.day}`;
        }
        return formattedDate;
    }
}
