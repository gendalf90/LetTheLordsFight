export enum SegmentType { Grass }

export class SegmentData {
    i: number;
    j: number;
    type: SegmentType;
    location: LocationData;
    objects: ObjectData[];
}

export class LocationData {
    leftx: number;
    rightx: number;
    upy: number;
    downy: number;
}

export class ObjectData {
    id: string;
    x: number;
    y: number;
}