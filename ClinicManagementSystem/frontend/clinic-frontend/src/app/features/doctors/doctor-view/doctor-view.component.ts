import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-doctor-view',
  templateUrl: './doctor-view.component.html',
  styleUrls: ['./doctor-view.component.scss']
})
export class DoctorViewComponent implements OnInit {
  doctor: Doctor | null = null;
  loading = false;
  error = '';

  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const doctorId = Number(this.route.snapshot.paramMap.get('id'));
    if (doctorId) {
      this.loadDoctor(doctorId);
    }
  }

  private loadDoctor(id: number) {
    this.loading = true;
    this.doctorService.getDoctorById(id)
      .subscribe({
        next: (doctor) => {
          this.doctor = doctor;
          this.loading = false;
        },
        error: error => {
          this.error = error;
          this.loading = false;
        }
      });
  }

  onEdit() {
    this.router.navigate(['/doctors/edit', this.doctor?.id]);
  }

  onBack() {
    this.router.navigate(['/doctors/list']);
  }
}