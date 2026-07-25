import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  // Dashboard statistics - in a real app, these would come from a service
  stats = [
    {
      title: 'Total Patients',
      value: '1,234',
      icon: 'people',
      color: 'blue'
    },
    {
      title: 'Today Appointments',
      value: '24',
      icon: 'event_note',
      color: 'green'
    },
    {
      title: 'Active Doctors',
      value: '45',
      icon: 'person',
      color: 'orange'
    },
    {
      title: 'Medical Records',
      value: '3,456',
      icon: 'description',
      color: 'purple'
    }
  ];

  // Recent activities - in a real app, these would come from a service
  recentActivities = [
    {
      time: '2 minutes ago',
      description: 'New patient registered: John Doe',
      icon: 'person_add',
      color: 'success'
    },
    {
      time: '15 minutes ago',
      description: 'Appointment scheduled for Dr. Smith',
      icon: 'calendar_today',
      color: 'info'
    },
    {
      time: '30 minutes ago',
      description: 'Medical record updated for patient Jane Wilson',
      icon: 'edit',
      color: 'warning'
    },
    {
      time: '1 hour ago',
      description: 'New prescription issued for patient Bob Johnson',
      icon: 'local_pharmacy',
      color: 'secondary'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }
}
