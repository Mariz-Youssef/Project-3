import { Component, OnInit, ViewChild } from '@angular/core';
import { DoctorService } from '../services/doctor.service';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';

export interface Doctor {
  id: number;
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
  selector: 'app-doctor-list',
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.scss']
})
export class DoctorListComponent implements OnInit {
  displayedColumns: string[] = ['select', 'id', 'firstName', 'lastName', 'email', 'phone', 'specialty', 'experienceYears', 'consultationFee', 'isAvailable', 'actions'];
  dataSource = new MatTableDataSource<Doctor>();
  selection = new SelectionModel<Doctor>(true, []);
  loading = false;
  error = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private doctorService: DoctorService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors() {
    this.loading = true;
    this.doctorService.getDoctors()
      .subscribe({
        next: (doctors) => {
          this.dataSource.data = doctors;
          this.loading = false;
        },
        error: error => {
          this.error = 'Failed to load doctors';
          this.loading = false;
          console.error('Error loading doctors:', error);
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
  checkboxLabel(row?: Doctor): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }

  deleteDoctor(id: number) {
    if (confirm('Are you sure you want to delete this doctor?')) {
      this.loading = true;
      this.doctorService.deleteDoctor(id)
        .subscribe({
          next: () => {
            this.loadDoctors(); // Refresh the list
          },
          error: error => {
            this.error = 'Failed to delete doctor';
            this.loading = false;
            console.error('Error deleting doctor:', error);
          }
        });
    }
  }

  viewDoctor(id: number) {
    this.router.navigate(['/doctors/view', id]);
  }

  editDoctor(id: number) {
    this.router.navigate(['/doctors/edit', id]);
  }

  createDoctor() {
    this.router.navigate(['/doctors/create']);
  }
}