import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

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

  constructor(private http: HttpClient) { }

  // :returns an Observable of type Member []Array
  //getMembers(): Observable<Member[]> {
  getMembers() {
    //return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
    //return this.http.get<Member[]>(this.baseUrl + 'users');

    if (this.members.length > 0) 
      return of(this.members);  // return an observable of members

    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    )
    
  }

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

  updateMember(member: Member) {
    //return this.http.put(this.baseUrl + 'users', member);
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }
}
