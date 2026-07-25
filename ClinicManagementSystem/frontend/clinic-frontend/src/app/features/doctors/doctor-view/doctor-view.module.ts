import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorViewComponent } from './doctor-view.component';
import { RouterModule } from '@angular/router';

// Angular Material Modules
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  declarations: [
    DoctorViewComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatProgressBarModule
  ]
})
export class DoctorViewModule { }