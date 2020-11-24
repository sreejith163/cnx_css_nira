// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  sso: {
    authBaseUrl: 'https://auth-api.concentrix.com/employee/authenticate?attributes[0]=mail&attributes[1]=uid&attributes[2]=displayname&attributes[3]=employeeid&client=',
    authAppToken: 'w6VEeeJ9CdAuCWh8ORi702Es400EXcMnVf1'
  },
  services: {
    adminService: 'https://localhost:44318/api/v1',
    setupService: 'https://localhost:44398/api/v1',
    schedulingService: 'https://localhost:44393/api/v1'
  },
  settings: {
    applicationUrl: 'http://localhost:4200',
    sessionName: 'session',
    cookiePath: '/'
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
