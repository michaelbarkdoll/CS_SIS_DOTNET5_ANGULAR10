import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-print-job-admin-status-dialog',
  templateUrl: './print-job-admin-status-dialog.component.html',
  styleUrls: ['./print-job-admin-status-dialog.component.css']
})
export class PrintJobAdminStatusDialogComponent implements OnInit {
  public onClose: Subject<boolean>;
  public onClosePrintJob: Subject<string>;
  title: string;
  closeBtnName: string;
  list: string[];
  printStatus: string;
  jobStatus: string;
  printStatusList = [{value: 'Held', display: 'Held'}, {value: 'Queued', display: 'Queued'}, {value: 'Cancelled', display: 'Cancelled'}, {value: 'Completed', display: 'Completed'}];

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    this.onClosePrintJob = new Subject();
  }

  confirm(): void {
    this.onClose.next(true);
    // this.message = 'Confirmed!';
    this.bsModalRef.hide();
  }

  returnPrintJob(printJobStatus: string) {
    this.onClosePrintJob.next(printJobStatus);
    this.bsModalRef.hide();
  }
 
  decline(): void {
    // this.message = 'Declined!';
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

}

