import { Component } from '@angular/core';
import { SegmentData, LocationData, ObjectData } from '../Segment/data';

@Component({
    selector: 'view',
    templateUrl: './view.component.html'
})

export class ViewComponent {
    segments: SegmentData[][];

    constructor() {
        let segment = new SegmentData();
        segment.type = 'grass';
        segment.location = new LocationData();
        segment.location.i = 0;
        segment.location.j = 0;
        segment.location.leftx = 0;
        segment.location.upy = 0;
        segment.location.rightx = 10;
        segment.location.downy = 10;
        let object = new ObjectData();
        object.id = 'asdf@asdf.ru';
        object.x = 3.85;
        object.y = 7.4;
        segment.objects = [object];

        this.segments = [
            [segment, segment, segment],
            [segment, segment, segment],
            [segment, segment, segment]
        ];
    }

    private onclick(e): void {
        //let tileRect = e.currentTarget.getBoundingClientRect();
        //let clickedX = e.clientX - tileRect.left; //+right
        //let clickedY = e.clientY - tileRect.top; //+bottom
        //e.currentTarget.width // +height

        //alert(`${clickedX} ${clickedY} ${this.data.i}`);

        alert('click');
    }
}