import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AuthPageModule } from './auth-page/auth-page.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { GamePageModule } from './game-page/game-page.module';
import { WaitRoomModule } from './wait-room/wait-room.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, AuthPageModule, BrowserAnimationsModule, WaitRoomModule, AppRoutingModule, GamePageModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
