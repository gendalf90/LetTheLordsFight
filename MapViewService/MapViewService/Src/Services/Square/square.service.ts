import { Injectable } from '@angular/core';
import { ConfigurationService } from '../Configuration/configuration.service';
import { UserService } from '../User/user.service';
import { ObjectData } from '../User/data';
import { SquareData } from './data';

var axios = require('axios');

@Injectable()
export class SquareService {

    constructor(private configuration: ConfigurationService, private user: UserService) {
    }

    public async getData(): Promise<SquareData> {
        let userObject = await this.user.getCurrentObject();
        let square = await this.getSquare(userObject.segment.i, userObject.segment.j);
        this.addUserObjectInSquareIfNotExist(userObject, square);
        return square;
    }

    private async getSquare(i: number, j: number): Promise<SquareData> {
        let configuration = this.configuration.getData();
        let response = await axios.get(`${configuration.api}api/v1/map/segments/square5x5/i/${i}/j/${j}`);
        return response.data;
    }

    private addUserObjectInSquareIfNotExist(userObject: ObjectData, square: SquareData) {
        let isExist = square.objects.some(object => object.id == userObject.id);

        if (!isExist) {
            square.objects.push(userObject);
        }
    }
}