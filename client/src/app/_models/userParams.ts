import { User } from './user';

export class UserParams {
    gender: string;
    printStatus: string;
    searchUser: string;
    searchPrinter: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 12;
    printJobPageNumber = 1;
    printJobPageSize = 10;
    printerPageNumber = 1;
    printerPageSize = 5;
    accessPageNumber = 1;
    accessPageSize = 10;
    semesterPageNumber = 1;
    semesterPageSize = 10;
    classlistPageNumber = 1;
    classlistPageSize = 10;
    urlRequestsPageSize = 5;
    orderBy = 'lastActive';
    searchYear: string;
    searchTerm: string;
    semesterId: number;
    

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
        this.printStatus = 'Held';
        this.searchUser = "";
        this.searchPrinter = "";
        this.searchYear = "All";
        this.searchTerm = "All";
    }
}

