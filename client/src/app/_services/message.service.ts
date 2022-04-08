import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl= environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMessages(pageNumber, pageSize, container): Observable<PaginatedResult<Message[]>>{
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string): Observable<Message[]>{
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }
}
