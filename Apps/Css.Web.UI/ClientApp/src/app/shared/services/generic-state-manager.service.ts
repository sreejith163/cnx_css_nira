import { Injectable } from '@angular/core';
import { KeyValue } from '../models/key-value.model';

@Injectable()
export class GenericStateManagerService {

  language: KeyValue;

  constructor() { }

  getCurrentLanguage() {
    return this.language;
  }

  setCurrentLanguage(language: KeyValue) {
    this.language = language;
  }
}
