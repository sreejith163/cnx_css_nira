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

const modules = [BrowserModule, BrowserAnimationsModule, CommonModule, HttpClientModule];

const providers = [AuthGuard, AuthService, CookieService, HttpBaseService, TokenInterceptorProviders, UnAuthorizedInterceptorProviders];

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
