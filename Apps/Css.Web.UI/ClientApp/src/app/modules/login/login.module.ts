import { NgModule } from '@angular/core';
import { LoginRoutingModule } from './login-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { LoginComponent } from './components/login/login.component';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';

const modules = [SharedModule, LoginRoutingModule, NgbCarouselModule];
const components = [LoginComponent];
const providers = [];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class LoginModule { }
