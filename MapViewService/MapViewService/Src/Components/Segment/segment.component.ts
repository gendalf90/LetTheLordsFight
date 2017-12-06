﻿import { Input, Component } from '@angular/core';
import { SegmentData, ObjectData, SegmentType } from './data';
import { TileData, TileImage } from '../Tile/data';

@Component({
    selector: 'segment',
    templateUrl: './segment.component.html'
})
export class SegmentComponent {

    private tilesSize: number = 5;
    private tiles: TileData[];
    private currentData: SegmentData;
    private currentTiles: TileData[][];

    constructor() {
        this.tiles = [];
    }

    @Input()
    set data(value: SegmentData) {
        this.currentData = value;
        this.initializeTiles();
        this.initializeObjects();
        this.createTiles();
    }

    private initializeTiles() {
        this.currentTiles = [];
        for (let i = 0; i < this.tilesSize; i++) {
            this.currentTiles[i] = [];
            for (let j = 0; j < this.tilesSize; j++) {
                this.currentTiles[i][j] = this.createTileData(i, j);
            }
        }
    }

    private createTileData(i: number, j: number): TileData {
        let locationIJ = this.createTileIJ(i, j);
        let locationXY = this.createTileXY(i, j);
        let images = this.createTileImages();
        return Object.assign({}, locationIJ, locationXY, images);
    }

    private createTileIJ(i: number, j: number) {
        return {
            i: this.currentData.i * this.tilesSize + i,
            j: this.currentData.j * this.tilesSize + j
        };
    }

    private createTileXY(i: number, j: number) {
        return {
            upy: this.currentData.upy + this.tileHeight * i,
            downy: this.currentData.upy + this.tileHeight * (i + 1),
            leftx: this.currentData.leftx + this.tileWidth * j,
            rightx: this.currentData.leftx + this.tileWidth * (j + 1)
        };
    }

    private createTileImages() {
        return {
            background: this.getBackgroundImage(),
            object: TileImage.Empty
        };
    }

    private getBackgroundImage(): TileImage {
        switch (this.currentData.type) {
            case SegmentType.Grass: {
                return TileImage.Grass;
            }
            default: {
                return TileImage.Empty;
            }
        }
    }

    private initializeObjects() {
        if (!this.currentData.objects) {
            return;
        }

        for (let object of this.currentData.objects) {
            this.addObjectToTiles(object);
        }
    }

    private addObjectToTiles(data: ObjectData) {
        let objectTile = this.getCurrentTileDataByXY(data.x, data.y);

        if (!objectTile) {
            return;
        }

        objectTile.object = TileImage.Knight;
    }

    private getCurrentTileDataByXY(x: number, y: number): TileData {
        let i = Math.floor((y - this.currentData.upy) / this.tileHeight);
        let j = Math.floor((x - this.currentData.leftx) / this.tileHeight);

        if (!this.currentTiles[i] || !this.currentTiles[i][j]) {
            return;
        }

        return this.currentTiles[i][j];
    }

    private get tileWidth(): number {
        return (this.currentData.rightx - this.currentData.leftx) / this.tilesSize;
    }

    private get tileHeight(): number {
        return (this.currentData.downy - this.currentData.upy) / this.tilesSize;
    }

    private createTiles() {
        this.tiles = this.currentTiles.reduce((sumArr, currentArr) => sumArr.concat(currentArr), []);
    }
}