import { useSnackbar } from '@/composables/useSnackbar';
import type { CourseCreateDto, CourseDto, CourseState } from '@/types/course';
import type { PagedResultDto } from '@/types/shared';
import axios from 'axios';
import { defineStore } from 'pinia';
const { showSnackbar } = useSnackbar();

function getInitialState(): CourseState {
    const courses: CourseDto[] = [];
    const totalCount = 0;
    const course = null;
    const loadingList = false;
    const loadingItem = false;
    const error = null;
    const pagedInput = null;
    return { courses, totalCount, course, loadingList, loadingItem, error, pagedInput };
}

export const useCourseStore = defineStore('Courses', {
    state: (): CourseState => getInitialState(),
    getters: {
        isLoadingCourses: (state): boolean => !!state.loadingList,
        isLoadingCourse: (state): boolean => !!state.loadingItem,
        totalCountCourses: (state): number => state.totalCount
    },
    actions: {
        async createCourse(dto: CourseCreateDto): Promise<void> {
            const response = await axios.post<CourseDto>('/Courses', dto);
            this.course = response.data;
            await this.fetchCourses();
        },
        async fetchCourses(pageNumber?: number, pageSize?: number, search?: string): Promise<void> {
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

                const response = await axios.get<PagedResultDto<CourseDto>>('/Courses', { params: this.pagedInput });
                this.courses = response.data.items;
                this.totalCount = response.data.totalCount;
            } catch (err) {
                showSnackbar('Não foi possível carregar os cursos.', 'error');
                this.error = 'Não foi possível carregar os cursos.';
                console.error(err);
            } finally {
                this.loadingList = false;
            }
        },
        async fetchAllCourses(): Promise<CourseDto[]> {
            this.loadingList = true;
            this.error = null;
            try {
                const response = await axios.get<CourseDto[]>('/Courses/all');
                return response.data;
            } catch (err) {
                showSnackbar('Não foi possível carregar os cursos.', 'error');
                this.error = 'Não foi possível carregar os cursos.';
                console.error(err);
                return [];
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
                showSnackbar('Não foi possível carregar o curso.', 'error');
                this.error = 'Não foi possível carregar o curso.';
                console.error(err);
            } finally {
                this.loadingItem = false;
            }
        },
        async updateCourse(dto: CourseDto): Promise<void> {
            await axios.put<CourseDto>('/Courses/' + dto.id, dto);
            await this.fetchCourses();
        },
        async deleteStudent(id: string): Promise<void> {
            await axios.delete<CourseDto>('/Courses/' + id);
            await this.fetchCourses();
        }
    }
})
