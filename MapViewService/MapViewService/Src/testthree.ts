import 'zone.js';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';
import { AppModule } from './testtwo';
import { enableProdMode } from '@angular/core';

enableProdMode();
const platform = platformBrowserDynamic();
platform.bootstrapModule(AppModule);