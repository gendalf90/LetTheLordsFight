﻿export enum TileImage { Empty, Grass, Knight, User, Forest };

export class TileData {
    i: number;
    j: number;
    leftx: number;
    rightx: number;
    upy: number;
    downy: number;
    background: TileImage;
    object: TileImage;
}