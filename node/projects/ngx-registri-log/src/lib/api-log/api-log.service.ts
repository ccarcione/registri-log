import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginationResult } from '../shared/pagination-result';
import { ApiLog } from './api-log';
import { FiltriApiLog } from './filtri-api-log';
const url = 'api/ApiLog';

@Injectable({
  providedIn: 'root'
})
export class ApiLogService {

  constructor(private http: HttpClient) { }

  async get(filtri:FiltriApiLog): Promise<PaginationResult<ApiLog>> {
    let apiLogList = await this.http.post<PaginationResult<ApiLog>>(`${url}/GetPaginazione`, filtri, {
      params:
      {
        page: filtri.paginazione.pageIndex.toString(),
        pageCount: filtri.paginazione.pageSize.toString()
      }
    }).toPromise();

    return apiLogList;
  }
}
