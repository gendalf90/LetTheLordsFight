export enum SegmentType { Grass }

export class ObjectData {
    id: string;
    x: number;
    y: number;
}

export class SegmentData {
    i: number;
    j: number;
    type: SegmentType;
    leftx: number;
    rightx: number;
    upy: number;
    downy: number;
    objects: ObjectData[];
}