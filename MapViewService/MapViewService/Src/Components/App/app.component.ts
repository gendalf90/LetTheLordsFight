import { Component } from '@angular/core';
import { SegmentData, SegmentType } from '../Segment/data';

@Component({
    selector: 'app',
    templateUrl: './app.component.html'
})

export class AppComponent {

    private data: SegmentData;

    constructor() {
        this.data = {
            i: 0,
            j: 0,
            type: SegmentType.Grass,
            location: {
                leftx: 0,
                rightx: 10,
                upy: 0,
                downy: 10
            },
            objects: [{
                id: 'asdf@asdf.ru',
                x: 3.5,
                y: 7.8
            }]
        };
    }
}