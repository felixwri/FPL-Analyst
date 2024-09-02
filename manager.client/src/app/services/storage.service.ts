import { Injectable } from '@angular/core';
import { TeamData } from '../../types';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  setObject(key: string, value: Object) {
    localStorage.setItem(key, JSON.stringify(value));
  }

  getObject(key: string): any {
    return JSON.parse(localStorage.getItem(key) as string);
  }

  setTeamData(value: TeamData) {
    this.setObject("teamData", value);
  }

  getTeamData(): TeamData {
    return this.getObject("teamData");
  }
}
