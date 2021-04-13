
import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appOnlynumber]'
})
export class OnlynumberDirective {

    private navigationKeys = [
        'Backspace',
        'Delete',
        'Tab',
        'Escape',
        'Enter',
        'Home',
        'End',
        'ArrowLeft',
        'ArrowRight',
        'Clear',
        'Copy',
        'Paste'
    ];
    inputElement: HTMLElement;

    constructor(public el: ElementRef) {
        this.inputElement = el.nativeElement;
    }
    @HostListener('propertychange', ['$event'])
    onPropertyChange(e: KeyboardEvent) {

        var reg = /^0+/gi;
        console.log(this.el.nativeElement.innerText)
        // if (this.el.nativeElement.innerText.match(reg)){
        //     this.el.nativeElement.innerText = this.el.nativeElement.innerText.replace(reg, '');
        // }
        if(this.el.nativeElement.innerText == ""){
            this.el.nativeElement.innerText = 0
           } 
 
}
// @HostListener('change', ['$event'])
// onChange(e: KeyboardEvent) {

//     var reg = /^0+/gi;
//     if (this.el.nativeElement.innerText.match(reg)){
//         this.el.nativeElement.innerText = this.el.nativeElement.innerText.replace(this.el.nativeElement.innerText.charAt(0),"");
//         e.preventDefault();
//     }

// }
@HostListener('keyup', ['$event'])
onKeyPress(e: KeyboardEvent) {
    var reg = /^0+/gi;
    if (this.el.nativeElement.innerText.match(reg)){
        this.el.nativeElement.innerText = this.el.nativeElement.innerText.replace(this.el.nativeElement.innerText.charAt(0),"");
        e.preventDefault();
    }
}
    @HostListener('keydown', ['$event'])
    onKeyDown(e: KeyboardEvent) {
        // var reg = /^0+/gi;
        // console.log(this.el.nativeElement.innerText)
        // if (this.el.nativeElement.innerText.match(reg)){
        //     this.el.nativeElement.innerText = this.el.nativeElement.innerText.replace(this.el.nativeElement.innerText.charAt(0),"");
        //     e.preventDefault();
        // }
     
        if (
            this.navigationKeys.indexOf(e.key) > -1 || // Allow: navigation keys: backspace, delete, arrows etc.
            (e.key === 'a' && e.ctrlKey === true) || // Allow: Ctrl+A
            (e.key === 'c' && e.ctrlKey === true) || // Allow: Ctrl+C
            (e.key === 'v' && e.ctrlKey === true) || // Allow: Ctrl+V
            (e.key === 'x' && e.ctrlKey === true) || // Allow: Ctrl+X
            (e.key === 'a' && e.metaKey === true) || // Allow: Cmd+A (Mac)
            (e.key === 'c' && e.metaKey === true) || // Allow: Cmd+C (Mac)
            (e.key === 'v' && e.metaKey === true) || // Allow: Cmd+V (Mac)
            (e.key === 'x' && e.metaKey === true) // Allow: Cmd+X (Mac)
        ) {
            // let it happen, don't do anything
            return;
        }
        const position = this.el.nativeElement.innerText.length;

        if (this.el.nativeElement.innerText == 0 && this.el.nativeElement.innerText.which == 48 ){
            this.el.nativeElement.innerText = "";
         }
        // Ensure that it is a number and stop the keypress
        if (
            (e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && 
            (e.keyCode < 96 || e.keyCode > 105) || (position > 10)
        ) {
            e.preventDefault();
        }
        

    }
 
    @HostListener('blur', ['$event'])
    blur(event: ClipboardEvent) {
        const position = this.el.nativeElement.innerText.length;
        var reg = /^0+/gi;
    //   if(this.el.nativeElement.innerText.match(reg)){
    //     this.el.nativeElement.innerText = this.el.nativeElement.innerText.replace(this.el.nativeElement.innerText.charAt(0),"");
        
    //   }
    //   else 
    if(this.el.nativeElement.innerText == ""){
       this.el.nativeElement.innerText = 0
      } 
    }
    // @HostListener('paste', ['$event'])
    // paste(event: ClipboardEvent) {
    //     const position = this.el.nativeElement.innerText.length;
    //     console.log(position)
    //   if(position > 10){
    //     this.el.nativeElement.innerText = 0
    //   }
     
    // }

    @HostListener('paste', ['$event'])
    onPaste(event: ClipboardEvent) {
       
        var reg = /^0+/gi;
       
        if (this.el.nativeElement.innerText.match(reg)){
            event.preventDefault();
        }
        const pastedInput: string = event.clipboardData
            .getData('text/plain')
            .slice(0, 10)
            .replace(/\D/g, ''); // get a digit-only string
        document.execCommand('insertText', false, pastedInput);


        const position = this.el.nativeElement.innerText;
        if(position.length > 10){
            this.el.nativeElement.innerText = "";
        }
       
        event.preventDefault();
     
    }

    @HostListener('drop', ['$event'])
    onDrop(event: DragEvent) {
        event.preventDefault();
        const textData = event.dataTransfer.getData('text').replace(/\D/g, '');
        this.inputElement.focus();
        document.execCommand('insertText', false, textData);
    }


}