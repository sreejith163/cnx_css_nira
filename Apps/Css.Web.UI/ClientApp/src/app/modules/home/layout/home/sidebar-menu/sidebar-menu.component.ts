import { Component, OnInit } from '@angular/core';
import { LoggedUserInfo } from 'src/app/core/models/logged-user-info.model';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent implements OnInit {

  loggedUser: LoggedUserInfo;

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loggedUser = this.authService.getLoggedUserInfo();
  }
}
