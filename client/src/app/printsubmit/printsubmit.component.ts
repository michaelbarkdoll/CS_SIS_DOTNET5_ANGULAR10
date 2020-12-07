import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { User } from '../_models/user';
import { UserFile } from '../_models/userfile';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-printsubmit',
  templateUrl: './printsubmit.component.html',
  styleUrls: ['./printsubmit.component.css']
})
export class PrintsubmitComponent implements OnInit {
  uploader: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user: User;
  member: Member;
  fileUploaded: boolean;
  
  constructor(private accountService: AccountService, private memberService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
    this.loadMember();
    this.initializeUploader();
  }

  loadMember() {
    //this.memberService.getMember(this.user.username).subscribe(member => {
    this.memberService.getMemberFiles(this.user.username).subscribe(member => {
      this.member = member;
    })
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      // url: this.baseUrl + 'users/add-photo',
      // url: this.baseUrl + 'users/add-user-file',
      url: this.baseUrl + 'users/add-user-file-print',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['pdf'],
      // allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const userFile: UserFile = JSON.parse(response);
        this.member.userFiles.push(userFile);

        if(userFile) {
          this.fileUploaded = true;
        }
        
        // const photo: Photo = JSON.parse(response);
        // this.member.photos.push(photo);

        // if(photo.isMain) {
        //   this.user.photoUrl = photo.url;
        //   this.member.photoUrl = photo.url;
        //   this.accountService.setCurrentUser(this.user);
        // }
      }
    }
  }
}
