import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs';


@Component({
  selector: 'app-before-game',
  templateUrl: './before-game.component.html',
  styleUrls: ['./before-game.component.css'],
  
})
export class BeforeGameComponent implements OnInit {

  @Input()
  playerId: number;

  @Input()
  username: string;

  pageRanking = 1;

  sizeRanking = 10;

  pageHistory = 1;

  sizeHistory = 10;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap
      .pipe(map(() => window.history.state))
      .subscribe(data => {
        this.playerId = data.playerId;
        this.username = data.username;
    });
  }

  startGame() {
    this.router.navigate(["/game"], { state: { playerId: this.playerId, username: this.username } });
  }
}
