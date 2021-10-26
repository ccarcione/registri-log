import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { AuditLogComponent } from '../lib/audit-log/audit-log.component';
import { ApiLogComponent } from '../public-api';
import { MaterialModule } from './material.module';
import { NgxJsonViewerModule } from 'ngx-json-viewer';

@NgModule({
  declarations: [
    ApiLogComponent,
    AuditLogComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    HttpClientModule,
    NgxJsonViewerModule
  ],
  exports: [
    ApiLogComponent,
    AuditLogComponent
  ]
})
export class NgxRegistriLogModule { }
