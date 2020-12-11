import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { env } from 'process';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { SemesterManagementComponent } from '../admin/semester-management/semester-management.component';
import { Member } from '../_models/member';
import { MemberAdminView } from '../_models/memberadminview';
import { Semester } from '../_models/semester';
import { User } from '../_models/user';
import { UserFile } from '../_models/userfile';
import { UserParams } from '../_models/userParams';
import { getPaginatedResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  accessUsersCache = new Map();
  semesterCache = new Map();
  semesterClasslistCache = new Map();


  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }

  getMembersPaginatedAccessList(userParams: UserParams) {

    // console.log(Object.values(userParams).join('-'));
  
    // Disabled cache..
    var response = this.accessUsersCache.get(Object.values(userParams).join('-'));
    // if(response) {
    //   return of(response);
    // }
  
    let params = this.getPaginationHeaders(userParams.accessPageNumber, userParams.accessPageSize);
  
    // params = params.append('printStatus', userParams.printStatus.toString());
    params = params.append('searchUser', userParams.searchUser.toString());
    // params = params.append('searchPrinter', userParams.searchPrinter.toString());
    // params = params.append('minAge', userParams.minAge.toString());
    // params = params.append('maxAge', userParams.maxAge.toString());
    // params = params.append('Gender', userParams.gender);
    // params = params.append('orderBy', userParams.orderBy);
    
  
    // Previous get method would get body.  Now that we're passing up the params we'll get the full response back and we need to look into the body to get the body.
    //return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    
    // return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    //   .pipe(map(response => {
    //     this.memberCache.set(Object.values(userParams).join('-'), response);
    //     return response;
    //   }))
  
    // return getPaginatedResult<Member[]>(this.baseUrl + 'printer/get-user-paged-printjobs/admin', params, this.http)
    // return getPaginatedResult<PrintJob[]>(this.baseUrl + 'printer/get-user-paged-printjobs/admin', params, this.http)

    //admin/users-with-roles
    return getPaginatedResult<Partial<MemberAdminView[]>>(this.baseUrl + 'admin/get-users-paged-accesslist', params, this.http)
      .pipe(map(response => {
        this.accessUsersCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
    
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }

  updateMemberAccess(member: MemberAdminView) {

    //return this.http.put(this.baseUrl + 'users', member);
    return this.http.put(this.baseUrl + 'admin/update-access', member);
  }

  getSemesters() {
    return this.http.get<Partial<Semester[]>>(this.baseUrl + 'courses/get-semesters');
  }


  // getMembersPaginatedAccessList(userParams: UserParams) {
  getSemestersPaginated(userParams: UserParams) {

    // console.log(Object.values(userParams).join('-'));
  
    // Disabled cache..
    var response = this.semesterCache.get(Object.values(userParams).join('-'));
    // if(response) {
    //   return of(response);
    // }
  
    let params = this.getPaginationHeaders(userParams.semesterPageNumber, userParams.semesterPageSize);

    params = params.append('searchYear', userParams.searchYear.toString());
    params = params.append('searchTerm', userParams.searchTerm.toString());
    
  
    // Previous get method would get body.  Now that we're passing up the params we'll get the full response back and we need to look into the body to get the body.
    //return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    
    // return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    //   .pipe(map(response => {
    //     this.memberCache.set(Object.values(userParams).join('-'), response);
    //     return response;
    //   }))
  
    // return getPaginatedResult<Member[]>(this.baseUrl + 'printer/get-user-paged-printjobs/admin', params, this.http)
    // return getPaginatedResult<PrintJob[]>(this.baseUrl + 'printer/get-user-paged-printjobs/admin', params, this.http)

    //admin/users-with-roles
    // return getPaginatedResult<Partial<MemberAdminView[]>>(this.baseUrl + 'admin/get-users-paged-accesslist', params, this.http)
    return getPaginatedResult<Partial<Semester[]>>(this.baseUrl + 'courses/get-semesters-paged', params, this.http)
      .pipe(map(response => {
        this.semesterCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
    
  }

  getSemesterClasslistPaginated(userParams: UserParams) {

    // console.log(Object.values(userParams).join('-'));
  
    // Disabled cache..
    var response = this.semesterClasslistCache.get(Object.values(userParams).join('-'));
    // if(response) {
    //   return of(response);
    // }
  
    let params = this.getPaginationHeaders(userParams.semesterPageNumber, userParams.semesterPageSize);

    params = params.append('semesterId', userParams.semesterId.toString());
    // params = params.append('searchYear', userParams.searchYear.toString());
    // params = params.append('searchTerm', userParams.searchTerm.toString());
    
  
    // Previous get method would get body.  Now that we're passing up the params we'll get the full response back and we need to look into the body to get the body.
    //return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    
    // return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    //   .pipe(map(response => {
    //     this.memberCache.set(Object.values(userParams).join('-'), response);
    //     return response;
    //   }))
  
    // return getPaginatedResult<Member[]>(this.baseUrl + 'printer/get-user-paged-printjobs/admin', params, this.http)
    // return getPaginatedResult<PrintJob[]>(this.baseUrl + 'printer/get-user-paged-printjobs/admin', params, this.http)

    //admin/users-with-roles
    // return getPaginatedResult<Partial<MemberAdminView[]>>(this.baseUrl + 'admin/get-users-paged-accesslist', params, this.http)
    // return getPaginatedResult<Partial<Semester[]>>(this.baseUrl + 'courses/get-semesters-paged', params, this.http)
    return getPaginatedResult<Partial<UserFile[]>>(this.baseUrl + 'admin/get-semester-classlist-files-paged', params, this.http)
      .pipe(map(response => {
        this.semesterClasslistCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
    
  }

  processClassList(publicId: string, semester: Semester) {
    // var Indata = {'product': $scope.product, 'product2': $scope.product2 };
    // var Indata = {'publicId': publicId, 'id': semester };

    // return this.http.post(this.baseUrl + 'admin/process-user-file-classlist/' + '?publicId=' + publicId + '?semesterId=' + semester.id, {});
    // return this.http.post(this.baseUrl + 'admin/process-user-file-classlist/' + Indata, {});
    return this.http.post(this.baseUrl + 'admin/process-user-file-classlist/' + publicId + '?semesterId=' + semester.id, {});
  }

  batchAddUsersFromClassList(publicId: string, semester: Semester) {
    return this.http.post(this.baseUrl + 'admin/batch-add-user-file-classlist/' + publicId + '?semesterId=' + semester.id, {});
  }

  batchAllowUsersLoginFromClassList(publicId: string) {
    return this.http.post(this.baseUrl + 'admin/batch-allow-users-login-from-classlist/' + publicId, {});
  }

  batchDisableUsersLoginFromClassList(publicId: string) {
    return this.http.post(this.baseUrl + 'admin/batch-disable-users-login-from-classlist/' + publicId, {});
  }

  batchUpdateMajorsFromClassList(publicId: string) {
    return this.http.post(this.baseUrl + 'admin/batch-update-majors-from-classlist/' + publicId, {});
  }

  createNewSemester(newSemesterYear: number, newSemesterTermName: string) {
    // var newSemester: Semester;
    let newSemester: Semester = { id: -1, term: newSemesterTermName, year: newSemesterYear, courses: null }

    newSemester.term = newSemesterTermName;
    // newSemester.term = "Spring";
    newSemester.year = newSemesterYear;

    //return this.http.put(this.baseUrl + 'courses/create-semester' + newSemester, {});
    return this.http.put(this.baseUrl + 'courses/create-semester', newSemester);
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
  }


}
