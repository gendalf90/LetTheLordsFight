import { Injectable } from '@angular/core';
import { UserData, ObjectData, LocationData } from './data';
import { ConfigurationService } from '../Configuration/configuration.service';

var axios = require('axios');

@Injectable()
export class UserService {

    constructor(private configuration: ConfigurationService) {
    }

    public getCurrentData(): UserData {
        return {
            id: this.currentId
        };
    }

    public async getCurrentObject(): Promise<ObjectData> {
        let configuration = this.configuration.getData();
        let response = await axios.get(`${configuration.api}api/v1/map/objects/${this.currentId}`);
        return response.data;
    }

    public async currentMoveTo(location: LocationData) {
        let configuration = this.configuration.getData();
        await axios.patch(`${configuration.api}api/v1/map/objects/${this.currentId}`, { destination: location });
    }

    private get currentId(): string {
        return sessionStorage.getItem('id');
    }
}