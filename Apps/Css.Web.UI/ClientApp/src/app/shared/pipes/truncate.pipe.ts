import { Pipe, PipeTransform } from '@angular/core';
import { TextTruncateOptions } from '../interfaces/truncate-options';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {

  transform(textContent: string, options: TextTruncateOptions): string {
    if (textContent.length >= options.sliceEnd) {
      let truncatedText = textContent.slice(options.sliceStart, options.sliceEnd);
      if (options.prepend) { truncatedText = `${options.prepend}${truncatedText}`; }
      if (options.append) { truncatedText = `${truncatedText}${options.append}`; }
      return truncatedText;
    }
    return textContent;
  }
}
