import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {

  readonly _url = "https://localhost:7009" 
  constructor(private http: HttpClient) { }

  getUserList():Observable<any[]> {
   return this.http.get<any>(this._url + '/user');
  }

  addUser(user : User): Observable<any> {
    return this.http.post<any>(this._url + '/user', user);
   }

   updateUser(id:number | string, data:any) {
    return this.http.put(this._url + `/user/${id}`, data);
   }

   deleteUser(id:any) :Observable<any> {
    return this.http.delete(this._url + `/user/${id}`);
   }
  //  login(){
  //    return this.http.post (this._url + '/login', user)
  //  }
}
