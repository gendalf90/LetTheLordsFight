import { Injectable } from '@angular/core';
import { UserData, ObjectData } from './data';
import { ConfigurationService } from '../Configuration/configuration.service';

var axios = require('axios');

@Injectable()
export class UserService {

    constructor(private configuration: ConfigurationService) {
    }

    public getCurrentData(): UserData {
        return {
            id: sessionStorage.getItem('id')
        };
    }

    public async getCurrentObject(): Promise<ObjectData> {
        let id = sessionStorage.getItem('id');
        return await this.getObjectById(id);
    }

    private async getObjectById(id: string): Promise<ObjectData> {
        let configuration = this.configuration.getData();
        let response = await axios.get(`${configuration.api}api/v1/map/objects/${id}`);
        return response.data;
    }
}