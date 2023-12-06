
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Board, NextMove } from '../models/board';
import { environment } from '../../../environment';
import { Observable } from 'rxjs';
import { group } from '@angular/animations';

@Injectable({
  providedIn: 'root',
  useFactory: () => new SignalRService()
})
export class SignalRService {
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .build();

    this.hubConnection.on("GameStart", (message: string, groupName: string) => {
      console.log(message);
      console.log("Group name: " + groupName);
      //I moze tu invoke async żeby
    });
  }

  sendPlayerInfo() {
    this.hubConnection.on("GetPlayerInfo", async () => {
      const promise = new Promise((resolve) => {
        setTimeout(() => {
          resolve("message");
        }, 1);
      });
      return promise;
    });
  }

  observeStartOfTheGame(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on("GameStart", (message: string, groupName: string) => {
        console.log(message);
        console.log("Group name: " + groupName);
        observer.next(groupName);
        observer.complete();
      });
    });
  }

  observeChangingBoard(): Observable<[Board, NextMove]> {
    return new Observable<[Board, NextMove]>((observer) => {
      this.hubConnection.on("MadeMove", (board: string, nextMove: string, message: string) => {
        const boardObj = JSON.parse(board) as Board;
        const nextMoveObj = JSON.parse(nextMove) as NextMove;
        console.log(JSON.parse(board) as Board);
        console.log(JSON.parse(nextMove) as NextMove);
        console.log(message);
        //console.log("WHo has won???" + whoHasWOn);
        observer.next([boardObj, nextMoveObj]);
        observer.complete();
      });
    });
  }

  startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log("Session has been sucessfully created!");
      })
      .catch((err) => {
        console.error("Error during setting up a session: " + err.toString());
      });
  }

  endConnection(): void {
    this.hubConnection
      .stop()
      .then(() => {
        console.log('Connection ' + this.hubConnection.connectionId + ' has been ended!');
      }).catch((err) => {
        console.error("Couldn't end session: " + this.hubConnection.connectionId + ": " + err.toString())
      });
  }

  getConnectionState(): HubConnectionState {
    return this.hubConnection.state;
  }

  onMadeMove(): void {
    //this.updateBoard();
  }

  updateBoard(board: Board, whoIsNext: NextMove) {
    const boardStr = JSON.stringify(board);
    const whoIsNextStr = JSON.stringify(whoIsNext);
    this.hubConnection.invoke("UpdateBoardAfterMove", boardStr, whoIsNextStr, "tu pozniej group name").then(() => {
      console.log("Method " + "'UpdateBoardAfterMove'" + " was sucessfully invoked!");
    }).catch((err) => {
      console.error("We have an error in invoke of the server method: " + err.toString());
    });
  }

}
