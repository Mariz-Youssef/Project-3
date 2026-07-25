import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientViewComponent } from './patient-view.component';
import { RouterModule } from '@angular/router';

// Angular Material Modules
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  declarations: [
    PatientViewComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatProgressBarModule
  ]
})
export class PatientViewModule { }