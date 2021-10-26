import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { BehaviorSubject } from 'rxjs';
import { Audit } from '../audit-log/audit';
import { FiltriAuditLog } from '../audit-log/filtri-audit-log';
import { Pagination } from '../shared/pagination';
import { PaginationResult } from '../shared/pagination-result';
import { AuditLogService } from '../audit-log/audit-log.service';

@Component({
  selector: 'lib-audit-log',
  templateUrl: './audit-log.component.html',
  styleUrls: ['./audit-log.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class AuditLogComponent implements OnInit, OnDestroy {
  @ViewChild('title', { static: true }) title: ElementRef;
  @ViewChild(MatPaginator) set paginator(pag: MatPaginator) {
    pag.showFirstLastButtons = true;
    pag._intl.itemsPerPageLabel = 'Elementi per pagina';
    pag._intl.firstPageLabel = 'Prima pagina';
    pag._intl.lastPageLabel = 'Ultima pagina';
    pag._intl.nextPageLabel = 'Pagina successiva';
    pag._intl.previousPageLabel = 'Pagina precedente';
    this.matPaginator = pag;
  }

  loading = false;
  filtriView: FiltriAuditLog = new FiltriAuditLog({ orderColumn:'id', desc: true, paginazione: new Pagination({ pageIndex: 1, pageSize: 10, allItemsLength: 1 }) });
  filtri = new BehaviorSubject<FiltriAuditLog>(new FiltriAuditLog(this.filtriView));
  dataSource: Audit[];
  matPaginator: MatPaginator;
  columnsToDisplay = ['id', 'dateTime', 'userName', 'type', 'tableName', 'primaryKey'];
  expandedElement: Audit | null;

  constructor(
    private auditLogService: AuditLogService,
  ) { }
  
  async ngOnInit() {
    this.loading = true;
    
    this.filtri.subscribe( async x => {
      let pagination: PaginationResult<Audit>;
      pagination = await this.auditLogService.get(this.filtriView);
      
      this.filtriView.paginazione.pageIndex = pagination.pagination.currentPage;
      this.filtriView.paginazione.pageSize = pagination.pagination.pageSize;
      this.filtriView.paginazione.allItemsLength = pagination.pagination.totalCount;

      this.dataSource = pagination.data.map(a => new Audit(a));
      
      this.loading = false;
    });
  }
  
  ngOnDestroy(): void {
  }

  changePage(pageEvent) {
    this.filtriView.paginazione.pageIndex = pageEvent.pageIndex + 1;
    this.filtriView.paginazione.allItemsLength = pageEvent.length;
    this.filtriView.paginazione.pageSize = pageEvent.pageSize;

    this.filtri.next(new FiltriAuditLog(this.filtriView));
  }
}
