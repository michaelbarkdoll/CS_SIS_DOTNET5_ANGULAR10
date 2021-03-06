import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { PrescenceService } from './_services/prescence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  // title = 'Student Information System';
  title = 'SIU - School of Computing';
  users: any;

  //constructor(private http: HttpClient, private accountService: AccountService) {}
  constructor(private accountService: AccountService, private presence: PrescenceService) {}

  ngOnInit() {
    //this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    if(user) {
      this.accountService.setCurrentUser(user);
      this.presence.createHubConnection(user);
    }
    
  }

  // getUsers() {
  //   this.http.get('https://localhost:5001/api/users').subscribe(response => {
  //     this.users = response;
  //   }, error => {
  //     console.log(error);
  //   })
  // }
}
