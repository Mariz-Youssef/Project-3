import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../services/patient.service';

interface Patient {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  gender: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
}

@Component({
  selector: 'app-patient-view',
  templateUrl: './patient-view.component.html',
  styleUrls: ['./patient-view.component.scss']
})
export class PatientViewComponent implements OnInit {
  patient: Patient | null = null;
  loading = false;
  error = '';

  constructor(
    private patientService: PatientService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const patientId = Number(this.route.snapshot.paramMap.get('id'));
    if (patientId) {
      this.loadPatient(patientId);
    }
  }

  private loadPatient(id: number) {
    this.loading = true;
    this.patientService.getPatientById(id)
      .subscribe({
        next: (patient) => {
          this.patient = patient;
          this.loading = false;
        },
        error: error => {
          this.error = error;
          this.loading = false;
        }
      });
  }

  onEdit() {
    this.router.navigate(['/patients/edit', this.patient?.id]);
  }

  onBack() {
    this.router.navigate(['/patients/list']);
  }
}
