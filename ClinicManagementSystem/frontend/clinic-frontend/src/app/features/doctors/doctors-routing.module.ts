import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full'
  },
  {
    path: 'list',
    loadChildren: () => import('./doctor-list/doctor-list.module').then(m => m.DoctorListModule)
  },
  {
    path: 'create',
    loadChildren: () => import('./doctor-create/doctor-create.module').then(m => m.DoctorCreateModule)
  },
  {
    path: 'edit/:id',
    loadChildren: () => import('./doctor-edit/doctor-edit.module').then(m => m.DoctorEditModule)
  },
  {
    path: 'view/:id',
    loadChildren: () => import('./doctor-view/doctor-view.module').then(m => m.DoctorViewModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DoctorsRoutingModule { }