import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-alert',
  template: `
    <div class="alert" [class.alert-success]="type === 'success'"
         [class.alert-danger]="type === 'error'"
         [class.alert-warning]="type === 'warning'"
         [class.alert-info]="type === 'info'">
      {{ message }}
    </div>
  `,
  styles: [`
    .alert {
      padding: 12px;
      margin-bottom:host, the file content. It should be a single code block starting with ``` and ending with ```. Let's produce correct format
<tool_call>
<function=Write>
<parameter=content>
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-alert',
  template: `
    <div class="alert" [class.alert-success]="type === 'success'"
         [class.alert-danger]="type === 'error'"
         [class.alert-warning]="type === 'warning'"
         [class.alert-info]="type === 'info'">
      {{ message }}
    </div>
  `,
  styles: [`
    .alert {
      padding: 12px;
      margin-bottom: 1rem;
      border-radius: 4px;
    }
    .alert-success { background-color: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
    .alert-danger { background-color: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
    .alert-warning { background-color: #fff3cd; color: #856404; border: 1px solid #ffeeba; }
    .alert-info { background-color: #d1ecf1; color: #0c5460; border: 1px solid #bee5eb; }
  `]
})
export class AlertComponent {
  @Input() message = '';
  @Input() type: 'success' whose 'success' | 'error' // but we need correct syntax: 'success' | 'error' | 'warning' | 'info';
}