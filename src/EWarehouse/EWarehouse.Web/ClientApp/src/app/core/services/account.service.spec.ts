import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AccountService } from './account.service';
import { User, LoginModel } from '../../core';

describe('AccountService', () => {
  let httpTestingController: HttpTestingController;
  let service: AccountService;
  let httpMock: HttpTestingController;
  let mockUsers: User[];
  let mockLogin: LoginModel;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: 'BASE_URL', useValue: 'http://localhost:9876/' },
        AccountService
      ],
      imports: [HttpClientTestingModule]
    });

    httpTestingController = TestBed.get(HttpTestingController);
    service = TestBed.get(AccountService);
    httpMock = TestBed.get(HttpTestingController);

    mockUsers = [{
      id: 1,
      userName: 'Test1',
      assignedRoles: [{ roleName: 'User' }]
    },
    {
      id: 2, userName: 'Test2',
      assignedRoles: [{ roleName: 'User' }]
    }];

  });

  mockLogin = {
    email: 'test@test.com',
    password: 'password'
  };

  afterEach(() => {
    httpMock.verify();
  });


  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get user by id', () => {
    service = TestBed.get(AccountService);
    httpMock = TestBed.get(HttpTestingController);
    const basicUrl = TestBed.get('BASE_URL');
    const id = 1;

    service.getUser(id).subscribe(user => {
      expect(user).toEqual(mockUsers[id]);
    });

    httpMock.expectOne({
      method: 'GET',
      url: `${basicUrl}account/getuser/${id}`
    }).flush(mockUsers[id]);
  });

  it('should get the list of users', () => {
    service = TestBed.get(AccountService);
    httpMock = TestBed.get(HttpTestingController);
    const basicUrl = TestBed.get('BASE_URL');

    service.getUsers().subscribe(users => {
      expect(users).toEqual(mockUsers);
    });

    const request = httpMock.expectOne(`${basicUrl}account/getusers`);
    expect(request.request.method).toBe('GET');
    request.flush(mockUsers);
  });

  it('should post the correct data', () => {
    service = TestBed.get(AccountService);
    httpMock = TestBed.get(HttpTestingController);
    const basicUrl = TestBed.get('BASE_URL');

    service.login(mockLogin).subscribe((data: any) => {
      expect(data.accessToken).toBe('accessToken');
    });

    const req = httpMock.expectOne(
      `${basicUrl}token`,
      'post to api'
    );
    expect(req.request.method).toBe('POST');

    req.flush({
      accessToken: 'accessToken'
    });
  });

});
