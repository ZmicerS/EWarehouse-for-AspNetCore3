import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Observable, Observer } from 'rxjs';

import { UserListComponent } from './user-list.component';
import { AccountService } from '../../core';
import { User } from '../../core';

describe('UserListComponent', () => {
  let component: UserListComponent;
  let fixture: ComponentFixture<UserListComponent>;

  const mockData = [{
    id: 1,
    userName: 'Test',
    assignedRoles: [{ roleName: 'User' }]
  }] as User[];

  class MockAccountService {
    getUsers(): Observable<User[]> {
      return new Observable((observer: Observer<any>) => {
        observer.next(mockData);
        observer.complete();
      });
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      declarations: [UserListComponent],
      providers: [
        { provide: AccountService, useValue: new MockAccountService() },
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


  it('should make a call to AccountService.getUsers()', () => {
    const service: AccountService = TestBed.get(AccountService);
    spyOn(service, 'getUsers').and.callThrough();

    service.getUsers();

    expect(service.getUsers).toHaveBeenCalled();
  });

  it('should get Users', () => {
    component.ngOnInit();
    expect(component.users.length).toBe(mockData.length);
  });

  it('should filling table of users', () => {
    component.ngOnInit();

    const row = fixture.debugElement.nativeElement.querySelector('table tbody tr:nth-of-type(1)');
    const col1 = fixture.debugElement.nativeElement.querySelector('table tbody tr td:nth-of-type(1)');
    const col3 = fixture.debugElement.nativeElement.querySelector('table tbody tr td a:nth-of-type(1)');

    const textCol2 = row.childNodes[1].children[0].innerText;
    const textCol4 = row.children[3].firstChild.href;

    expect(col1.innerText).toContain('Test');
    expect(textCol2).toContain('User');
    expect(col3.href.endsWith('/edit/1')).toBeTruthy();
    expect(textCol4.endsWith('/delete/1')).toBeTruthy();
  });


});
