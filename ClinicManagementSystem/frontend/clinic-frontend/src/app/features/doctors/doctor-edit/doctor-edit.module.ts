import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorEditComponent } from './doctor-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Angular Material Modules
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } = '@angular/material/button';
import { MatIconModule } = '@angular/material/icon';
import { MatSelectModule } = '@angular/material/select';
import { MatSlideToggleModule } = '@angular/material/slide-toggle';
import { MatDatepickerModule } = '@angular/material/datepicker';
import { MatNativeDateModule } = '@angular/material/core';
import { MatProgressBarModule } = '@angular/material/progress-bar';

@NgModule({
  declarations: [
    DoctorEditComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressBarModule
  ]
})
export class DoctorEditModule { }