// signalr.service.ts

import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Board, NextMove } from '../models/board';

@Injectable({
  providedIn: 'root'
})
export class SignalRService implements OnDestroy {
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7166/moves') 
      .build();

    this.hubConnection.on("ReceiveMessage", (message: string) => {
      console.log(message);
    });
  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  startConnection(): void {
    console.log("WTFSS");
  }

  endConnection(): void {
    this.hubConnection
      .stop()
      .then(() => {
        console.log('Connection ' + this.hubConnection.connectionId + ' has been ended!');
      });
  }



  getConnection() {
   
    console.log('1' + this.hubConnection.state);

    this.hubConnection.start().then(function () {
    }).catch(function (err) {
      return console.error("Aha" + err.toString());
    });

    console.log('2' + this.hubConnection.state);

    console.log('4' + this.hubConnection.state);
    this.hubConnection.on('MadeMove', (board: string, nextMove: string) => {
      console.log("Info from server: ");
      console.log(board);
      console.log("No STARA KURWA");
      console.log(nextMove);
      console.log("PIERDOLE? NIC BARDZIEJ MYLNEGO");
      console.log(JSON.parse(board) as Board);
      console.log(JSON.parse(nextMove) as NextMove);
      console.log("NO CHOCIAZ KURWA TYLE");
    });
  };

  private receiveMessage(arg1: string, arg2: string) {
    console.log(`Received arguments: ${arg1}, ${arg2}`);
  }

  getState() {
  
    console.log('3' + this.hubConnection.state);
    this.hubConnection.invoke("Hello").then(() => {
      console.log("Method was sucessfully invoked!");
    }).catch(function (err) {
      return console.error("We have error in invoke of the server method" + err.toString());
    });

  }

  

  test(): void {
    this.hubConnection.on('DisplayMessage', (message: string) => {
      console.log(message);
    });
  }
}
