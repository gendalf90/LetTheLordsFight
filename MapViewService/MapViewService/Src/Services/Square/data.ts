export class LocationData {
    x: number;
    y: number;
}

export class ObjectData {
    id: string;
    location: LocationData;
}

export class SegmentData {
    i: number;
    j: number;
    type: string;
    leftx: number;
    rightx: number;
    upy: number;
    downy: number;
}

export class SquareData {
    segments: SegmentData[];
    objects: ObjectData[];
}