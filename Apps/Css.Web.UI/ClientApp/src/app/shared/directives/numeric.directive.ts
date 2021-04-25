import { Directive, ElementRef, HostListener } from '@angular/core';
@Directive({
  selector: '[numberOnly]'
})
export class NumericDirective {
  private regex: RegExp = new RegExp(/^\d*\.?\d{0,2}$/g);
  private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home', 'ArrowLeft', 'ArrowRight', 'Del', 'Delete'];
  constructor(private el: ElementRef) {
  }
  @HostListener('blur', ['$event'])
  blur(event: MouseEventInit) {
      const position = this.el.nativeElement.innerText.length;
    if(position == ""){
      this.el.nativeElement.innerText = '0.00';
    }
   
  }

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    // console.log(this.el.nativeElement.value);
    // Allow Backspace, tab, end, and home keys
    if (this.specialKeys.indexOf(event.key) !== -1) {
      return;
    }

    const current: string = this.el.nativeElement.innerText;

    //console.log(current);
    const position = this.el.nativeElement.innerText;
    const next: string = [current.slice(0, position), event.key === 'Decimal' ? '.' : event.key, current.slice(position)].join('');
    if (next && !String(next).match(this.regex)) {
      event.preventDefault();
    }
    if (position == 0 && event.which == 48 ){
      event.preventDefault();
   }

  }
}
