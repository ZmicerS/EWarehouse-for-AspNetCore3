import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { ConfirmPasswordValidator } from './confirm-password-validator';
import { RegisterModel } from '../../core';
import { AccountService } from '../../core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  _submitting = false;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService, private router: Router) { }

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
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: ['', Validators.required]
    },
      { validator: ConfirmPasswordValidator.MatchPassword }
    );
  }

  isControlInvalid(controlName: string): boolean {
    const control = this.registerForm.controls[controlName];
    const result = control.invalid && control.touched;
    return result;
  }

  isControlNotFilled(controlName: string): boolean {
    const control = this.registerForm.controls[controlName];
    const result = control.errors && control.errors.required && (control.touched || control.dirty);
    return result;
  }

  onSubmit() {
    this.register();
  }

  private register() {
    this.registerForm.disable();
    this._submitting = true;
    const registerData = {
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword
    } as RegisterModel;

    this.accountService.register(registerData).subscribe(event => {
      this.registerForm.enable();
      this.router.navigate(['/account']);
    });
  }

  onCancel() {
    this.router.navigate(['/']);
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.registerForm.dirty && !this._submitting) {
      return window.confirm('Do you want leave this page?');
    }
    return true;
  }

}
