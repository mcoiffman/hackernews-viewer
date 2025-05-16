import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StoryService } from '../../services/story.service';
import { Story } from '../../../models/story.model';

@Component({
  selector: 'app-story-table',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './story-table.component.html',
  styleUrls: ['./story-table.component.css']
})
export class StoryTableComponent {
  stories: Story[] = [];
  page: number = 1;
  pageSize: number = 10;
  pageSizeOptions = [5, 10, 20, 50];
  searchTerm: string = '';
  isLoading = true;

  constructor(private storyService: StoryService) {}

  ngOnInit(): void {
    this.storyService.getStories().subscribe(data => {
      this.stories = data;
      this.isLoading = false;
    });
  }

  get filteredStories(): Story[] {
    return this.stories.filter(story =>
      story.title?.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  get pagedStories(): Story[] {
    const start = (this.page - 1) * this.pageSize;
    return this.filteredStories.slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredStories.length / this.pageSize);
  }

  onPageSizeChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.pageSize = parseInt(target.value, 10);
    this.page = 1;
  }
}
