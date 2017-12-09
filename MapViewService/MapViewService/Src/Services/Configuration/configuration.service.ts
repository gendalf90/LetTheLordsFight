import { Injectable } from '@angular/core';
import { ConfigurationData } from './data';

@Injectable()
export class ConfigurationService {

    public getData(): ConfigurationData {
        return {
            api: sessionStorage.getItem('api'),
            token: localStorage.getItem('token')
        };
    }
}