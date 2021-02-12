// export const environment = {
//   production: true,
//   sso: {
//     authBaseUrl: 'https://auth-api.concentrix.com/employee/authenticate?attributes[0]=mail&attributes[1]=uid&attributes[2]=displayname&attributes[3]=employeeid&client=',
//     authAppToken: '9xxo7FxS7IdXQRQpBTaVQtmpgHTZBKxOEhN'
//   },
//   services: {
//     gatewayService: 'http://10.87.222.9:4201/api'
//   },
//   settings: {
//     applicationUrl: 'http://10.87.221.34:4200',
//     sessionName: 'session',
//     cookiePath: '/'
//   }
// };

import { DynamicEnvironment } from './dynamic-environment';
class Environment extends DynamicEnvironment {

  public production: boolean;
  public debugMode: boolean;
  constructor() {
    super();
    this.production = false;
  }
}

export const environment = new Environment();