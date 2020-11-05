import { Injectable } from '@angular/core';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { Constants } from 'src/app/shared/util/constants.util';

@Injectable()
export class TranslationService {

  language = ['English', 'Chinese', 'Portugese', 'Spanish', 'Tamil'];
  translationValueList: Translation[] = [];

  constructor() {
    this.createTranslationList();
  }

  getTablePageSizeList() {
    const tablePageSize: PaginationSize[] = [
      {
        count: 5,
        text: '5/Page'
      },
      {
        count: 10,
        text: '10/Page'
      },
      {
        count: 15,
        text: '15/Page'
      },
      {
        count: 20,
        text: '20/Page'
      }
    ];

    return tablePageSize;
  }

  createTranslationList() {
    Constants.schedulingCodeTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.agentCategoriesTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.translationTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.clientNameTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.clientLOBGroupTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.skillGroupsTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.skillTagsTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.agentSchedulingGroupTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.agentAdminTranslationValues.forEach(value => this.translationValueList.push(value));
    Constants.schedulingGridTranslationValues.forEach(value => this.translationValueList.push(value));
  }

  addTranslation(translateData: Translation) {
    this.translationValueList.push(translateData);
  }

  updateTranslation(translateData: Translation) {
    const translateDataIndex = this.translationValueList.findIndex(
      (x) =>
        x.variableId === translateData.variableId &&
        x.menu === translateData.menu
    );
    if (translateDataIndex !== -1) {
      this.translationValueList[translateDataIndex] = translateData;
    }
  }

  deleteTranslation(translateData: Translation) {
    const translateDataIndex = this.translationValueList.findIndex(
      (x) =>
        x.variableId === String(translateData.variableId) && x.menu === translateData.menu
    );
    if (translateDataIndex !== -1) {
      this.translationValueList.splice(translateDataIndex, 1);
    }
  }

  getTranslationList() {
    return this.translationValueList;
  }

  getTranslationItem(language: string, menu: string, variable: string) {
    const translateDataIndex = this.translationValueList.findIndex(
      (x) =>
        x.language === language &&
        x.menu === menu &&
        x.variableId === variable
    );
    if (translateDataIndex !== -1) {
      return this.translationValueList[translateDataIndex];
    }
  }
}
