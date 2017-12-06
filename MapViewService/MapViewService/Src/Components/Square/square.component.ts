import { Component } from '@angular/core';
import { SegmentData, SegmentType, ObjectData } from '../Segment/data';

@Component({
    selector: 'square',
    templateUrl: './square.component.html'
})
export class SquareComponent {
    private segments: SegmentData[];

    constructor() {
        let segment = new SegmentData();
        segment.type = SegmentType.Grass;
        segment.i = 0;
        segment.j = 0;
        segment.leftx = 0;
        segment.upy = 0;
        segment.rightx = 10;
        segment.downy = 10;
        let object = new ObjectData();
        object.id = 'asdf@asdf.ru';
        object.x = 3.85;
        object.y = 7.4;
        segment.objects = [object];

        this.segments = [segment];
    }
}