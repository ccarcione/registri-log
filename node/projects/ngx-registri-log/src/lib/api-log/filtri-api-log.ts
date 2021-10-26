import { Pagination } from '../shared/pagination';

export class FiltriApiLog {
    dataDa: Date | null;
    dataA: Date | null;
    repartoProduzioneId:number;
    paginazione: Pagination;
    desc: boolean;
    orderColumn: string;

    constructor(data?: Partial<FiltriApiLog>) {
        Object.assign(this, data);
        this.dataDa =  new Date(this.dataDa);
        this.dataA =  new Date(this.dataA);
        this.repartoProduzioneId = (this.repartoProduzioneId || 0);
    }
}