import { Pagination } from '../shared/pagination';

export class FiltriAuditLog {
    userId: string;
    type: string;
    tableName: string;

    dataDa: Date | null;
    dataA: Date | null;

    paginazione: Pagination;
    desc: boolean;
    orderColumn: string;
    
    constructor(data?: Partial<FiltriAuditLog>) {
        Object.assign(this, data);
        this.dataDa =  new Date(this.dataDa);
        this.dataA =  new Date(this.dataA);
    }
}