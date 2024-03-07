import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  @Input() isAdmin = false;
  @Input() isAdminView = false;
  @Input() isMapper = false;
  @Input() isPlayer = false;
  @Input() playerName: string = "Test"

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
