import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PatientService } from '../services/patient.service';

@Component({
  selector: 'app-patient-create',
  templateUrl: './patient-create.component.html',
  styleUrls: ['./patient-create.component.scss']
})
export class PatientCreateComponent implements OnInit {
  patientForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private patientService: PatientService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.patientForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required],
      address: [''],
      city: [''],
      state: [''],
      zipCode: ['']
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.patientForm.controls; }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.error = '';

    // stop here if form is invalid
    if (this.patientForm.invalid) {
      return;
    }

    this.loading = true;
    this.patientService.createPatient(this.patientForm.value)
      .subscribe({
        next: () => {
          // navigate to patient list on success
          this.router.navigate(['/patients/list']);
        },
        error: error => {
          this.error = error;
          this.loading = false;
        }
      });
  }

  onCancel() {
    this.router.navigate(['/patients/list']);
  }
}
