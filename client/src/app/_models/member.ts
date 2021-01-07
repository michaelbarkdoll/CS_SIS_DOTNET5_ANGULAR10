import { PrintjobsComponent } from '../printjobs/printjobs.component';
import { Photo } from './photo';
import { PrintJob } from './printjob';
import { UserFile } from './userfile';

export interface Member {
    id: number;
    username: string;
    photoUrl: string;
    age: number;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photos: Photo[];

    dawgTag: number;
    personalURL: string;
    requestedURL: string;
    oldPersonalURL: string;
    pageQuota: number;
    totalPagesPrinted: number;
    printJobs: PrintJob[];

    publicKeySSH1: string;
    privateKeySSH1: string;
    publicKeySSH2: string;
    privateKeySSH2: string;
    
    userFiles: UserFile[];

}

