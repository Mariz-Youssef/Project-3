import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Doctor {
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

export interface DoctorCreateDTO {
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

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private apiUrl = `${environment.apiUrl}/doctors`;

  constructor(private http: HttpClient) { }

  getDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.apiUrl)
      .pipe(
        catchError(error => {
          console.error('Error fetching doctors:', error);
          return throwError(() => new Error('Failed to fetch doctors'));
        })
      );
  }

  getDoctorById(id: number): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(error => {
          console.error(`Error fetching doctor ${id}:`, error);
          return throwError(() => new Error(`Failed to fetch doctor ${id}`));
        })
      );
  }

  createDoctor(doctor: DoctorCreateDTO): Observable<Doctor> {
    return this.http.post<Doctor>(this.apiUrl, doctor)
      .pipe(
        catchError(error => {
          console.error('Error creating doctor:', error);
          return throwError(() => new Error('Failed to create doctor'));
        })
      );
  }

  updateDoctor(id: number, doctor: DoctorCreateDTO): Observable<Doctor> {
    return this.http.put<Doctor>(`${this.apiUrl}/${id}`, doctor)
      .pipe(
        catchError(error => {
          console.error(`Error updating doctor ${id}:`, error);
          return throwError(() => new Error(`Failed to update doctor ${id}`));
        })
      );
  }

  deleteDoctor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(error => {
          console.error(`Error deleting doctor ${id}:`, error);
          return throwError(() => new Error(`Failed to delete doctor ${id}`));
        })
      );
  }
}