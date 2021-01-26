import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { ContainerJob } from 'src/app/_models/containerjob';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { DockerContainerStatusDialogComponent } from '../docker-container-status-dialog/docker-container-status-dialog.component';

@Component({
  selector: 'app-docker-container-view',
  templateUrl: './docker-container-view.component.html',
  styleUrls: ['./docker-container-view.component.css']
})
export class DockerContainerViewComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  
  user: User;
  member: Member;
  
  pagination: Pagination;
  userParams: UserParams;
  containerStatusList = [{value: 'Running', display: 'Running'}, {value: 'Starting', display: 'Starting'}, {value: 'Created', display: 'Created'}, {value: 'All', display: 'All'}];
  containerJobs: Partial<ContainerJob[]> = [];

  bsModalRef: BsModalRef;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService, private modalService: BsModalService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    // this.loadMember();
    this.loadContainerJobs();
  }

  loadMember() {
    this.memberService.getMemberPrintJobs(this.user.username).subscribe(member => {
      this.member = member;
    })
  }

  loadContainerJobs() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getMembersPaginatedContainerJobs(this.userParams).subscribe(response => {
      // this.printJobs = response.result;
      this.containerJobs = response.result;
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
    this.loadContainerJobs();
  }


  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.userParams.containerJobPageNumber = event.page; //.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    this.loadContainerJobs();
  }


  openModal(template: TemplateRef<any>, containerJob: ContainerJob) {
    const config = {
      class: 'modal-dialog-centered',
      stuff: 'test',
      
      initialState: {
        title2: 'Test Data',
        printJob2: containerJob
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
      title: 'Current container job status: ' + containerJob.containerStatus,
      containerJob
    };

    // this.bsModalRef = this.modalService.show(template, {class: 'modal-sm'});
    this.bsModalRef = this.modalService.show(DockerContainerStatusDialogComponent, {initialState});

    // this.bsModalRef.content.title = 'Current Job Status: ' + printJob.jobStatus;
    
    this.bsModalRef.content.jobStatus = containerJob.containerStatus;
    // this.bsModalRef = this.modalService.show(template, {initialState});

    // this.bsModalRef.content.onClose.subscribe(result => {
    //   console.log('results', result);
    // })

    this.bsModalRef.content.onClosePrintJob.subscribe(result => {
      // console.log('printJob', printJob.jobStatus);
      // console.log('results', result);   // result= string of printJob.jobStatus
      if(result === 'Running') {
        this.memberService.setContainerJobRunning(containerJob.id).subscribe(response => {
          // console.log(response);
          this.loadContainerJobs();
        });
      }
      if(result === 'Starting') {
        this.memberService.setContainerJobStarting(containerJob.id).subscribe(response => {
          // console.log(response);
          this.loadContainerJobs();
        });
      }
      if(result === 'Running') {
        this.memberService.setContainerJobRunning(containerJob.id).subscribe(response => {
          // console.log(response);
          this.loadContainerJobs();
        });
      }
      if(result === 'Creating') {
        this.memberService.setContainerJobCreating(containerJob.id).subscribe(response => {
          // console.log(response);
          this.loadContainerJobs();
        });
      }
      if(result === 'Stop') {
        this.memberService.setContainerJobStop(containerJob.id).subscribe(response => {
          // console.log(response);
          this.loadContainerJobs();
        });
      }
      if(result === 'Delete') {
        this.memberService.setContainerJobDelete(containerJob.id).subscribe(response => {
          // console.log(response);
          this.loadContainerJobs();
        });
      }

    })
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
