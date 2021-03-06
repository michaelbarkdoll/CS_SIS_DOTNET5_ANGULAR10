import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, pipe } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ContainerJob } from '../_models/containerjob';
import { Member } from '../_models/member';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { Printers } from '../_models/printer';
import { PrintJob } from '../_models/printjob';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  printerCache = new Map();
  printerUsersCache = new Map();
  dockerUsersCache = new Map();
  printersCache = new Map();
  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    })
   }

   getUserParams() {
     return this.userParams;
   }

   setUserParams(params: UserParams) {
     this.userParams = params;
   }

   resetUserParams() {
     this.userParams = new UserParams(this.user);
     return this.userParams;
   }

  // :returns an Observable of type Member []Array
  //getMembers(): Observable<Member[]> {
    /*
  getMembers() {
    //return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
    //return this.http.get<Member[]>(this.baseUrl + 'users');

    // Caching of results
    if (this.members.length > 0) 
      return of(this.members);  // return an observable of members

    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    )
    
  }*/

  //getMembers(page?: number, itemsPerPage?: number) {
  getMembers(userParams: UserParams) {

    // console.log(Object.values(userParams).join('-'));
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response) {
      return of(response);
    }

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('Gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    

    // Previous get method would get body.  Now that we're passing up the params we'll get the full response back and we need to look into the body to get the body.
    //return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    
    // return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    //   .pipe(map(response => {
    //     this.memberCache.set(Object.values(userParams).join('-'), response);
    //     return response;
    //   }))

    return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
    
  }

  getMembersURLRequests(userParams: UserParams) {

    // console.log(Object.values(userParams).join('-'));
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    // if(response) {
    //   return of(response);
    // }

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.urlRequestsPageSize);

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

    return getPaginatedResult<Member[]>(this.baseUrl + 'users/url-requests', params, this.http)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
    
  }

  getMember(username: string) {
    // console.log(this.memberCache);
    // ... spread operator
    //const member = [...this.memberCache.values()];
    //console.log(member);
    
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.username === username);

    if(member) {
      return of(member);
    }


    
    // console.log(member);

    // Make the API call if we don't have the member
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  //Retreive your userfiles with member
  getMemberFiles(username: string) {    
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.username === username);

    if(member) {
      return of(member);
    }
    // console.log(member);

    // Make the API call if we don't have the member
    return this.http.get<Member>(this.baseUrl + 'users/get-user-files-by-token');
  }

  //Admin method
  getMemberNameFiles(username: string) {    
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.username === username);

    if(member) {
      return of(member);
    }
    // console.log(member);

    // Make the API call if we don't have the member
    return this.http.get<Member>(this.baseUrl + 'admin/user-files' + username);
  }
/*
  getMember(username: string) {
    const member = this.members.find(x => x.username === username);

    // if find found a member
    if (member !== undefined) {
      return of(member);    // return an observable of member
    }

    // Make the API call if we don't have the member
    //return this.http.get<Member>(this.baseUrl + 'users/' + username, httpOptions);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }
*/

getMemberPrintJobs(username: string) {
  // console.log(this.memberCache);
  // ... spread operator
  //const member = [...this.memberCache.values()];
  //console.log(member);
  
  const member = [...this.memberCache.values()]
    .reduce((arr, elem) => arr.concat(elem.result), [])
    .find((member: Member) => member.username === username);

  if(member) {
    return of(member);
  }


  
  // console.log(member);

  // Make the API call if we don't have the member
  // return this.http.get<Member>(this.baseUrl + 'users/get-user-printjobs/' + username);
  return this.http.get<Member>(this.baseUrl + 'printer/get-user-printjobs/' + username);
}


getMemberSshKeys() {
  // Make the API call if we don't have the member
  return this.http.get<Member>(this.baseUrl + 'users/get-user-ssh-keys');
}


