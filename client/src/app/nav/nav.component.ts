import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  factor: string;
  factorStatusList = [{value: 'Push', display: 'Push'}, {value: 'Phone', display: 'Phone'}, {value: 'Passcode', display: 'Passcode'}, {value: 'SMS', display: 'SMS'}];
  //loggedIn: boolean;
  //currentUser$: Observable<User>;

  constructor(public accountService: AccountService, private router: Router, 
    private toastr: ToastrService) { 
      // factor = 
    }

  ngOnInit(): void {
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$;
    this.model.factor = "Push";

  }


  login() {
    //console.log(this.model);
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
      this.router.navigateByUrl('/webinfo');
      // this.router.navigateByUrl('/member/edit');
      // this.router.navigateByUrl('/members');
      //this.loggedIn = true;
    })
  }
  // login() {
  //   //console.log(this.model);
  //   this.accountService.login(this.model).subscribe(response => {
  //     // console.log(response);
  //     this.router.navigateByUrl('/members');
  //     //this.loggedIn = true;
  //   }, error => {
  //     console.log(error);
  //     this.toastr.error(error.error);
  //   })
  // }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    //this.loggedIn = false;
  }

  getCurrentUser() {
    // It's not good practice to subscribe to something and then not unsubscribe to it.
    // We'll instead use an async pipe
    // this.accountService.currentUser$.subscribe(user => {
    //   this.loggedIn = !!user;   // turns user into a boolean; null=false ifsomething=true
    // }, error => {
    //   console.log(error);
    // })
  }

}
