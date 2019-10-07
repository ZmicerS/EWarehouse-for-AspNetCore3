import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Observable, Observer, of } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

import { User, Role } from '../../core/models';
import { AccountService } from '../../core/services';
import { UserManageComponent } from './user-manage.component';

describe('UserManageComponent', () => {
  let component: UserManageComponent;
  let fixture: ComponentFixture<UserManageComponent>;

  const router = {
    navigate: jasmine.createSpy('navigate')
  };

  const mockUser = {
    id: 1,
    userName: 'Test',
    assignedRoles: [{
      id: 2,
      roleName: 'User'
    }]
  } as User;

  const mockRoles = [{
    id: 1,
    roleName: 'Admin'
  }, {
    id: 2,
    roleName: 'User'
  }] as Role[];

  class MockAccountService {
    getUser(): Observable<User[]> {
      return new Observable((observer: Observer<any>) => {
        observer.next(mockUser);
        observer.complete();
      });
    }
    getRoles(): Observable<Role[]> {
      return of(mockRoles);
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, FormsModule],
      declarations: [UserManageComponent],
      providers: [
        { provide: AccountService, useValue: new MockAccountService() },
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({ get: (key: string) => '1' })
          }
        },
        { provide: Router, useValue: router }
      ]
    })
      .compileComponents();
  }));


  beforeEach(() => {
    fixture = TestBed.createComponent(UserManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate pointed path when cancel', () => {
    const activatedRoute: ActivatedRoute = TestBed.get(ActivatedRoute);
    component.onCancel();
    expect(router.navigate).toHaveBeenCalledWith(['../../list'], { relativeTo: activatedRoute });
  });

  it('should filling form of user roles correctly', () => {
    component.ngOnInit();

    const roleForm = fixture.debugElement.nativeElement.querySelector('Form');

    const firstChekBox = roleForm[0];
    const secondChekBox = roleForm[1];

    expect(firstChekBox.checked).toBe(false);
    expect(secondChekBox.checked).toBe(true);
  });

});



