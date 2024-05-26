import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent{
  baseUrl = 'https://localhost:7137/api/';
  validationErrors: string[] = [];

  constructor(private http: HttpClient) { }

  get404Error() {
    this.http.get(this.baseUrl + 'errorhandling/not-found').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    })
  }
  get400ValidationError() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
      this.validationErrors = error;
    })
  }
  get400Error() {
    this.http.get(this.baseUrl + 'errorhandling/bad-request').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    })
  }
  get500Error() {
    this.http.get(this.baseUrl + 'errorhandling/server-error').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    })
  }
  get401Error() {
    this.http.get(this.baseUrl + 'errorhandling/auth').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    })
  }
}
