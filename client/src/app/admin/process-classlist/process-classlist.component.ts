import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Pagination } from 'src/app/_models/pagination';
import { Semester } from 'src/app/_models/semester';
import { User } from 'src/app/_models/user';
import { UserFile } from 'src/app/_models/userfile';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { AdminService } from 'src/app/_services/admin.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-process-classlist',
  templateUrl: './process-classlist.component.html',
  styleUrls: ['./process-classlist.component.css']
})
export class ProcessClasslistComponent implements OnInit {
  @Input() semester: Semester;

  user: User;
  userParams: UserParams;
  pagination: Pagination;
  classLists: UserFile[];
  isHidden = true;

  constructor(private accountService: AccountService, private memberService: MembersService, private adminService: AdminService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadClassList();
  }

  loadClassList() {
    this.userParams.semesterId = this.semester.id;
    // this.userParams.semesterId = 2;
    // this.adminService.getSemesterClasslistFilesPaginated(this.userParams).subscribe(response => {
    this.adminService.getSemesterClasslistPaginated(this.userParams).subscribe(response => {
      this.classLists = response.result;
      this.pagination = response.pagination;
      this.isHidden = false;
    })
    // this.semester.id
  }

  batchAddUsersFromClassList(publicId: string) {
    // Call process classlist based upon userFile
    this.adminService.batchAddUsersFromClassList(publicId, this.semester).subscribe(response => {
      this.toastr.success("Create users from Classlist processed successfully.");
    })
  }

  batchAllowUsersLoginFromClassList(publicId: string) {
    // Call process classlist based upon userFile
    this.adminService.batchAllowUsersLoginFromClassList(publicId).subscribe(response => {
      this.toastr.success("Allowed logins from Classlist processed successfully.");
    })
  }

  batchDisableUsersLoginFromClassList(publicId: string) {
    // Call process classlist based upon userFile
    this.adminService.batchDisableUsersLoginFromClassList(publicId).subscribe(response => {
      this.toastr.success("Disabled logins from Classlist processed successfully.");
    })
  }

  batchUpdateMajorsFromClassList(publicId: string) {
    // Call process classlist based upon userFile
    this.adminService.batchUpdateMajorsFromClassList(publicId).subscribe(response => {
      this.toastr.success("Majors from Classlist processed successfully.");
    })
  }

  processClassList() {
    // Call process classlist based upon userFile
    // this.adminService.processClassList(userFile.publicId, this.semester).subscribe(response2 => {
    //   this.toastr.success("Classlist updated successfully.");
    // })
  }

  pageChanged(event: any) {
    // console.log(this.userParams.pageNumber);
    console.log(event.page);
    // this.userParams.pageNumber = event.page;
    this.userParams.classlistPageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    this.loadClassList();
  }

}
