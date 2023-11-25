import { Component } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-before-game',
  templateUrl: './before-game.component.html',
  styleUrls: ['./before-game.component.css'],
  
})
export class BeforeGameComponent {

  constructor(private router: Router) { }

  Click() {
    this.router.navigate(["/game"]);
  }

  
}
