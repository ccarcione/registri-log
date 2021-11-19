export class ApiLog {
    id: number;
    date: Date;
    userId: string;
    method: string;
    url: string;
    queryString: string;

    requestBody: string;
    requestBodyObject: object;
    /// <summary>
    /// Body size in kb.
    /// </summary>
    requestSize: number;
    
    responseBody: string;
    responseBodyObject: object;
    /// <summary>
    /// Body size in kb.
    /// </summary>
    responseSize: number;
    
    elapsedMilliseconds: number;

    constructor(data?: Partial<ApiLog>) {
        Object.assign(this, data);
        this.id = (+this.id || 0);
        if (this.requestSize != null)
            this.requestSize = +this.requestSize.toFixed(1);
        
            if (this.responseSize != null)
            this.responseSize = +this.responseSize.toFixed(1);
        
        if (this.requestBody)
            this.requestBodyObject = JSON.parse(this.requestBody);
        
        if (this.responseBody)
            this.responseBodyObject = JSON.parse(this.responseBody);
    }
}