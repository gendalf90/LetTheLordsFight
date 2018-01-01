import { Input, Component } from '@angular/core';
import { TileData, TileImage } from './data';
import { UserService } from '../../Services/User/user.service';

const grass = require('../../Img/grass.png');
const forest = require('../../Img/forest.png');
const empty = require('../../Img/empty.png');
const user = require('../../Img/knight-red.png');
const knight = require('../../Img/knight-black.png');

@Component({
    selector: 'tile',
    templateUrl: './tile.component.html',
    styleUrls: ['./tile.component.css']
})
export class TileComponent {

    private pixelSize: number = 32;
    private currentData: TileData;
    private styles: any;

    constructor(private user: UserService) {
        this.styles = this.defaultStyles;
    }

    private get defaultStyles() {
        return {
            backgroundImage: `url(${this.defaultImage})`,
            top: '0px',
            left: '0px',
            width: `${this.pixelSize}px`,
            height: `${this.pixelSize}px`
        };
    }

    @Input()
    set data(value: TileData) {
        this.initialize(value);
        this.setImages();
        this.setPosition();
        this.setBorders();
    }

    private initialize(value: TileData) {
        this.currentData = value;
    }

    private setImages() {
        let background = this.getImageByType(this.currentData.background);
        let object = this.getImageByType(this.currentData.object);
        this.styles.backgroundImage = `url(${object}), url(${background})`;
    }

    private setPosition() {
        let top = this.currentData.i * this.pixelSize;
        let left = this.currentData.j * this.pixelSize;
        this.styles.left = `${left}px`;
        this.styles.top = `${top}px`;
    }

    private setBorders() {
        this.styles['border-left'] = this.getBorderIfExist(this.currentData.borderLeft);
        this.styles['border-right'] = this.getBorderIfExist(this.currentData.borderRight);
        this.styles['border-top'] = this.getBorderIfExist(this.currentData.borderUp);
        this.styles['border-bottom'] = this.getBorderIfExist(this.currentData.borderDown);
    }

    private getBorderIfExist(exist: boolean): string {
        if (exist) {
            return 'solid 1px';
        }
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
            case TileImage.User: {
                return user;
            }
            default: {
                return empty;
            }
        }
    }

    private get defaultImage(): string {
        return empty;
    }

    private onclick(e) {
        let tileRectangle = e.currentTarget.getBoundingClientRect();
        let clickedPixelX = e.clientX - tileRectangle.left;
        let clickedPixelY = e.clientY - tileRectangle.top;
        let clickedTileX = clickedPixelX * this.width / e.currentTarget.width + this.currentData.leftx;
        let clickedTileY = clickedPixelY * this.height / e.currentTarget.height + this.currentData.upy;
        this.user.currentMoveTo({ x: clickedTileX, y: clickedTileY });
    }

    private get width(): number {
        return this.currentData.rightx - this.currentData.leftx;
    }

    private get height(): number {
        return this.currentData.downy - this.currentData.upy;
    }
}