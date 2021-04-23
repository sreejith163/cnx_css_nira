import { Directive, ElementRef, HostListener } from "@angular/core";

@Directive({
    selector: '[integeronly]'
})

export class SystemAdminIntegerDirective {
    inputElement: HTMLElement;
    constructor(public el: ElementRef){
        this.inputElement = el.nativeElement;
    }

    @HostListener('keydown',['$event'])
    onkeydown(event:KeyboardEvent){
        var regex = /[^0-9]/gi;

        if(this.el.nativeElement.value.match(regex)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/[^0-9]/gi,"")
            event.preventDefault();
        }
    }


    @HostListener('keyup',['$event'])
    onkeyup(event:KeyboardEvent){
        var regex = /[^0-9]/gi;

        if(this.el.nativeElement.value.match(regex)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/[^0-9]/gi,"")
            event.preventDefault();
        }
    }
}