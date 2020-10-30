import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'horizontalScroll',
  pure: true
})
export class HorizontalScrollPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    const defaultIcons = value as Array<string>;
    const startIndex = args[0];
    const endIndex = args[1];
    const count = args[2];
    if (startIndex <= -1) {
      const icons = defaultIcons.slice(0, Number(count));
      return icons;
    } else if (endIndex > defaultIcons.length) {
      const icons = defaultIcons.slice(defaultIcons.length - Number(count), defaultIcons.length);
      return icons;
    }
    else if (startIndex <= (defaultIcons.length - Number(count)) && endIndex <= defaultIcons.length) {
      const icons = defaultIcons.slice(Number(startIndex), Number(endIndex));
      return icons;
    }

  }

}
