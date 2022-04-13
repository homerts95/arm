import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UserApiService } from '../../services/userApi.service'
import { faX, faEdit } from '@fortawesome/free-solid-svg-icons';
import { User } from '../../../models/user';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  user!: User;

  faX = faX;
  faEdit = faEdit;

  userList$!:Observable<any[]>;

  

  constructor(private apiService: UserApiService) { }

  ngOnInit(): void {
    this.userList$ = this.apiService.getUserList();
  }

  onDelete(user: any) {
    
   this.apiService.deleteUser(user.id).subscribe(() => {
     console.log(`deletion of ${user.id}`);
   })
  }

  onEdit():void {

  }

}
