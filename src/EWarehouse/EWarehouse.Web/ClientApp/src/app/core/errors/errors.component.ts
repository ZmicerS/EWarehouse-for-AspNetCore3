import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.scss']
})
export class ErrorsComponent implements OnInit {

  _routeParams: any;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  get routeParams() {
    return this._routeParams;
  }

  ngOnInit() {
    this._routeParams = this.activatedRoute.snapshot.queryParams;
  }

  goHome() {
    this.router.navigate(['/']);
  }

}
