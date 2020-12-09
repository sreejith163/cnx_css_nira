import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { KeyValue } from '../models/key-value.model';

@Injectable()
export class GenericStateManagerService {

  userLanguageChanged = new BehaviorSubject<number>(undefined);

  constructor() { }

  getCurrentLanguage() {
    return +localStorage.getItem('language');
  }

  setCurrentLanguage(language: KeyValue) {
    localStorage.setItem('language', String(language.id));
    this.userLanguageChanged.next(language.id);
  }
}
