import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TileComponent } from './Components/Tile/tile.component';
import { SegmentComponent } from './Components/Segment/segment.component';
import { SquareComponent } from './Components/Square/square.component';
import { AppComponent } from './Components/App/app.component';
import { ConfigurationService } from './Services/Configuration/configuration.service';
import { UserService } from './Services/User/user.service';
import { SquareService } from './Services/Square/square.service';
 
@NgModule({
    imports: [BrowserModule],
    declarations: [AppComponent, SquareComponent, SegmentComponent, TileComponent],
    providers: [SquareService, ConfigurationService, UserService],
    bootstrap: [AppComponent]
})
export class AppModule{}