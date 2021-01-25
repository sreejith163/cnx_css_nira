import { Routes } from '@angular/router';
import { CallbackComponent } from './callback.component';
import { AuthGuard } from './core/guards/auth.guard';
import { LanguagePreferenceResolver } from './shared/resolvers/language-preference.resolver';

export const AppRoutes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadChildren: () => import('./modules/login/login.module').then(m => m.LoginModule)
  },
  {
    path: 'callback',
    component: CallbackComponent
  },
  {
    path: 'home',
    loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule),
    canActivate: [AuthGuard],
    runGuardsAndResolvers: 'always',
    resolve: { languagePreference: LanguagePreferenceResolver }
  }
];
