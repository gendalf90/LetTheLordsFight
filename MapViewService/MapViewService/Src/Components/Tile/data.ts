export enum TileImage { Empty, Grass, Knight };

export class TileData {
    i: number;
    j: number;
    background: TileImage;
    object: TileImage;
}