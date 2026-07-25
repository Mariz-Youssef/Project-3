import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusLabel'
})
export class StatusLabelPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return '';
    const status = value.toLowerCase();
    switch (status) {
      case 'active': return '<span class="badge bg-success">Active</span>';
      case 'inactive': return '<span class="badge bg-secondary">Inactive</span>';
      case 'pending': return '<span class="badge bg-warning text-dark">Pending</span>';
      case 'completed': return '<span class="badge bg-info">Completed</span>';
      case 'cancelled': return '<span class="badge bg-danger">Cancelled</span>';
      default: return `<span class="badge bg-light text-dark">${value}</span>`;
    }
  }
}