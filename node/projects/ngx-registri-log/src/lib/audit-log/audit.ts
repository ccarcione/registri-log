export class Audit {
    id: number;
    userId: string;
    userName: string;
    type: string;
    tableName: string;
    dateTime: Date;
    oldValues: string;
    newValues: string;
    oldValuesObject: object;
    newValuesObject: object;
    primaryKeyObject: object;
    affectedColumns: string;
    primaryKey: string;
    
    constructor(data?: Partial<Audit>) {
        Object.assign(this, data);
        this.id = (+this.id || 0);
        this.oldValuesObject = JSON.parse(this.oldValues);
        this.newValuesObject = JSON.parse(this.newValues);
        this.primaryKeyObject = JSON.parse(this.primaryKey);
    }
}