import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { LoginModel } from '../../core';
import { AccountService, AuthService } from '../../core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  _submitting = false;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private authService: AuthService,
    private router: Router) { }

  get submitting() {
    return this._submitting;
  }

  set submitting(value: boolean) {
    if (value) {
      this._submitting = value;
    }
  }

  ngOnInit() {
    this.initForm();
  }

  private initForm() {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]],
    });
  }

  isControlInvalid(controlName: string): boolean {
    const control = this.loginForm.controls[controlName];
    const result = control.invalid && control.touched;
    if (controlName === 'email') {
    }
    return result;
  }

  isControlNotFilled(controlName: string): boolean {
    const control = this.loginForm.controls[controlName];
    const result = control.errors && control.errors.required && (control.touched || control.dirty);
    return result;
  }

  onSubmit() {
    this.loginForm.disable();
    this._submitting = true;
    this.login();
  }

  private login() {
    const loginData = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    } as LoginModel;

    this.accountService.login(loginData).subscribe(res => {
      this.authService.login(res.accessToken);
      this.loginForm.enable();
      this._submitting = false;
      this.router.navigate(['/']);
    });
  }

  onCancel() {
    this.router.navigate(['/']);
  }

}
