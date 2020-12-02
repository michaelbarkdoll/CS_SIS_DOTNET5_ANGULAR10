import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { PrintJob } from 'src/app/_models/printjob';

@Component({
  selector: 'app-printer-admin-status-dialog',
  templateUrl: './printer-admin-status-dialog.component.html',
  styleUrls: ['./printer-admin-status-dialog.component.css']
})
export class PrinterAdminStatusDialogComponent implements OnInit {
  public onClose: Subject<boolean>;
  public onClosePrintJob: Subject<string>;
  title: string;
  closeBtnName: string;
  list: string[];
  printStatus: string;
  // jobStatus: string;
  printerName: string;

  // printStatusList = [{value: 'Held', display: 'Held'}, {value: 'Queued', display: 'Queued'}, {value: 'Cancelled', display: 'Cancelled'}, {value: 'Completed', display: 'Completed'}];

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

  returnPrintJob(printerStatus: string) {
    this.onClosePrintJob.next(printerStatus);
    this.bsModalRef.hide();
  }
 
  decline(): void {
    // this.message = 'Declined!';
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

}
