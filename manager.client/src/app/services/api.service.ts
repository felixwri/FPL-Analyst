import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private httpClient: HttpClient) { }

  get(url: string) {
    return this.httpClient.get(url) as Observable<any>;
  }

  post(url: string, body: any) {
    return this.httpClient.post(url, body) as Observable<any>;
  }
}
