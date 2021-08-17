import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  users: any;

  constructor(private http: HttpClient) { }
  ngOnInit() {
    console.log("Init");
    this.getUsers();
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/Users').subscribe(response => {
      this.users = response;
      console.log("Users retrieved");
    }, error => {
      console.log(error);
    });
  }
}
