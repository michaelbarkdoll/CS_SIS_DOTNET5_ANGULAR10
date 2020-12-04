import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { Member } from 'src/app/_models/member';
import { MemberAdminView } from 'src/app/_models/memberadminview';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AdminService } from 'src/app/_services/admin.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-user-access-management',
  templateUrl: './user-access-management.component.html',
  styleUrls: ['./user-access-management.component.css']
})
export class UserAccessManagementComponent implements OnInit {
  users: Partial<User[]>;
  
  pagination: Pagination;
  userParams: UserParams;

  // userStatusList = [{value: 'Active', display: 'Active'}, {value: 'Inactive', display: 'Inactive'}, {value: 'BAGraduated', display: 'BA Graduated'}, {value: 'BSGraduate', display: 'BS Graduate'}, {value: 'MSGraduate', display: 'MS Graduate'}, , {value: 'CYMSGraduate', display: 'CYMS Graduate'}, {value: 'PHDGraduate', display: 'PhD Graduate'}, {value: 'All', display: 'All'}];
  userStatusList = [{value: 'Active', display: 'Active'}, {value: 'Inactive', display: 'Inactive'}, {value: 'Graduate', display: 'Graduate'}, {value: 'All', display: 'All'}];
  // members: Partial<Member[]> = [];
  members: Partial<MemberAdminView[]> = [];
  bsModalRef: BsModalRef;
  
  constructor(private adminService: AdminService, private memberService: MembersService, 
      private modalService: BsModalService, private toastr: ToastrService) { 
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.getMembersPaginatedAccessList();
    // this.getUsersWithRoles();
  }

  getMembersPaginatedAccessList() {
    this.memberService.setUserParams(this.userParams);
    this.adminService.getMembersPaginatedAccessList(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
      // console.log("Pagination headers returned:")
      // console.log(this.pagination);
    })
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe(users => {
      this.users = users;
    })
    // this.printJobs = response.result;
    //   this.pagination = response.pagination;
  }

  resetFilters() {
    //this.userParams = new UserParams(this.user);
    this.userParams = this.memberService.resetUserParams();
    this.getUsersWithRoles();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.userParams.accessPageNumber = event.page; //.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    //this.pageNumber = event.page;
    // this.getUsersWithRoles();
    this.getMembersPaginatedAccessList();
  }

  openRolesModal(user: MemberAdminView) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    //this.bsModalRef.content.closeBtnName = 'Close';
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roles: [...values.filter(el => el.checked === true).map(el => el.name)]   // ... spread operation
      };
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe(() => {
          // user.roles = [...rolesToUpdate.roles]
          this.getMembersPaginatedAccessList();
        })
      }
    })
  }

  private getRolesArray(user: MemberAdminView) {
    const roles = [];
    const userRoles = user.userRoles;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin', id: 2},
      {name: 'Moderator', value: 'Moderator', id: 3},
      {name: 'Member', value: 'Member', id: 1}
    ];

    availableRoles.forEach(role => {
      let isMatch = false;
      for(const userRole of userRoles) {
        if(role.id === userRole.roleId) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if(!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }

  approve(member: MemberAdminView) {
    console.log(member);
    if(member.accessPermitted == false)
    {
      member.accessPermitted = !member.accessPermitted;

      this.adminService.updateMemberAccess(member).subscribe(response => {
        //   // console.log(response);
          this.toastr.success('Allowed access for ' + member.username);
          this.getMembersPaginatedAccessList();
        });
    }
  }

  deny(member: MemberAdminView) {
    if(member.accessPermitted == true)
    {
      member.accessPermitted = !member.accessPermitted;

      this.adminService.updateMemberAccess(member).subscribe(response => {
        //   // console.log(response);
          this.toastr.success('Restricting access for ' + member.username);
          this.getMembersPaginatedAccessList();
        });
    }
  }

}
