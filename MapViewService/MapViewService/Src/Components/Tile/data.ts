export enum TileImage { Empty, Grass, Knight, User, Forest };

export class TileData {
    i: number;
    j: number;
    leftx: number;
    rightx: number;
    upy: number;
    downy: number;
    borderLeft: boolean;
    borderRight: boolean;
    borderUp: boolean;
    borderDown: boolean;
    background: TileImage;
    object: TileImage;
}