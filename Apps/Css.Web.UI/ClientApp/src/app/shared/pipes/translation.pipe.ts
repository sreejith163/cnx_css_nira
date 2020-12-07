import { Pipe, PipeTransform } from '@angular/core';
import { CssLanguage } from '../enums/css-language.enum';
import { TranslationDetails } from '../models/translation-details.model';

@Pipe({
  name: 'translation',
  pure: true
})
export class TranslationPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    const defaultText = value;
    const args1 = [...args];

    if (args1.length) {
      const controId = args[0];
      const translationData = args[1] as TranslationDetails[];
      const controlName = translationData?.find(x => x.variableName === controId);
      return controlName?.translation ?? defaultText;
    }

    return defaultText;
  }
}
