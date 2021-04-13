import { CommonModule } from '@angular/common';
import { NgModule, ModuleWithProviders, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from './services/auth.service';

import { EnsureModuleLoadedOnceGuard } from './guards/ensure-module-loaded-once.guard';
import { AuthGuard } from './guards/auth.guard';

import { TokenInterceptorProviders } from './interceptors/token.interceptor';
import { UnAuthorizedInterceptorProviders } from './interceptors/unauthorized.interceptor';
import { HttpBaseService } from './services/http-base.service';
import { PermissionsService } from '../modules/home/modules/system-admin/services/permissions.service';
import { PermissionsGuard } from './guards/permissions.guard';
import { NgxCsvParserModule } from 'ngx-csv-parser';
import { ToastrModule } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';

const modules = [BrowserModule, 
  FormsModule, 
  BrowserAnimationsModule,
  ToastrModule.forRoot({
    timeOut: 2000,
    positionClass: 'toast-top-right'
  }) , CommonModule, HttpClientModule,  NgxCsvParserModule];

const providers = [PermissionsService, PermissionsGuard, AuthGuard, AuthService, CookieService, HttpBaseService,
                   TokenInterceptorProviders, UnAuthorizedInterceptorProviders];

@NgModule({
  imports: [modules],
  declarations: [],
  exports: [modules]
})

export class CoreModule extends EnsureModuleLoadedOnceGuard {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    super(parentModule);
  }

  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers
    };
  }
}
