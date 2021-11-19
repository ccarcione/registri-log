import { Pagination } from '../shared/pagination';

export class FiltriLogs {
    level: string;
    userName: string;
    operation: string;

    dataDa: Date | null;
    dataA: Date | null;

    paginazione: Pagination;
    desc: boolean;
    orderColumn: string;
    
    constructor(data?: Partial<FiltriLogs>) {
        Object.assign(this, data);
        this.dataDa =  new Date(this.dataDa);
        this.dataA =  new Date(this.dataA);
    }
}