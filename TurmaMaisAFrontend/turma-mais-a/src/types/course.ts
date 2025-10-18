import type { PagedInputDto } from "./shared";

export interface CourseState {
    courses: CourseDto[],
    totalCount: number;
    loadingList: boolean,
    loadingItem: boolean,
    error: string | null,
    course: CourseDto | null,
    pagedInput: PagedInputDto | null
}

export interface CourseCreateDto {
    name: string;
}

export interface CourseDto {
    id: string;
    name: string;
}