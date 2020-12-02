import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { PrintJob } from '../_models/printjob';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-printjobs',
  templateUrl: './printjobs.component.html',
  styleUrls: ['./printjobs.component.css']
})
export class PrintjobsComponent implements OnInit {
  // members: Partial<Member[]> = [];
  // member: Member;
  user: User;

  pagination: Pagination;
  
  printJobs: Partial<PrintJob[]> = [];
  
  userParams: UserParams;
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
  printStatusList = [{value: 'Held', display: 'Held'}, {value: 'Queued', display: 'Queued'}, {value: 'Cancelled', display: 'Cancelled'}, {value: 'Completed', display: 'Completed'}, {value: 'All', display: 'All'}];
  
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadPrintJobs();
  }

  loadPrintJobs() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getMemberPaginatedPrintJobs(this.userParams).subscribe(response => {
      this.printJobs = response.result;
      this.pagination = response.pagination;
      // console.log("Pagination headers returned:")
      // console.log(this.pagination);
    })

    // this.memberService.getMemberPrintJobs(this.user.username).subscribe(member => {
    //   this.member = member;
    // })
  }

  resetFilters() {
    //this.userParams = new UserParams(this.user);
    this.userParams = this.memberService.resetUserParams();
    this.loadPrintJobs();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.userParams.printJobPageNumber = event.page; //.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    this.loadPrintJobs();
  }
}
