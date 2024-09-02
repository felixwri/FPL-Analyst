import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { TeamData } from '../../types';

@Injectable({
  providedIn: 'root'
})
export class DataLoaderService {
  private teamData = new BehaviorSubject<TeamData | null>(null);
  public observer = this.teamData.asObservable();

  setTeamData(data: TeamData | null) {
    this.teamData.next(data);
    console.log("Updated")
  }
}
