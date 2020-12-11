import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Semester } from 'src/app/_models/semester';
import { User } from 'src/app/_models/user';
import { UserFile } from 'src/app/_models/userfile';
import { AccountService } from 'src/app/_services/account.service';
import { AdminService } from 'src/app/_services/admin.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-upload-classlist',
  templateUrl: './upload-classlist.component.html',
  styleUrls: ['./upload-classlist.component.css']
})
export class UploadClasslistComponent implements OnInit {
  @Input() semester: Semester;

  uploader: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user: User;
  member: Member;
  fileUploaded: boolean;

  constructor(private accountService: AccountService, private memberService: MembersService, private adminService: AdminService, private toastr: ToastrService) { 
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
      // url: this.baseUrl + 'users/add-user-file-print',
      url: this.baseUrl + 'admin/add-user-file-classlist',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedMimeType: ['text/csv', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'],
      // xdg-mime query filetype "Class List - SP21 1204_2020.csv"
      // file --mime-type -b "SIU Week 5-day Schedule.xlsx"
      // allowedFileType: ['image'],
      /*
          application
          image
          video
          audio
          pdf
          compress
          doc
          xls
          ppt
      */
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 25 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const userFile: UserFile = JSON.parse(response);
        this.member.userFiles.push(userFile);

        // Call process classlist based upon userFile
        this.adminService.processClassList(userFile.publicId, this.semester).subscribe(response2 => {
          this.toastr.success("Classlist updated successfully.");
        })

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
