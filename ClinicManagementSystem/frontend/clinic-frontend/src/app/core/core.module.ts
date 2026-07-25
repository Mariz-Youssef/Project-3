import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppLayoutComponent } from './layouts/app-layout.component';

@NgModule({
  declarations: [
    AppLayoutComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    AppLayoutComponent
  ]
})
export class CoreModule { }
