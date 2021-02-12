declare var window: any;

export class DynamicEnvironment {
    public get sso() { 
        return window.config.sso;
    }
    public get settings() {
        return window.config.settings;
    }
    public get services() {
        return window.config.services;
    }
}