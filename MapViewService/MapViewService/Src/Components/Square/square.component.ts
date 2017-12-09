import { Component, OnInit } from '@angular/core';
import { SegmentData as SegmentComponentData, SegmentType as SegmentComponentType, ObjectData as ObjectComponentData } from '../Segment/data';
import { SquareService } from '../../Services/Square/square.service';
import { SquareData as SquareServiceData, SegmentData as SegmentServiceData, ObjectData as ObjectServiceData } from '../../Services/Square/data';

@Component({
    selector: 'square',
    templateUrl: './square.component.html'
})
export class SquareComponent implements OnInit {

    private data: SquareServiceData;
    private segments: SegmentComponentData[];

    constructor(private square: SquareService) {
    }

    async ngOnInit() {
        await this.loadData();
        this.setData();
    }

    private async loadData() {
        this.data = await this.square.getData();
    }

    private setData() {
        this.segments = this.createSegments(this.data);
    }

    private createSegments(square: SquareServiceData): SegmentComponentData[] {
        let segments = square.segments.map(segment => this.createSegment(segment));
        let objects = square.objects.map(object => this.createObject(object));
        this.mergeObjectsAndSegments(segments, objects);
        return segments;
    }

    private createSegment(segment: SegmentServiceData): SegmentComponentData {
        return {
            i: segment.i,
            j: segment.j,
            type: this.toSegmentType(segment.type),
            leftx: segment.leftx,
            rightx: segment.rightx,
            upy: segment.upy,
            downy: segment.downy,
            objects: []
        };
    }

    private createObject(object: ObjectServiceData): ObjectComponentData {
        return {
            id: object.id,
            x: object.location.x,
            y: object.location.y
        };
    }

    private mergeObjectsAndSegments(segments: SegmentComponentData[], objects: ObjectComponentData[]) {
        for (let segment of segments) {
            for (let object of objects) {
                this.addObjectInSegmentIfNeeded(segment, object);
            }
        }
    }

    private addObjectInSegmentIfNeeded(segment: SegmentComponentData, object: ObjectComponentData) {
        if (object.x > segment.leftx && object.x < segment.rightx && object.y > segment.upy && object.y < segment.downy) {
            segment.objects.push(object);
        }
    }

    private toSegmentType(type: string): SegmentComponentType {
        switch (type) {
            case 'Grass': {
                return SegmentComponentType.Grass;
            }
            case 'Forest': {
                return SegmentComponentType.Forest;
            }
            default: {
                return SegmentComponentType.Empty;
            }
        }
    }
}