import { Component, HostListener, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { PrintjobsComponent } from 'src/app/printjobs/printjobs.component';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { PrintJob } from 'src/app/_models/printjob';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { PrintJobAdminStatusDialogComponent } from '../print-job-admin-status-dialog/print-job-admin-status-dialog.component';

@Component({
  selector: 'app-printer-job-management',
  templateUrl: './printer-job-management.component.html',
  styleUrls: ['./printer-job-management.component.css']
})
export class PrinterJobManagementComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User;

  pagination: Pagination;
  userParams: UserParams;
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
  printStatusList = [{value: 'Held', display: 'Held'}, {value: 'Queued', display: 'Queued'}, {value: 'Cancelled', display: 'Cancelled'}, {value: 'Completed', display: 'Completed'}, {value: 'All', display: 'All'}];
  printJobs: Partial<PrintJob[]> = [];

  bsModalRef: BsModalRef;
  
  
  // Access browser events:  Window before unload
  // @HostListener('window:beforeunload', ['$event']) unloadNofitication($event: any) {
  //   if(this.editForm.dirty) {
  //     $event.returnValue = true;
  //   }
  // }

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService, private modalService: BsModalService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    // this.loadMember();
    this.loadPrintJobs();
  }

  loadMember() {
    this.memberService.getMemberPrintJobs(this.user.username).subscribe(member => {
      this.member = member;
    })
  }

  // updateMember() {
  //   // console.log(this.member);

  //   // We don't get anything back for this post so empty ()
  //   this.memberService.updateMember(this.member).subscribe(() => {
  //     this.toastr.success("Values updated successfully.");
  //     this.editForm.reset(this.member);
  //   })
  // }

  loadPrintJobs() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getMembersPaginatedPrintJobs(this.userParams).subscribe(response => {
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


  openModal(template: TemplateRef<any>, printJob: PrintJob) {
    const config = {
      class: 'modal-dialog-centered',
      stuff: 'test',
      
      initialState: {
        title2: 'Test Data',
        printJob2: printJob
        //printJob,
        // roles: this.getJobStatus(printJob)
      }
    };

    const initialState = {
      class: 'modal-dialog-centered',
      list: [
        'Open a modal with component',
        'Pass your data',
        'Do something else',
        '...'
      ],
      title: 'Current print job status: ' + printJob.jobStatus,
      printJob
    };

    // this.bsModalRef = this.modalService.show(template, {class: 'modal-sm'});
    this.bsModalRef = this.modalService.show(PrintJobAdminStatusDialogComponent, {initialState});

    // this.bsModalRef.content.title = 'Current Job Status: ' + printJob.jobStatus;
    
    this.bsModalRef.content.jobStatus = printJob.jobStatus;
    // this.bsModalRef = this.modalService.show(template, {initialState});

    // this.bsModalRef.content.onClose.subscribe(result => {
    //   console.log('results', result);
    // })

    this.bsModalRef.content.onClosePrintJob.subscribe(result => {
      // console.log('printJob', printJob.jobStatus);
      // console.log('results', result);   // result= string of printJob.jobStatus
      if(result === 'Held') {
        this.memberService.setPrintJobHeld(printJob.id).subscribe(response => {
          // console.log(response);
          this.loadPrintJobs();
        });
      }
      if(result === 'Queued') {
        this.memberService.setPrintJobQueued(printJob.id).subscribe(response => {
          // console.log(response);
          this.loadPrintJobs();
        });
      }
      if(result === 'Completed') {
        this.memberService.setPrintJobCompleted(printJob.id).subscribe(response => {
          // console.log(response);
          this.loadPrintJobs();
        });
      }
      if(result === 'Cancelled') {
        this.memberService.setPrintJobCancel(printJob.id).subscribe(response => {
          // console.log(response);
          this.loadPrintJobs();
        });
      }


      
      // this.memberService.

      
      // this.resetFilters();
      // this.loadPrintJobs();
      // this.loadPrintJobs();

      // this.memberService.getMembersPaginatedPrintJobs(this.userParams).subscribe(response => {
      //   this.printJobs = response.result;
      //   this.pagination = response.pagination;
      //   // console.log("Pagination headers returned:")
      //   // console.log(this.pagination);
      // })
    })

    //   console.log(values);
    // })
    // this.bsModalRef.content.title.subscribe(values => {
    //   console.log(values);
    // })

    //this.bsModalRef.content.update

    // this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
    //   const rolesToUpdate = {
    //     roles: [...values.filter(el => el.checked === true).map(el => el.name)]   // ... spread operation
    //   };
  }

  confirm(): void {
    // this.message = 'Confirmed!';
    this.bsModalRef.hide();
  }
 
  decline(): void {
    // this.message = 'Declined!';
    this.bsModalRef.hide();
  }
}
