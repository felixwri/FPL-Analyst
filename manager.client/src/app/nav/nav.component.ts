import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DataLoaderService } from '../services/data-loader.service';
import { Router } from '@angular/router';
import { TeamData } from '../../types';
import { StorageService } from '../services/storage.service';

@Component({
  selector: 'navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  teamName: string | null = null;
  teamData: TeamData | null = null;
  personalLink: string = "/personal";
  leagueLink: string = "/league";
  linkActive: "active" | "inactive" = "inactive";
  sidebarActive: boolean = false;

  constructor(
    private dataLoader: DataLoaderService,
    private router: Router,
    private storageService: StorageService
  ) { }
  
  ngOnInit(): void {
    this.dataLoader.observer.subscribe(data => {
      this.teamName = data?.name || null;
      this.teamData = data;
      if (this.teamData) this.updateLinks();
    });

    if (!this.teamData) {
      this.teamData = this.storageService.getTeamData();
      if (this.teamData) {
        this.updateLinks();
        this.teamName = this.teamData.name;
      }
    }
  }

  updateLinks(): void {
    this.personalLink = `/personal/${this.teamData?.id}`;
    this.leagueLink = `/league`;
    this.linkActive = "active";
  }

  goToHome(): void {
    this.router.navigate(['/']);
  }
}
