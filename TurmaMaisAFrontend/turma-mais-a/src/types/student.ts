export interface StudentState {
    students: StudentListDto[],
    loadingList: boolean,
    loadingItem: boolean,
    error: string | null,
    student: StudentDto | null
}

export interface StudentDto {
    id: string;
    name: string;
    ra: string;
    cpf: string;
    email: string;
}

export interface StudentListDto {
    id: string;
    name: string;
    ra: string;
    cpf: string;
}

export interface StudentCreateDto {
    name: string;
    email: string;
    cpf: string;
}

export interface StudentUpdateDto {
    id: string;
    name: string;
    email: string;
}