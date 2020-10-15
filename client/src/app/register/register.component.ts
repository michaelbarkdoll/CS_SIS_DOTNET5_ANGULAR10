import { registerLocaleData } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  // model: any = {};
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, private toastr: ToastrService, 
    private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    // this.registerForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')])
    // })
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)] ],
      confirmPassword: [ '', Validators.required ]
      // confirmPassword: [ '', [Validators.required, this.matchValues('password')] ]
    }, {
      validators: this.passwordMatchValidator
    });
    //})
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      // control?.value is the confirmPassword control
      // matchTo is the 'password' control that we're going to compare this value to..
      // if successful match we return null; if fails to match we attach isMatching = true error so that the form validator knows it didnt pass.
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching: true}
    }
  }

  // Validator will be run when any input changes on the form.
  passwordMatchValidator(g: FormGroup): void {
    const password = g.controls.password;
    const confirmPassword = g.controls.confirmPassword;
 
    if (password.value !== confirmPassword.value) {
      confirmPassword.setErrors({isMatching: true});
    } else if ((password.value === "" && confirmPassword.value === "") ) {
      confirmPassword.setErrors({required: true});
    } else {
      confirmPassword.setErrors(null);
    }
  }

  register() {
    // console.log(this.registerForm.value);
    // console.log(this.model);
    //this.accountService.register(this.model).subscribe(response => {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      // console.log(response);
      //this.cancel();
      this.router.navigateByUrl('/members');
    }, error => {
      console.log(error);
      // this.toastr.error(error.error);
      this.validationErrors = error;
    })
  }

  cancel() {
    console.log('cancelled');
    this.cancelRegister.emit(false);
  }
}
