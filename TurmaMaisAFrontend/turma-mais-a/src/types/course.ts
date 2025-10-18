export interface CourseState {
    courses: CourseDto[],
    loadingList: boolean,
    loadingItem: boolean,
    error: string | null,
    course: CourseDto | null
}

export interface CourseCreateDto {
    name: string;
}

export interface CourseDto {
    id: string;
    name: string;
}