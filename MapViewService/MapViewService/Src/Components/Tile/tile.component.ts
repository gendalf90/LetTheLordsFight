import { Input, Component } from '@angular/core';
import { TileData, TileImage } from './data';

const grass = require('../../Img/grass.png');
const forest = require('../../Img/forest.png');
const empty = require('../../Img/empty.png');
const user = require('../../Img/knight-black.png');
const knight = require('../../Img/knight-red.png');

@Component({
    selector: 'tile',
    templateUrl: './tile.component.html',
    styleUrls: ['./tile.component.css']
})
export class TileComponent {

    private pixelSize: number = 32;
    private backgroundImage: string;
    private objectImage: string;
    private pixelTop: number;
    private pixelLeft: number;
    private currentData: TileData;

    constructor() {
        this.backgroundImage = this.getDefaultImage();
        this.objectImage = this.getDefaultImage();
        this.pixelTop = 0;
        this.pixelLeft = 0;
    }

    @Input()
    set data(value: TileData) {
        this.currentData = value;
        this.applyData();
    }

    private applyData() {
        this.backgroundImage = this.getImageByType(this.currentData.background);
        this.objectImage = this.getImageByType(this.currentData.object);
        this.pixelTop = this.currentData.i * this.pixelSize;
        this.pixelLeft = this.currentData.j * this.pixelSize;
    }

    private getImageByType(type: TileImage): string {
        switch (type) {
            case TileImage.Grass: {
                return grass;
            }
            case TileImage.Forest: {
                return forest;
            }
            case TileImage.Empty: {
                return empty;
            }
            case TileImage.Knight: {
                return knight;
            }
            default: {
                return empty;
            }
        }
    }

    private getDefaultImage(): string {
        return empty;
    }

    private onclick(e): void {
        //let tileRect = e.currentTarget.getBoundingClientRect();
        //let clickedX = e.clientX - tileRect.left; //+right
        //let clickedY = e.clientY - tileRect.top; //+bottom
        //e.currentTarget.width // +height

        //alert(`${clickedX} ${clickedY} ${this.data.i}`);

        alert('click');
    }
}