//getMembers(page?: number, itemsPerPage?: number) {
getMemberPaginatedPrintJobs(userParams: UserParams) {

  // console.log(Object.values(userParams).join('-'));
  var response = this.printerCache.get(Object.values(userParams).join('-'));
  if(response) {
    return of(response);
  }

  let params = this.getPaginationHeaders(userParams.printJobPageNumber, userParams.printJobPageSize);

  params = params.append('printStatus', userParams.printStatus.toString());
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
  return getPaginatedResult<PrintJob[]>(this.baseUrl + 'printer/get-user-paged-printjobs', params, this.http)
    .pipe(map(response => {
      this.printerCache.set(Object.values(userParams).join('-'), response);
      return response;
    }))
    
}

getMembersPaginatedPrintJobs(userParams: UserParams) {

  // console.log(Object.values(userParams).join('-'));

  // Disabled cache..
  // var response = this.printerUsersCache.get(Object.values(userParams).join('-'));
  // if(response) {
  //   return of(response);
  // }

  let params = this.getPaginationHeaders(userParams.printJobPageNumber, userParams.printJobPageSize);

  params = params.append('printStatus', userParams.printStatus.toString());
  params = params.append('searchUser', userParams.searchUser.toString());
  params = params.append('searchPrinter', userParams.searchPrinter.toString());
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
  return getPaginatedResult<PrintJob[]>(this.baseUrl + 'printer/get-users-paged-printjobs', params, this.http)
    .pipe(map(response => {
      this.printerUsersCache.set(Object.values(userParams).join('-'), response);
      return response;
    }))
  
}

getMembersPaginatedContainerJobs(userParams: UserParams) {

  // console.log(Object.values(userParams).join('-'));

  // Disabled cache..
  // var response = this.printerUsersCache.get(Object.values(userParams).join('-'));
  // if(response) {
  //   return of(response);
  // }

  let params = this.getPaginationHeaders(userParams.containerJobPageNumber, userParams.containerJobPageSize);

  // params = params.append('printStatus', userParams.printStatus.toString());
  params = params.append('containerStatus', userParams.printStatus.toString());
  params = params.append('searchUser', userParams.searchUser.toString());
  params = params.append('searchPrinter', userParams.searchPrinter.toString());
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
  // return getPaginatedResult<PrintJob[]>(this.baseUrl + 'printer/get-users-paged-printjobs', params, this.http)
  return getPaginatedResult<ContainerJob[]>(this.baseUrl + 'docker/get-user-paged-containerjobs', params, this.http)
    .pipe(map(response => {
      this.dockerUsersCache.set(Object.values(userParams).join('-'), response);
      return response;
    }))
  
}

getPaginatedPrinters(userParams: UserParams) {

  let params = this.getPaginationHeaders(userParams.printerPageNumber, userParams.printerPageSize);

  return getPaginatedResult<Printers[]>(this.baseUrl + 'printer/get-paged-printers', params, this.http)
    .pipe(map(response => {
      this.printersCache.set(Object.values(userParams).join('-'), response);
      return response;
    }))
  
}

getMemberPrintQuota() {
  // getMemberPrinters() {
  // console.log(this.memberCache);
  // ... spread operator
  //const member = [...this.memberCache.values()];
  //console.log(member);
  
  // const member = [...this.memberCache.values()]
  //   .reduce((arr, elem) => arr.concat(elem.result), [])
  //   .find((member: Member) => member.username === username);

  // if(member) {
  //   return of(member);
  // }
  
  // console.log(member);

  // Make the API call if we don't have the member
  // return this.http.get<Printers[]>(this.baseUrl + 'users/get-user-printers/' + username);
  // return this.http.get<Printers[]>(this.baseUrl + 'printer/get-user-printers/' + username);
  return this.http.get<Member>(this.baseUrl + 'printer/get-user-printquota');
  // return this.http.get<Printers[]>(this.baseUrl + 'printer/get-printers');
}

