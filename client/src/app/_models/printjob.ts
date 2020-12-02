
export interface PrintJob {
    id: number;
    jobNumber: number;
    jobOwner: string;
    jobName: string;
    jobStatus: string;
    printerName: string;
    numberOfPages: number;
    pagesPrinted: number;    
}
