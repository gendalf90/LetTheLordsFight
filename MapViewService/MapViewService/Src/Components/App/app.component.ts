import { Component } from '@angular/core';
import { SegmentData, SegmentType } from '../Segment/data';
import { UserService } from '../../Services/User/user.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html'
})
export class AppComponent {

    constructor(private user: UserService) {
    }

    private get currentUserId(): string {
        let currentUserData = this.user.getCurrentData();
        return currentUserData.id;
    }
}