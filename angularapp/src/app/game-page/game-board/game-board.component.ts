import { Component } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { SignalRService } from '../services/HubService';


@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css'],
  animations: [
    trigger('changeColor', [
      state('initial', style({
        backgroundColor: 'red'
      })),
      state('final', style({
        backgroundColor: 'green'
      })),
      transition('initial <=> final', animate('1000ms'))
    ])
  ]
})
export class GameBoardComponent {
  constructor(private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.signalRService.startConnection();
  }

  colorState = 'initial';
  toggleColor() {
    this.colorState = (this.colorState === 'initial') ? 'final' : 'initial';
  }
}
