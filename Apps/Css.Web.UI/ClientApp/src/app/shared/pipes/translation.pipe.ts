import { Pipe, PipeTransform } from '@angular/core';
import { CssLanguages } from '../enums/css-languages.enum';
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
      const language = CssLanguages.English;
      const translationData = args[1] as TranslationDetails[];
      const controlName = translationData?.find(x => x.variableName === controId && x.languageId === language);
      return controlName?.translation ?? defaultText;
    }

    return defaultText;
  }
}
