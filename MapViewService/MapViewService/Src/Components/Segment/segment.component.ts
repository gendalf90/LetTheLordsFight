import { Input, Component } from '@angular/core';
import { SegmentData, ObjectData, SegmentType } from './data';
import { TileData, TileImage } from '../Tile/data';

@Component({
    selector: 'segment',
    templateUrl: './segment.component.html'
})
export class SegmentComponent {

    private splitCoefficient: number = 5;
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
        for (let i = 0; i < this.splitCoefficient; i++) {
            this.currentTiles[i] = [];
            for (let j = 0; j < this.splitCoefficient; j++) {
                this.currentTiles[i][j] = this.createTileData(i, j);
            }
        }
    }

    private createTileData(i: number, j: number): TileData {
        return {
            i: this.currentData.i * this.splitCoefficient + i,
            j: this.currentData.j * this.splitCoefficient + j,
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
        let tileHeight = (this.currentData.location.downy - this.currentData.location.upy) / this.splitCoefficient;
        let i = Math.floor((y - this.currentData.location.upy) / tileHeight);
        let tileWidth = (this.currentData.location.rightx - this.currentData.location.leftx) / this.splitCoefficient;
        let j = Math.floor((x - this.currentData.location.leftx) / tileHeight);

        if (!this.currentTiles[i] || !this.currentTiles[i][j]) {
            return;
        }

        return this.currentTiles[i][j];
    }

    private createTiles() {
        this.tiles = this.currentTiles.reduce((sumArr, currentArr) => sumArr.concat(currentArr), []);
    }
}