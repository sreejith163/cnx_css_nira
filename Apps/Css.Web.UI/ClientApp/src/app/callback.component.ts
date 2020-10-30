import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  constructor(
    private router: Router,
    private cookieService: CookieService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const authorizationToken = this.route.snapshot.queryParamMap.get('token');
    if (authorizationToken) {
      this.cookieService.set(environment.settings.sessionName, authorizationToken, null, environment.settings.cookiePath, null, false, 'Strict');
      this.router.navigate(['home']);
    }
  }
}
