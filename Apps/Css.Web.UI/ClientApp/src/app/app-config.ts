// app-config.ts

import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

export interface Config {
    sso: {
        authBaseUrl: string,
        authAppToken: string
    },
    services: {
        gatewayService: string
    },
    settings: {
        applicationUrl: string,
        sessionName: string,
        cookiePath: string
    }
}

declare var window: any;

@Injectable()
export class AppConfig {
    constructor(private readonly http: HttpClient) {}
    public load() {
        return new Promise((resolve, reject) => {
            let request: any = null;

            switch(location.hostname) { 
                case "localhost": { 
                    request = this.http.get<Config>('assets/config.test.json');
                    break; 
                } 
                case "127.0.0.1": { 
                    request = this.http.get<Config>('assets/config.test.json');
                    break; 
                }
                
                case "css-dev.concentrix.com": {
                    request = this.http.get<Config>('assets/config.dev.json');
                    break; 
                }
                
                case "css-uat.concentrix.com": {
                    request = this.http.get<Config>('assets/config.uat.json');
                    break; 
                }

                default: { 
                    request = this.http.get<Config>('assets/config.dev.json');
                    break; 
                } 
            } 

            // // use config.test if on local environment
            // if (location.hostname === "localhost" || location.hostname === "127.0.0.1"){
            //     request = this.http.get<Config>('assets/config.test.json');
            // }else{
            // // otherwise use the config set on Docker env
            //     request = this.http.get<Config>('assets/config.json');
            // }

            // process the request from config.json
            if (request) {
                request.subscribe(config => {
                    window.config = config;
                    resolve(true);
                });
            } else {
                console.error('Env config file "config.json" is not valid');
                resolve(true);
            }
        });
    }
}

export function initConfig(config: AppConfig) {
    return () => config.load();
}
