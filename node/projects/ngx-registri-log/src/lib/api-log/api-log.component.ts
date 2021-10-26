import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { BehaviorSubject } from 'rxjs';
import { Pagination } from '../shared/pagination';
import { PaginationResult } from '../shared/pagination-result';
import { ApiLog } from './api-log';
import { ApiLogService } from './api-log.service';
import { FiltriApiLog } from './filtri-api-log';

@Component({
  selector: 'lib-event-api',
  templateUrl: './api-log.component.html',
  styleUrls: ['./api-log.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class ApiLogComponent implements OnInit, OnDestroy {
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
  filtriView: FiltriApiLog = new FiltriApiLog({ orderColumn:'id', desc: true, paginazione: new Pagination({ pageIndex: 1, pageSize: 10, allItemsLength: 1 }) });
  filtri = new BehaviorSubject<FiltriApiLog>(new FiltriApiLog(this.filtriView));
  dataSource: ApiLog[];
  matPaginator: MatPaginator;
  columnsToDisplay = ['id', 'date', 'method', 'url', 'elapsedMilliseconds'];
  expandedElement: ApiLog | null;
  
  constructor(
    private eventApiService: ApiLogService,
  ) { }

  async ngOnInit() {
    this.loading = true;
    
    this.filtri.subscribe( async x => {
      let pagination: PaginationResult<ApiLog>;
      pagination = await this.eventApiService.get(this.filtriView);
      
      this.filtriView.paginazione.pageIndex = pagination.pagination.currentPage;
      this.filtriView.paginazione.pageSize = pagination.pagination.pageSize;
      this.filtriView.paginazione.allItemsLength = pagination.pagination.totalCount;

      this.dataSource = pagination.data.map(a => new ApiLog(a));
      
      this.loading = false;
    });
  }

  ngOnDestroy(): void {
  }

  changePage(pageEvent) {
    this.filtriView.paginazione.pageIndex = pageEvent.pageIndex + 1;
    this.filtriView.paginazione.allItemsLength = pageEvent.length;
    this.filtriView.paginazione.pageSize = pageEvent.pageSize;

    this.filtri.next(new FiltriApiLog(this.filtriView));
  }
}
