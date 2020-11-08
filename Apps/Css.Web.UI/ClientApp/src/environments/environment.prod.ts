export const environment = {
  production: true,
  sso: {
    authBaseUrl: 'https://auth-api.concentrix.com/employee/authenticate?attributes[0]=mail&attributes[1]=uid&attributes[2]=displayname&attributes[3]=employeeid&client=',
    authAppToken: 'FGxyYfqQ3ht2omL0ARj4bySSa1rgrOeciUf'
  },
  services: {
    schedulingService: 'http://10.87.221.6:4202/api/v1'
  },
  settings: {
    applicationUrl: 'http://10.87.222.56:4200',
    sessionName: 'session',
    cookiePath: '/'
  }
};
