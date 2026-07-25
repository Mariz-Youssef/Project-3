import { Component } from '@angular/core';

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss']
})
export class AppLayoutComponent {
  sidebarOpened = true;

  toggleSidebar() {
    this.sidebarOpened = !this.sidebarOpened;
  }

  getUserName(): string {
    // This would come from auth service in a real implementation
    return localStorage.getItem('currentUser') ? 
      JSON.parse(localStorage.getItem('currentUser') || '{}').firstName || 'User' : 
      'Guest';
  }
}
