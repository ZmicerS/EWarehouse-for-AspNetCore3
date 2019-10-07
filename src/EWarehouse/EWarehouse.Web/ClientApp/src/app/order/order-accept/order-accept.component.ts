import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-order-accept',
  templateUrl: './order-accept.component.html',
  styleUrls: ['./order-accept.component.scss']
})
export class OrderAcceptComponent implements OnInit {

  constructor(private router: Router, public snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  onAccept() {
    this.showSnack();
    this.router.navigate(['/']);
  }

  onCancel() {
    this.router.navigate(['/']);
  }

  showSnack() {
    this.snackBar.open('Ok', '', {
      duration: 2000,
      verticalPosition: 'bottom'
    });
  }
}
