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
        var regex2 = /^0+/gi;

        if(this.el.nativeElement.value.match(regex)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/[^0-9]/gi,"")
            event.preventDefault();
        }
        else if(this.el.nativeElement.value.match(regex2)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(this.el.nativeElement.value.charAt(0),"")
            event.preventDefault();
        }
    }


    @HostListener('keyup',['$event'])
    onkeyup(event:KeyboardEvent){
        var regex = /[^0-9]/gi;
        var regex2 = /^0+/gi;

        if(this.el.nativeElement.value.match(regex)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/[^0-9]/gi,"")
            event.preventDefault();
        }
        else if(this.el.nativeElement.value.match(regex2)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(this.el.nativeElement.value.charAt(0),"")
            event.preventDefault();
        }
    }
}