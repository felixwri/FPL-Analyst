import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { ServerURL } from '../../env/environment';
import { LeagueHistory, UpcomingFixtures } from '../../types';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private httpClient: HttpClient) {}

  get(url: string) {
    return this.httpClient.get(url) as Observable<any>;
  }

  post(url: string, body: any) {
    return this.httpClient.post(url, body) as Observable<any>;
  }

  getLeagueHistory(leagueId: string): Observable<LeagueHistory[]> {
    return this.httpClient.get(`${ServerURL}/league/${leagueId}/history`) as Observable<LeagueHistory[]>;
  }

  getFixtures(): Observable<UpcomingFixtures[]> {
    return this.httpClient.get(`${ServerURL}/fixture`) as Observable<UpcomingFixtures[]>;
  }

  getPicks(teamId: string): Observable<any> {
    return this.httpClient.get(`${ServerURL}/team/${teamId}/players`) as Observable<any>;
  }
}
