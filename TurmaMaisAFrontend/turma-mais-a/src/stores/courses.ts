import type { CourseCreateDto, CourseDto, CourseState } from '@/types/course';
import axios from 'axios';
import { defineStore } from 'pinia'

function getInitialState(): CourseState {
    const courses: CourseDto[] = [];
    const course = null;
    const loadingList = false;
    const loadingItem = false;
    const error = null;
    return { courses, course, loadingList, loadingItem, error };
}

export const useCourseStore = defineStore('Courses', {
    state: (): CourseState => getInitialState(),
    getters: {
        isLoadingCourses: (state): boolean => !!state.loadingList,
        isLoadingCourse: (state): boolean => !!state.loadingItem
    },
    actions: {
        async createCourse(dto: CourseCreateDto): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                const response = await axios.post<CourseDto>('/Courses', dto);
                this.course = response.data;
                await this.fetchCourses();
            } catch (err) {
                this.error = 'Não foi possível carregar os cursos.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async fetchCourses(): Promise<void> {
            this.loadingList = true;
            this.error = null;
            try {
                const response = await axios.get<CourseDto[]>('/Courses');
                this.courses = response.data;
            } catch (err) {
                this.error = 'Não foi possível carregar os cursos.';
                console.error(err);
            } finally {
                this.loadingList = false;
            }
        },
        async fetchCourseById(id: string): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                const response = await axios.get<CourseDto>('/Courses/' + id);
                this.course = response.data;
            } catch (err) {
                this.error = 'Não foi possível carregar o curso.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async updateCourse(dto: CourseDto): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                await axios.put<CourseDto>('/Courses/' + dto.id, dto);
                await this.fetchCourses();
            } catch (err) {
                this.error = 'Não foi possível atualizar o curso.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async deleteStudent(id: string): Promise<void> {
            this.loadingItem = true;
            this.error = null;
            try {
                await axios.delete<CourseDto>('/Courses/' + id);
                await this.fetchCourses();
            } catch (err) {
                this.error = 'Não foi possível excluir o curso.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        }
    }
})
