import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Components
import { AlertComponent } from './components/alert/alert.component';

// Pipes
import { DateFormatPipe } from './pipes/date-format.pipe';
import { StatusLabelPipe } from './pipes/status-label.pipe';

// Directives
import { HasPermissionDirective } from './directives/has-permission.directive';

// Angular Material modules (re-export for convenience)
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';

@NgModule({
  declarations: [
    AlertComponent,
    DateFormatPipe,
    StatusLabelPipe,
    HasPermissionDirective
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatSortModule,
    MatTableModule,
    MatToolbarModule
  ],
  exports: [
    CommonModule,
    // Components
    AlertComponent,
    // Pipes
    DateFormatPipe,
    StatusLabelPipe,
    // Directives
    HasPermissionDirective,
    // Angular Material (re-export)
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatSortModule,
    MatTableModule,
    MatToolbarModule
  ]
})
export class SharedModule { }