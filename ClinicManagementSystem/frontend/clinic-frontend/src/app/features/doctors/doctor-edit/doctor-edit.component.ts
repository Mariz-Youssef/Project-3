import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DoctorService } from '../services/doctor.service';

interface Doctor {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  specialization: string;
  qualifications: string;
  experienceYears: number;
  consultationFee: number;
  isAvailable: boolean;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
}

interface DoctorCreateDTO {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  specialization: string;
  qualifications: string;
  experienceYears: number;
  consultationFee: number;
  isAvailable: boolean;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
}

@Component({
  selector: 'app-doctor-edit',
  templateUrl: './doctor-edit.component.html',
  styleUrls: ['./doctor-edit.component.scss']
})
export class DoctorEditComponent implements OnInit {
  doctorForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  doctorId: number | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private doctorService: DoctorService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.doctorId = Number(this.route.snapshot.paramMap.get('id'));

    this.doctorForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      specialization: ['', Validators.required],
      qualifications: [''],
      experienceYears: [''],
      consultationFee: [''],
      isAvailable: [false],
      address: [''],
      city: [''],
      state: [''],
      zipCode: ['']
    });

    // Load doctor data if ID is present
    if (this.doctorId) {
      this.loadDoctor(this.doctorId);
    }
  }

  // convenience getter for easy access to form fields
  get f() { return this.doctorForm.controls; }

  private loadDoctor(id: number) {
    this.loading = true;
    this.doctorService.getDoctorById(id)
      .subscribe({
        next: (doctor) => {
          this.doctorForm.patchValue(doctor);
          this.loading = false;
        },
        error: error => {
          this.error = error;
          this.loading = false;
        }
      });
  }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.error = '';

    // stop here if form is invalid
    if (this.doctorForm.invalid) {
      return;
    }

    this.loading = true;
    const doctorData = this.doctorForm.value;

    if (this.doctorId) {
      this.doctorService.updateDoctor(this.doctorId, doctorData)
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
  }

  onCancel() {
    this.router.navigate(['/doctors/list']);
  }
}