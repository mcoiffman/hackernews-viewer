import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Story } from '../../models/story.model';

@Injectable({
  providedIn: 'root'
})
export class StoryService {
  private apiUrl = 'https://localhost:7192/api/stories';

  constructor(private http: HttpClient) {}

  getStories(): Observable<Story[]> {
    return this.http.get<Story[]>(this.apiUrl);
  }
}
