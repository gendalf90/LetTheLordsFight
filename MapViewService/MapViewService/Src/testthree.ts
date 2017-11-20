import 'zone.js';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';
import {AppModule} from './testtwo';
 
const platform = platformBrowserDynamic();
platform.bootstrapModule(AppModule);