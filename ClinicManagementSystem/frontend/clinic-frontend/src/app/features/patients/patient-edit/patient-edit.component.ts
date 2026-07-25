import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../services/patient.service';

@Component({
  selector: 'app-patient-edit',
  templateUrl: './patient-edit.component.html',
  styleUrls: ['./patient-edit.component.scss']
})
export class PatientEditComponent implements OnInit {
  patientForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  patientId: number | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private patientService: PatientService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.patientId = Number(this.route.snapshot.paramMap.get('id'));
    
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

    // Load patient data if ID is present
    if (this.patientId) {
      this.loadPatient(this.patientId);
    }
  }

  // convenience getter for easy access to form fields
  get f() { return this.patientForm.controls; }

  private loadPatient(id: number) {
    this.loading = true;
    this.patientService.getPatientById(id)
      .subscribe({
        next: (patient) => {
          this.patientForm.patchValue(patient);
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
    if (this.patientForm.invalid) {
      return;
    }

    this.loading = true;
    const patientData = this.patientForm.value;
    
    if (this.patientId) {
      this.patientService.updatePatient(this.patientId, patientData)
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
  }

  onCancel() {
    this.router.navigate(['/patients/list']);
  }
}
