import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm!: FormGroup;
  submitted: boolean = false;
  
  constructor(
    private fb: FormBuilder, 
    private auth: AuthService, 
    private router: Router, 
    private userStore: UserStoreService,
    private toaster: ToastrService) { 

  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onLogin() {
    this.submitted = true;

    if(this.loginForm.valid) {
      this.submitted = true;

      this.auth.login(this.loginForm.value)
      .subscribe({
        next: (res) => {
          this.auth.storeToken(res.result.token);
          const tokenPayload = this.auth.decodeToken();
          this.userStore.setFullNameForStore(tokenPayload.unique_name);
          this.userStore.setRoleForStore(tokenPayload.role);
          this.auth.storeName(res.result.user.name);
          this.loginForm.reset();
          console.log(res.result);
          this.router.navigate(['mansion']);
        },
        error: (err) => {
          alert(err?.err.message)
        },
        complete: () => {
          this.toaster.success("Login Successful");
        }
      })
    }
  }
}