import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { AuditLogComponent } from '../lib/audit-log/audit-log.component';
import { ApiLogComponent } from '../public-api';
import { MaterialModule } from './material.module';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { LogsComponent } from './logs/logs-view.component';

@NgModule({
  declarations: [
    ApiLogComponent,
    AuditLogComponent,
    LogsComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    HttpClientModule,
    NgxJsonViewerModule
  ],
  exports: [
    ApiLogComponent,
    AuditLogComponent,
    LogsComponent
  ]
})
export class NgxRegistriLogModule { }
