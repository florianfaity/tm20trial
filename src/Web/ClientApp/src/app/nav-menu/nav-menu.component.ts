import {Component, Input} from '@angular/core';
import {MapDto, OpenplanetClient, SearchClient, UserDto} from "../web-api-client";
import {ToastService} from "../shared/services/toast.service";
import {debounce} from 'lodash';
import {Router} from "@angular/router";

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
  @Input() playerName: string = "";
  @Input() idUser: number;

  searchValue = '';
  isExpanded = false;
  listOfMapsFiltered: MapDto[] = [];
  listOfUsersFiltered: UserDto[] = [];
  loadingSearch = false;

  constructor(private _openplanetClient: OpenplanetClient, private _searchClient: SearchClient,
              private _toastService: ToastService, private _router: Router) {
    this.search = debounce(this.search, 250);
  }

  onClickSignIn() {
    window.location.href = `Identity/Account/Login`;
  }

  onClickSignOut() {
    window.location.href = "Identity/Account/Logout";
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  goToApi() {
    window.open("api", "_blank");
  }



  goToSearch(searchValue: string) {
    this._router.navigateByUrl('/trial/' + searchValue.replace(' ', '/'));
  }

  search(value: string) {
    if (!value || value.length < 3) {
      return;
    }
    this.loadingSearch = true;
    this.listOfMapsFiltered = [];
    this.listOfUsersFiltered = [];

    this._searchClient.getFilteredMapsUsers(value).subscribe({
      next: result => {
        this.loadingSearch = false;
        this.listOfMapsFiltered = result.maps;
        this.listOfUsersFiltered = result.users;
      },
      error: error => {
        this.loadingSearch = false;
        console.error(error)
        this._toastService.error("Error")
      }
    });
  }

  //
  // onClickSignInUbisoft(){
  //   this._openplanetClient.getAutorize().subscribe((result) => {
  //     const reader = new FileReader();
  //     // var resultTets = "";
  //     // reader.onload = () => {
  //     //   resultTets = reader.result as string;
  //     // };
  //     // reader.readAsText(result.body);
  //
  //     var test = result;
  //     console.log(test);
  //     // console.log(resultTets);
  //   });
  //   console.log("onClickSignInUbisoft");
  // }

  protected readonly console = console;
}
