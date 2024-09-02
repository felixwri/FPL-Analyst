import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DataLoaderService } from '../services/data-loader.service';

@Component({
  selector: 'navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  teamName: string | null = null;

  constructor(private dataLoader: DataLoaderService) { }
  
  ngOnInit(): void {
    this.dataLoader.observer.subscribe(data => {
      this.teamName = data?.name || null;
    });
  }
}
