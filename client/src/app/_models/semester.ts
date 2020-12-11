import { Course } from './course';

export interface Semester {
    id: number;
    year: number;
    term: string;
    courses: Course[];
}