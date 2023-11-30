// signalr.service.ts

import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/moves', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }) // Replace with your SignalR hub URL
      .build();
  }

  startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started!');
      })
      .catch(err => {
        console.error('Error while establishing connection: ' + err);
      });
  }

  // Add methods to send/receive messages or perform other actions
}
