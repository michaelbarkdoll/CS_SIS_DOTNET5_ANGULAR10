import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-webadmin',
  templateUrl: './webadmin.component.html',
  styleUrls: ['./webadmin.component.css']
})
export class WebadminComponent implements OnInit {
  user: User;
  members: Partial<Member[]>;
  userParams: UserParams;
  pagination: Pagination;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
   }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembersURLRequests(this.userParams).subscribe(response => {
      // this.members = members;
      this.members = response.result;
      this.pagination = response.pagination;
      // console.log(this.members);
      // this.oldUrl = this.member.personalURL;
    })
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.userParams.printerPageNumber = event.page; //.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    this.loadMembers();
  }

  approve(member: Member) {
    console.log(member);
    member.oldPersonalURL = member.personalURL;
    member.personalURL = member.requestedURL;
    member.requestedURL = "";

    this.memberService.updateMemberURLRequest(member).subscribe(response => {
      //   // console.log(response);
        this.toastr.success('Updated url for ' + member.username);
        this.loadMembers();
        // this.createPrinter();
        // this.loadPrinters();
      });
    // this.memberService.updateMemberURLRequest(member).subs


  }

  deny(member: Member) {
    console.log(member);
    // member.oldPersonalURL = member.personalURL;
    // member.personalURL = member.requestedURL;
    member.requestedURL = "";

    this.memberService.updateMemberURLRequest(member).subscribe(response => {
      //   // console.log(response);
        this.toastr.success('Denied url for ' + member.username);
        this.loadMembers();
        // this.createPrinter();
        // this.loadPrinters();
    });
  }

}
