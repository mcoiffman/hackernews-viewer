import { Component } from '@angular/core';
import { StoryTableComponent } from './components/story-table/story-table.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [StoryTableComponent, FormsModule],
  template: `<app-story-table></app-story-table>`,
})
export class AppComponent {}
