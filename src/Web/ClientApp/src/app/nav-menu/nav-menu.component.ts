import {Component, Input} from '@angular/core';

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

  onClickSignIn(){
    console.log(window.location.href);
    window.location.href = `Identity/Account/Login`;

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