getMemberPrinters() {
  // getMemberPrinters() {
  // console.log(this.memberCache);
  // ... spread operator
  //const member = [...this.memberCache.values()];
  //console.log(member);
  
  // const member = [...this.memberCache.values()]
  //   .reduce((arr, elem) => arr.concat(elem.result), [])
  //   .find((member: Member) => member.username === username);

  // if(member) {
  //   return of(member);
  // }
  
  // console.log(member);

  // Make the API call if we don't have the member
  // return this.http.get<Printers[]>(this.baseUrl + 'users/get-user-printers/' + username);
  // return this.http.get<Printers[]>(this.baseUrl + 'printer/get-user-printers/' + username);
  return this.http.get<Printers[]>(this.baseUrl + 'printer/get-printers');

  // return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http)
  //     .pipe(map(response => {
  //       this.memberCache.set(Object.values(userParams).join('-'), response);
  //       return response;
  //     }))
}

  updateMember(member: Member) {
    //return this.http.put(this.baseUrl + 'users', member);
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  updateMemberURLRequest(member: Member) {

    //return this.http.put(this.baseUrl + 'users', member);
    return this.http.put(this.baseUrl + 'users/update-url', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  updateMemberSshKeys(member: Member) {

    //return this.http.put(this.baseUrl + 'users', member);
    return this.http.put(this.baseUrl + 'users/update-ssh-keys', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  generateMemberSshKeys() {
    return this.http.get<Member>(this.baseUrl + 'users/generate-new-ssh-keys');
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {})
  }

  // getLikes(predicate: string) {
  //   return this.http.get<Partial<Member[]>>(this.baseUrl + 'likes?predicate=' + predicate); // liked or likedBy
  // }
  getLikes(predicate: string, pageNumber, pageSize) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);

    return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params, this.http);
    //return this.getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params);
    //return this.http.get<Partial<Member[]>>(this.baseUrl + 'likes?predicate=' + predicate); // liked or likedBy
  }

  setContainerJobRunning(containerJobId: number) {
    return this.http.put(this.baseUrl + 'docker/run-container-job/' + containerJobId, {});
  }

  setContainerJobStarting(containerJobId: number) {
    return this.http.put(this.baseUrl + 'docker/start-container-job/' + containerJobId, {});
  }

  setContainerJobCreating(containerJobId: number) {
    return this.http.put(this.baseUrl + 'docker/create-container-job/' + containerJobId, {});
  }

  setContainerJobStop(containerJobId: number) {
    return this.http.put(this.baseUrl + 'docker/stop-container-job/' + containerJobId, {});
  }

  setContainerJobDelete(containerJobId: number) {
    return this.http.put(this.baseUrl + 'docker/delete-container/' + containerJobId, {});
  }

  setPrintJobHeld(printJobId: number) {
    return this.http.put(this.baseUrl + 'printer/pause-print-job/' + printJobId, {});
  }

  setPrintJobQueued(printJobId: number) {
    return this.http.put(this.baseUrl + 'printer/queue-print-job/' + printJobId, {});
  }

  setPrintJobCompleted(printJobId: number) {
    return this.http.put(this.baseUrl + 'printer/complete-print-job/' + printJobId, {});
  }

  setPrintJobCancel(printJobId: number) {
    return this.http.put(this.baseUrl + 'printer/cancel-print-job/' + printJobId, {});
  }

  setPrintJobDelete(printJobId: number) {
    return this.http.put(this.baseUrl + 'printer/delete-print-job/' + printJobId, {});
  }

  addPrinter(printer: Printers) {
    console.log("Inside addPrinter:");
    console.log(printer);
    return this.http.post(this.baseUrl + 'printer/add-printer', printer);
    // return this.http.post(this.baseUrl + 'printer/add-printer/' + printer, {})
  }

  deletePrinter(printerId: number) {
    return this.http.delete(this.baseUrl + 'printer/delete-printer/' + printerId, {});
  }

  // Previous get method would get body.  Now that we're passing up the params we'll get the full response back and we need to look into the body to get the body.
  private getPaginatedResult<T>(url, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );

    // return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
    //   map(response => {
    //     this.paginatedResult.result = response.body;
    //     if (response.headers.get('Pagination') !== null) {
    //       this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
    //     }
    //     return this.paginatedResult;
    //   })
    // );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
  }
}
