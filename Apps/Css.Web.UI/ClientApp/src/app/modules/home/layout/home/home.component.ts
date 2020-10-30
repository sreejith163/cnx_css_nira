import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import * as AdminLte from 'admin-lte';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
    $('[data-widget="treeview"]').each(x => {
      AdminLte.Treeview._jQueryInterface.call($(this), 'init');
    });
  }

  navigateToAgentAdmin() {
    this.router.navigate(['add-agent-profile']);
  }

}
