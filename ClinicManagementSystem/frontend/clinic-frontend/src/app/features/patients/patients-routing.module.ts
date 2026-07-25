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
    loadChildren: () => import('./patient-list/patient-list.module').then(m => m.PatientListModule)
  },
  {
    path: 'create',
    loadChildren: () => import('./patient-create/patient-create.module').then(m => m.PatientCreateModule)
  },
  {
    path: 'edit/:id',
    loadChildren: () => import('./patient-edit/patient-edit.module').then(m => m.PatientEditModule)
  },
  {
    path: 'view/:id',
    loadChildren: () => import('./patient-view/patient-view.module').then(m => m.PatientViewModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
