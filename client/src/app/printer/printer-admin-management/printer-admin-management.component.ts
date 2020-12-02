import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { Printers } from 'src/app/_models/printer';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { PrinterAdminStatusDialogComponent } from '../printer-admin-status-dialog/printer-admin-status-dialog.component';

@Component({
  selector: 'app-printer-admin-management',
  templateUrl: './printer-admin-management.component.html',
  styleUrls: ['./printer-admin-management.component.css']
})
export class PrinterAdminManagementComponent implements OnInit {
  member: Member;
  user: User;
  printers: Printers[] = [];
  addPrinter: boolean = false;

  // newPrinter: Partial<Printers>;
  newPrinter: Printers;

  pagination: Pagination;
  userParams: UserParams;
  bsModalRef: BsModalRef;
  
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService, private modalService: BsModalService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    // this.loadMember();
    this.loadPrinters();
    this.newPrinter = {
      // id: number;
      // id: 1,
      printerName: 'printerName',
      url: 'http://127.0.0.1:631',
      port: 631,
      sshUsername: 'admin',
      sshPassword: 'toor',
      sshHostname: '127.0.0.1',
      sshPublicKey: 'publickey'
    }
    // this.newPrinter.printName = 'Test2';
  }

  // loadMember() {
  //   this.memberService.getMemberPrinters(this.user.username).subscribe(member => {
  //     this.member = member;
  //   })
  // }


  loadPrinters() {
    this.memberService.setUserParams(this.userParams);

    this.memberService.getPaginatedPrinters(this.userParams).subscribe(response => {
      this.printers = response.result;
      this.pagination = response.pagination;
      // console.log("Pagination headers returned:")
      // console.log(this.pagination);
    })

    // this.memberService.getMemberPrintJobs(this.user.username).subscribe(member => {
    //   this.member = member;
    // })
  }


  // loadPrinters() {
  //   this.memberService.getMemberPrinters().subscribe(response => {
  //     this.printers = response;
  //   })
  // }

  createPrinter() {
    this.addPrinter = !this.addPrinter;
  }

  submitcreatePrinter() {
    // console.log(this.newPrinter);
    this.memberService.addPrinter(this.newPrinter).subscribe(response => {
    //   // console.log(response);
      this.toastr.success('Added printer ' + this.newPrinter.printerName);
      this.createPrinter();
      this.loadPrinters();
    });
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.userParams.printerPageNumber = event.page; //.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    this.loadPrinters();
  }

  // openModal(template: TemplateRef<any>, printJob: PrintJob) {
  openModal(template: TemplateRef<any>, printer: Printers) {
    const config = {
      class: 'modal-dialog-centered',
      stuff: 'test',
      
      initialState: {
        title2: 'Test Data',
        printJob2: printer
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
      title: 'Current printer url: ' + printer.url,
      printer
    };

    // this.bsModalRef = this.modalService.show(template, {class: 'modal-sm'});
    this.bsModalRef = this.modalService.show(PrinterAdminStatusDialogComponent, {initialState});

    // this.bsModalRef.content.title = 'Current Job Status: ' + printJob.jobStatus;
    
    this.bsModalRef.content.printerName = printer.printerName;
    // this.bsModalRef = this.modalService.show(template, {initialState});

    // this.bsModalRef.content.onClose.subscribe(result => {
    //   console.log('results', result);
    // })

    this.bsModalRef.content.onClosePrintJob.subscribe((result: string) => {
      // console.log('printJob', printJob.jobStatus);
      // console.log('results', result);   // result= string of printJob.jobStatus
      if(result === 'Delete') {
        // console.log(result);
        this.memberService.deletePrinter(printer.id).subscribe(response => {
        //   // console.log(response);
          this.loadPrinters();
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
