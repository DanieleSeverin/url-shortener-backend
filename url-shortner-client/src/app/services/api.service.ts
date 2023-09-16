import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  get(url: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${url}`);
  }

  post(url: string, payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/${url}`, payload);
  }

  put(url: string, payload: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${url}`, payload);
  }

  delete(url: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${url}`);
  }
}
