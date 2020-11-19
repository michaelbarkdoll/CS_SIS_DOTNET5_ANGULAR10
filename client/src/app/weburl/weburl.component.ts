import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-weburl',
  templateUrl: './weburl.component.html',
  styleUrls: ['./weburl.component.css']
})
export class WeburlComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User;

  // Access browser events:  Window before unload
  @HostListener('window:beforeunload', ['$event']) unloadNofitication($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    })
  }

  updateMember() {
    // console.log(this.member);

    // We don't get anything back for this post so empty ()
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success("Profile updated successfully.");
      this.editForm.reset(this.member);
      this.loadMember();
    })
  }
   
}
