export class Log {
    id: number;
    message: string;
    level: string;
    timestamp: Date;
    exception: string;
    userName: string;
    operation: string;
    jsonObject: string;
    jsonObjectObject: object;
    
    constructor(data?: Partial<Log>) {
        Object.assign(this, data);
        this.id = (+this.id || 0);
        this.timestamp =  new Date(this.timestamp);
        this.jsonObjectObject = JSON.parse(this.jsonObject);
    }
}