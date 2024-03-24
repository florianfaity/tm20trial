import {Component, Input} from '@angular/core';
import {OpenplanetClient} from "../web-api-client";
import {HttpResponse} from "@angular/common/http";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.less']
})
export class NavMenuComponent {
  @Input() isAdmin = false;
  @Input() isAdminView = false;
  @Input() isMapper = false;
  @Input() isPlayer = false;
  @Input() isConnected = false;
  @Input() playerName: string = "Test"

  isExpanded = false;

  constructor(private _openplanetClient : OpenplanetClient ) {}

  onClickSignIn(){
    window.location.href = `Identity/Account/Login`;
  }
  onClickSignOut(){
    window.location.href = "Identity/Account/Logout";
  }

  onClickSignInUbisoft(){
    this._openplanetClient.getAutorize().subscribe((result) => {
      const reader = new FileReader();
      // var resultTets = "";
      // reader.onload = () => {
      //   resultTets = reader.result as string;
      // };
      // reader.readAsText(result.body);

      var test = result;
      console.log(test);
      // console.log(resultTets);
    });
    console.log("onClickSignInUbisoft");
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
