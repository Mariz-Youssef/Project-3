import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DoctorService } from '../services/doctor.service';
import { Router } from '@angular/router';

export interface DoctorCreateDTO {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  specialty: string;
  qualification?: string;
  experienceYears?: number;
  consultationFee?: number;
  isAvailable: boolean;
}

@Component({
  selector: 'app-doctor-create',
  templateUrl: './doctor-create.component.html',
  styleUrls: ['./doctor-create.component.scss']
})
export class DoctorCreateComponent implements OnInit {
  doctorForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private doctorService: DoctorService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.doctorForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      specialty: ['', Validators.required],
      qualification: [''],
      experienceYears: [''],
      consultationFee: [''],
      isAvailable: [true]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.doctorForm.controls; }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.error = '';

    // stop here if form is invalid
    if (this.doctorForm.invalid) {
      return;
    }

    this.loading = true;
    this.doctorService.createDoctor(this.doctorForm.value)
      .subscribe({
        next: () => {
          // navigate to doctor list on success
          this.router.navigate(['/doctors/list']);
        },
        error: error => {
          this.error = error;
          this.loading = false;
        }
      });
  }

  onCancel() {
    this.router.navigate(['/doctors/list']);
  }
}