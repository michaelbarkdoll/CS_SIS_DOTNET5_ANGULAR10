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
    urlRequestsPageSize = 5;
    orderBy = 'lastActive';
    

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
        this.printStatus = 'Held';
        this.searchUser = "";
        this.searchPrinter = "";
    }
}

