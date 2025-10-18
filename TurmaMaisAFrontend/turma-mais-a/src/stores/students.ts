import { useSnackbar } from '@/composables/useSnackbar';
import type { PagedResultDto } from '@/types/shared';
import type { StudentCreateDto, StudentDto, StudentListDto, StudentState, StudentUpdateDto } from '@/types/student';
import axios from 'axios';
import { defineStore } from 'pinia';
const { showSnackbar } = useSnackbar();

function getInitialState(): StudentState {
    const students: StudentListDto[] = [];
    const totalCount = 0;
    const student = null;
    const loadingList = false;
    const loadingItem = false;
    const error = null;
    const pagedInput = null;
    return { students, totalCount, student, loadingList, loadingItem, error, pagedInput };
}

export const useStudentStore = defineStore('Students', {
    state: (): StudentState => getInitialState(),
    getters: {
        isLoadingStudents: (state): boolean => !!state.loadingList,
        isLoadingStudent: (state): boolean => !!state.loadingItem,
        totalCountStudents: (state): number => state.totalCount
    },
    actions: {
        async createStudent(dto: StudentCreateDto): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                const response = await axios.post<StudentDto>('/Students', dto);
                this.student = response.data;
                await this.fetchStudents();
                showSnackbar('Aluno criado com sucesso.', 'success');
            } catch (err) {
                showSnackbar('Não foi possível criar o aluno.', 'error');
                this.error = 'Não foi possível criar o aluno.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async fetchStudents(pageNumber?: number, pageSize?: number, search?: string): Promise<void> {
            this.loadingList = true;
            this.error = null;
            try {
                if (pageNumber || pageSize || search) {
                    this.pagedInput = {
                        pageNumber: pageNumber || 1,
                        pageSize: pageSize || 10,
                        search
                    }
                }

                const response = await axios.get<PagedResultDto<StudentListDto>>('/Students', { params: this.pagedInput });
                this.students = response.data.items;
                this.totalCount = response.data.totalCount;
            } catch (err) {
                showSnackbar('Não foi possível carregar os aluno.', 'error');
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
                showSnackbar('Não foi possível carregar o aluno.', 'error');
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
                showSnackbar('Aluno atualizado com sucesso.', 'success');
            } catch (err) {
                showSnackbar('Não foi possível atualizar o aluno.', 'error');
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
                showSnackbar('Aluno excluído com sucesso.', 'success');
            } catch (err) {
                showSnackbar('Não foi possível excluir o aluno.', 'error');
                this.error = 'Não foi possível excluir o aluno.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        }
    }
})
