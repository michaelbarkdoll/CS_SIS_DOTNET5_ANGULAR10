import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Course } from 'src/app/_models/course';
import { Pagination } from 'src/app/_models/pagination';
import { Semester } from 'src/app/_models/semester';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { AdminService } from 'src/app/_services/admin.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-semester-management',
  templateUrl: './semester-management.component.html',
  styleUrls: ['./semester-management.component.css']
})
export class SemesterManagementComponent implements OnInit {
  user: User;
  semesters: Semester[];
  pagedSemesters: Semester[];
  courses: Course[];
  userParams: UserParams;
  pagination: Pagination;
  termList = [{value: 'All', display: 'All'}, {value: 'Spring', display: 'Spring'}, {value: 'Summer', display: 'Summer'}, {value: 'SummerIntercession1', display: 'Intercession Summer 1'}, {value: 'SummerIntercession2', display: 'Intercession Summer 2'}, {value: 'Fall', display: 'Fall'}, {value: 'WinterIntercession1', display: 'Intercession Winter 1'}];
  newTermList = [{value: 'Spring', display: 'Spring'}, {value: 'Summer', display: 'Summer'}, {value: 'SummerIntercession1', display: 'Intercession Summer 1'}, {value: 'SummerIntercession2', display: 'Intercession Summer 2'}, {value: 'Fall', display: 'Fall'}, {value: 'WinterIntercession1', display: 'Intercession Winter 1'}];

  selectedSemester: Semester;
  selectedSemesterCSV: Semester;
  isHidden = false;
  addSemester = false;

  newSemesterTermName: string;
  newSemesterYear: number;

  constructor(private accountService: AccountService, private memberService: MembersService, private adminService: AdminService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadSemesters();
    this.newSemesterTermName = "Spring";
    this.newSemesterYear = 2020;
    // this.selectSemester();
  }

  // loadSemesters() {
  //   this.adminService.getSemesters().subscribe(semesters => {
  //     this.semesters = semesters;
  //   })
  // }

  loadSemesters() {
    this.adminService.getSemestersPaginated(this.userParams).subscribe(response => {
      this.semesters = response.result;
      this.pagination = response.pagination;
    })
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadSemesters();
  }

  uploadsemester(semester: Semester) {
    this.selectedSemester = semester;
    this.isHidden = true;
    this.addSemester = false;
    this.selectedSemesterCSV = null;
  }

  processSemesterCSV(semester: Semester) {
    this.selectedSemesterCSV = semester;
    this.isHidden = true;
    this.addSemester = false;
  }

  toggleCreateSemester() {
    this.selectedSemesterCSV = null;
    this.selectedSemester = null;
    this.isHidden = !this.isHidden;
    this.addSemester = !this.addSemester;
  }

  cancelUploadSemester(semester: Semester) {
    this.isHidden = false;
    this.selectedSemester = null;
    this.selectedSemesterCSV = null;
  }

  createNewSemester() {

    var alreadyExists = false;

    if(this.semesters) {
      this.semesters.forEach(element => {
        if(element.year == this.newSemesterYear && element.term == this.newSemesterTermName) {
          this.toastr.error("Semester already exists!");
          alreadyExists = true;
        }
      });

      if (!alreadyExists) {
        this.adminService.createNewSemester(this.newSemesterYear, this.newSemesterTermName).subscribe(response => {
          this.toastr.success("Semester created successfully.");
          this.loadSemesters();
          this.toggleCreateSemester();
        })
      }
      // this.adminService.createNewSemester(this.newSemesterYear, this.newSemesterTermName).subscribe(response => {
      //   this.toastr.success("Semester created successfully.");
      // })
    }
    


    
  }

  deletesemester(semester: Semester) {
    
  }


  pageChanged(event: any) {
    // console.log(this.userParams.pageNumber);
    console.log(event.page);
    // this.userParams.pageNumber = event.page;
    this.userParams.semesterPageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    this.loadSemesters();
  }

}
