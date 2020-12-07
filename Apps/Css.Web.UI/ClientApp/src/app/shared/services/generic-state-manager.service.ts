import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { KeyValue } from '../models/key-value.model';

@Injectable()
export class GenericStateManagerService {

  language: KeyValue;

  userLanguageChanged = new BehaviorSubject<number>(undefined);

  constructor() { }

  getCurrentLanguage() {
    return this.language;
  }

  setCurrentLanguage(language: KeyValue) {
    this.language = language;
    this.userLanguageChanged.next(language.id);
  }
}
