import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { KeyValue } from '../models/key-value.model';

@Injectable()
export class GenericStateManagerService {

  userLanguageChanged = new BehaviorSubject<number>(undefined);

  constructor() { }

  getCurrentLanguage() {
    const language = localStorage.getItem('language');
    return JSON.parse(language) as KeyValue;
  }

  setCurrentLanguage(language: KeyValue) {
    localStorage.setItem('language', JSON.stringify(language));
    this.userLanguageChanged.next(language.id);
  }

  getLanguage() {
    const language = localStorage.getItem('lang');
    return JSON.parse(language);
  }

  setLanguage(lang) {
    localStorage.setItem('lang', JSON.stringify(lang));
    this.userLanguageChanged.next(lang);
  }

}
