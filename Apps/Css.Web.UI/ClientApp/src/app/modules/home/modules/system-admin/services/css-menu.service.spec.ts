import { TestBed } from '@angular/core/testing';

<<<<<<< HEAD:Apps/Css.Web.UI/ClientApp/src/app/modules/home/modules/scheduling-menu/services/agent-admin.service.spec.ts
import { AgentAdminService } from './agent-admin.service';

describe('AgentAdminService', () => {
  let service: AgentAdminService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentAdminService);
=======
import { CssMenuService } from './css-menu.service';

describe('CssMenuService', () => {
  let service: CssMenuService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CssMenuService);
>>>>>>> origin/Sprint-4-Develop:Apps/Css.Web.UI/ClientApp/src/app/modules/home/modules/system-admin/services/css-menu.service.spec.ts
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
