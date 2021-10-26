import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginationResult } from '../shared/pagination-result';
import { Audit } from '../audit-log/audit';
import { FiltriAuditLog } from '../audit-log/filtri-audit-log';
const url = 'api/AuditLog';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {

  constructor(private http: HttpClient) { }

  async get(filtri:FiltriAuditLog): Promise<PaginationResult<Audit>> {
    let auditList = await this.http.post<PaginationResult<Audit>>(`${url}/GetPaginazione`, filtri,{
      params:
      {
        page: filtri.paginazione.pageIndex.toString(),
        pageCount: filtri.paginazione.pageSize.toString()
      }
    }).toPromise();

    return auditList;
  }
}
