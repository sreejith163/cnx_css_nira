import { Directive, ElementRef, HostListener } from "@angular/core";
@Directive({
    selector: '[employeeid]'
})
export class SystemAdminEmployeeIdDirective {
    inputElement: HTMLElement;
    constructor(public el: ElementRef){
        this.inputElement = el.nativeElement;
    }

    @HostListener('keydown',['$event'])
    onkeydown(event:KeyboardEvent){
        var regex = /[^A-Za-z0-9]/gi;
        var regex2 = /\B[a-zA-Z]/gi;

        if(this.el.nativeElement.value.match(regex)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/[^A-Za-z0-9]/gi,"")
            event.preventDefault();
        } 
        else if(this.el.nativeElement.value.match(regex2)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/\B[a-zA-Z]/gi,"")
            event.preventDefault();
        } 
    }


    @HostListener('keyup',['$event'])
    onkeyup(event:KeyboardEvent){
        var regex = /[^A-Za-z0-9]/gi;
        var regex2 = /\B[a-zA-Z]/gi;

        if(this.el.nativeElement.value.match(regex)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/[^A-Za-z0-9]/gi,"")
            event.preventDefault();
        } 
        else if(this.el.nativeElement.value.match(regex2)){
            this.el.nativeElement.value = this.el.nativeElement.value.replace(/\B[a-zA-Z]/gi,"")
            event.preventDefault();
        } 
    }
}