export class UserData {
    id: string;
}

export class LocationData {
    x: number;
    y: number;
}

export class SegmentData {
    i: number;
    j: number;
}

export class ObjectData {
    id: string;
    location: LocationData;
    segment: SegmentData;
}