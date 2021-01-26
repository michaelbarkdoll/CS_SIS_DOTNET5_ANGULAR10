import { Component, OnInit } from '@angular/core';
import { Subject } from '@microsoft/signalr';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-docker-container-status-dialog',
  templateUrl: './docker-container-status-dialog.component.html',
  styleUrls: ['./docker-container-status-dialog.component.css']
})
export class DockerContainerStatusDialogComponent implements OnInit {
  public onClose: Subject<boolean>;
  public onCloseContainerJob: Subject<string>;
  title: string;
  closeBtnName: string;
  list: string[];
  containerStatus: string;
  jobStatus: string;
  containerStatusList = [{value: 'Held', display: 'Held'}, {value: 'Queued', display: 'Queued'}, {value: 'Cancelled', display: 'Cancelled'}, {value: 'Completed', display: 'Completed'}];


  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    this.onCloseContainerJob = new Subject();
  }

  confirm(): void {
    this.onClose.next(true);
    // this.message = 'Confirmed!';
    this.bsModalRef.hide();
  }

  returnContainerJob(containerJobStatus: string) {
    this.onCloseContainerJob.next(containerJobStatus);
    this.bsModalRef.hide();
  }
 
  decline(): void {
    // this.message = 'Declined!';
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

}
