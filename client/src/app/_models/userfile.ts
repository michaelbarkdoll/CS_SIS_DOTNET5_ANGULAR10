export interface UserFile {
    id?: number;
    url?: string;
    description?: string;
    dateAdded: Date;
    isThesis: boolean;
    isProject: boolean;
    isPrintJob: boolean;
    isOther: boolean;
    publicId: string;
    fileName: string;
}