import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: 'home.component.html',
  styles:[`
    .background-image {      
      background-image: url("/assets/img/background.png");
      background-size: cover;  
      height: 100%;
      width: 100%;

      /* Center and scale the image nicely */
      background-position: center;
      background-repeat: no-repeat;
    }
  `],
})
export class HomeComponent {
  dataLoading = false;
}
