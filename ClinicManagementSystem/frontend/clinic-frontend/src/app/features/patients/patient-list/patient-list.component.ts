import { Component, OnInit, ViewChild } from '@angular/core';
import { PatientService } from '../services/patient.service';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';

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

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.scss']
})
export class PatientListComponent implements OnInit {
  displayedColumns: string[] = ['select', 'id', 'firstName', 'lastName', 'email', 'phone', 'gender', 'actions'];
  dataSource = new MatTableDataSource<Patient>();
  selection = new SelectionModel<Patient>(true, []);
  loading = false;
  error = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private patientService: PatientService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadPatients();
  }

  loadPatients() {
    this.loading = true;
    this.patientService.getPatients()
      .subscribe({
        next: (patients) => {
          this.dataSource.data = patients;
          this.loading = false;
        },
        error: error => {
          this.error = 'Failed to load patients';
          this.loading = false;
          console.error('Error loading patients:', error);
        }
      });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: Patient): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }

  deletePatient(id: number) {
    if (confirm('Are you sure you want to delete this patient?')) {
      this.loading = true;
      this.patientService.deletePatient(id)
        .subscribe({
          next: () => {
            this.loadPatients(); // Refresh the list
          },
          error: error => {
            this.error = 'Failed to delete patient';
            this.loading = false;
            console.error('Error deleting patient:', error);
          }
        });
    }
  }

  viewPatient(id: number) {
    this.router.navigate(['/patients/view', id]);
  }

  editPatient(id: number) {
    this.router.navigate(['/patients/edit', id]);
  }

  createPatient() {
    this.router.navigate(['/patients/create']);
  }
}