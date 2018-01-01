import { Injectable } from '@angular/core';
import { UserData, ObjectData, LocationData } from './data';
import { ConfigurationService } from '../Configuration/configuration.service';

var axios = require('axios');

@Injectable()
export class UserService {

    private sendDestinationInterval: number = 1000;
    private destination: LocationData = null;

    constructor(private configuration: ConfigurationService) {
        this.startDestinationSynchronization();
    }

    private startDestinationSynchronization() {
        setInterval(async () => {
            await this.sendDestinationIfNeeded();
        }, this.sendDestinationInterval);
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

    public currentMoveTo(location: LocationData) {
        this.destination = location;
    }

    private async sendDestinationIfNeeded() {
        if (!this.destination) {
            return;
        }

        let configuration = this.configuration.getData();
        await axios.patch(`${configuration.api}api/v1/map/objects/${this.currentId}`, { destination: this.destination });
        this.destination = null;
    }

    private get currentId(): string {
        return sessionStorage.getItem('id');
    }
}