import { Input, Component } from '@angular/core';
import { TileData, TileImage } from './data';

var grass = require('../../Img/grass.png');
var empty = require('../../Img/empty.png');
var knight = require('../../Img/test.png');

@Component({
    selector: 'tile',
    templateUrl: './tile.component.html'
})
export class TileComponent {

    private size: number = 32;
    private backgroundImage: string;
    private objectImage: string;
    private top: number;
    private left: number;

    constructor() {
        this.backgroundImage = this.getDefaultImage();
        this.objectImage = this.getDefaultImage();
        this.top = 0;
        this.left = 0;
    }

    @Input()
    set data(value: TileData) {
        this.backgroundImage = this.getImageByType(value.background);
        this.objectImage = this.getImageByType(value.object);
        this.top = value.i * this.size;
        this.left = value.j * this.size;
    }

    private getImageByType(type: TileImage): string {
        switch (type) {
            case TileImage.Grass: {
                return grass;
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
}