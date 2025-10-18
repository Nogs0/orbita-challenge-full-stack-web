import type { StudentState, StudentListDto, StudentDto, StudentUpdateDto, StudentCreateDto } from '@/types/student'
import axios, { isAxiosError } from 'axios';
import { defineStore } from 'pinia'

function getInitialState(): StudentState {
    const students: StudentListDto[] = [];
    const student = null;
    const loadingList = false;
    const loadingItem = false;
    const error = null;
    return { students, student, loadingList, loadingItem, error };
}

export const useStudentStore = defineStore('Students', {
    state: (): StudentState => getInitialState(),
    getters: {
        isLoadingStudents: (state): boolean => !!state.loadingList,
        isLoadingStudent: (state): boolean => !!state.loadingItem,
    },
    actions: {
        async createStudent(dto: StudentCreateDto): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                const response = await axios.post<StudentDto>('/Students', dto);
                this.student = response.data;
                await this.fetchStudents();
            } catch (err) {
                this.error = 'Não foi possível carregar os alunos.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async fetchStudents(): Promise<void> {
            this.loadingList = true;
            this.error = null;
            try {
                const response = await axios.get<StudentListDto[]>('/Students');
                this.students = response.data;
            } catch (err) {
                this.error = 'Não foi possível carregar os alunos.';
                console.error(err);
            } finally {
                this.loadingList = false;
            }
        },
        async fetchStudentById(id: string): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                const response = await axios.get<StudentDto>('/Students/' + id);
                this.student = response.data;
            } catch (err) {
                this.error = 'Não foi possível carregar o aluno.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async updateStudent(dto: StudentUpdateDto): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                await axios.put<StudentDto>('/Students/' + dto.id, dto);
                await this.fetchStudents();
            } catch (err) {
                this.error = 'Não foi possível atualizar o aluno.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async deleteStudent(id: string): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                await axios.delete<StudentDto>('/Students/' + id);
                await this.fetchStudents();
            } catch (err) {
                this.error = 'Não foi possível excluir o aluno.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        }
    }
})
