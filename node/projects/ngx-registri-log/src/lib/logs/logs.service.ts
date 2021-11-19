import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginationResult } from '../shared/pagination-result';
import { FiltriLogs } from './filtri-logs';
import { Log } from './log';
const url = 'api/Logs';

@Injectable({
  providedIn: 'root'
})
export class LogsService {

  constructor(private http: HttpClient) { }

  async get(filtri:FiltriLogs): Promise<PaginationResult<Log>> {
    let auditList = await this.http.post<PaginationResult<Log>>(`${url}/GetPaginazione`, filtri,{
      params:
      {
        page: filtri.paginazione.pageIndex.toString(),
        pageCount: filtri.paginazione.pageSize.toString()
      }
    }).toPromise();

    return auditList;
  }
}
