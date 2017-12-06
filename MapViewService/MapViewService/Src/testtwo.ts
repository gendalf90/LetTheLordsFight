import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TileComponent } from './Components/Tile/tile.component';
import { SegmentComponent } from './Components/Segment/segment.component';
import { SquareComponent } from './Components/Square/square.component';
import { AppComponent } from './Components/App/app.component';
 
@NgModule({
    imports: [BrowserModule],
    declarations: [AppComponent, SquareComponent, SegmentComponent, TileComponent],
    bootstrap: [AppComponent]
})
export class AppModule{}