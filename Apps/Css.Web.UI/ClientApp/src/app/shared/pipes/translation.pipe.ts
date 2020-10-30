import { Pipe, PipeTransform } from '@angular/core';
import { Translation } from '../models/translation.model';

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
      const language = 'English';
      const translationData = args[1] as Translation[];
      const controlName = translationData?.find(x => x.variableId === controId && x.language === language);
      return controlName?.translation ?? defaultText;
    }

    return defaultText;
  }
}
