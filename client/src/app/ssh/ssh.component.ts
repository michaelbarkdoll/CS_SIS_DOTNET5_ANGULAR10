import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-ssh',
  templateUrl: './ssh.component.html',
  styleUrls: ['./ssh.component.css']
})
export class SshComponent implements OnInit {

  member: Member;
  user: User;

  oldUrl: string;
  oldRequestedURL: string;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    // this.loadMember();
    this.loadMemberWithWebSSHKeys();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
      this.oldUrl = this.member.personalURL;
      this.oldRequestedURL = this.member.requestedURL;
    })
  }

  loadMemberWithWebSSHKeys() {
    this.memberService.getMemberSshKeys().subscribe(member => {
      this.member = member;
      this.oldUrl = this.member.personalURL;
      this.oldRequestedURL = this.member.requestedURL;
    })
  }

  generateWebSSHKeys() {
    this.memberService.generateMemberSshKeys().subscribe(member => {
      this.member = member;
      this.oldUrl = this.member.personalURL;
      this.oldRequestedURL = this.member.requestedURL;
    })

    // this.loadMemberWithWebSSHKeys();
  }

  updateMemberSshKeys(member: Member) {
    // member.publicKeySSH1 = 'D1';
    // member.privateKeySSH1 = 'D1';
    // member.publicKeySSH2 = 'D1';

    this.memberService.updateMemberSshKeys(member).subscribe(response => {
      //   // console.log(response);
        this.toastr.success('Updated sshkeys for ' + member.username);
        // this.loadMemberWithWebSSHKeys();
      });
  }

}
