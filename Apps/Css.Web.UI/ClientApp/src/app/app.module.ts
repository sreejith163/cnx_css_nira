import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';

import { AppComponent } from './app.component';
import { CallbackComponent } from './callback.component';

const modules = [CoreModule.forRoot(), SharedModule.forRoot(), AppRoutingModule];
const components = [AppComponent, CallbackComponent];
const providers = [];

@NgModule({
  declarations: components,
  imports: modules,
  providers,
  bootstrap: [AppComponent]
})
export class AppModule { }
