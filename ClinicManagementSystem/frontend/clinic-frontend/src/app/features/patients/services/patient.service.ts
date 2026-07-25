import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Patient {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  gender: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
}

export interface PatientCreateDTO {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  gender: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiUrl = `${environment.apiUrl}/patients`;

  constructor(private http: HttpClient) { }

  getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.apiUrl)
      .pipe(
        catchError(error => {
          console.error('Error fetching patients:', error);
          return throwError(() => new Error('Failed to fetch patients'));
        })
      );
  }

  getPatientById(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(error => {
          console.error(`Error fetching patient ${id}:`, error);
          return throwError(() => new Error(`Failed to fetch patient ${id}`));
        })
      );
  }

  createPatient(patient: PatientCreateDTO): Observable<Patient> {
    return this.http.post<Patient>(this.apiUrl, patient)
      .pipe(
        catchError(error => {
          console.error('Error creating patient:', error);
          return throwError(() => new Error('Failed to create patient'));
        })
      );
  }

  updatePatient(id: number, patient: PatientCreateDTO): Observable<Patient> {
    return this.http.put<Patient>(`${this.apiUrl}/${id}`, patient)
      .pipe(
        catchError(error => {
          console.error(`Error updating patient ${id}:`, error);
          return throwError(() => new Error(`Failed to update patient ${id}`));
        })
      );
  }

  deletePatient(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(error => {
          console.error(`Error deleting patient ${id}:`, error);
          return throwError(() => new Error(`Failed to delete patient ${id}`));
        })
      );
  }
}