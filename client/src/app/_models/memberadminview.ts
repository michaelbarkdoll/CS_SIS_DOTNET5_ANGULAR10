import { PrintjobsComponent } from '../printjobs/printjobs.component';
import { Photo } from './photo';
import { PrintJob } from './printjob';
import { UserRoles } from './userroles';

export interface MemberAdminView {
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
    userRoles: UserRoles[];

    primaryMajor: string;
    secondaryMajor: string;
    accessPermitted: boolean;
    
    
    

}

