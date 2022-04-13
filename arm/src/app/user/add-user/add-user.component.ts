import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { Data } from '@angular/router';
import { Observable } from 'rxjs';
import { UserApiService } from 'src/app/services/userApi.service';
import { User } from '../../../models/user'
import { Validators } from '@angular/forms';


@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {


  user!: User
  userForm!: FormGroup
  constructor(private fb: FormBuilder,private userapi: UserApiService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.userForm = new FormGroup({
      firstName: new FormControl(['', Validators.required,Validators.maxLength(20), Validators.minLength(2)]),
      surName: new FormControl (['', Validators.required, Validators.maxLength(20), Validators.minLength(2)]),
      email: new FormControl (['', Validators.required, Validators.email]),
      address: new FormControl (['', Validators.required, Validators.maxLength(20), Validators.minLength(2)]),
      role: new FormControl (['', Validators.required]),
      phoneNumber: new FormControl (['', Validators.required, Validators.maxLength(12), Validators.minLength(10)]),
      subscription: new FormControl (['', Validators.required]),
      dob: new FormControl(['', Validators.required]),
      postCode: new FormControl(['', Validators.minLength(2), Validators.maxLength(7)])
    })
  };
  selectSubscription(event:any): void{
    this.userForm.patchValue({
      subscription: event.target.value
    });
  }

  onSubmit() {
    this.user = this.userForm.value;
    console.log(this.user)
   return this.userapi.addUser(this.user).subscribe((response: any) =>{
      console.log("added")
    })
  }

}
