import { useSnackbar } from '@/composables/useSnackbar';
import type { CourseDto } from '@/types/course';
import type { EnrollmentDto, EnrollmentState, SetStudentEnrollmentsDto } from '@/types/enrollment';
import type { StudentState } from '@/types/student';
import axios from 'axios';
import { defineStore } from 'pinia';
const { showSnackbar } = useSnackbar();

function getInitialState(): EnrollmentState {
    const enrollments: EnrollmentDto[] = [];
    const error = "";
    const loading = false;
    return { enrollments, error, loading };
}

export const useEnrollmentsStore = defineStore('Enrollments', {
    state: (): EnrollmentState => getInitialState(),
    getters: {
        isLoadingEnrollments: (state) => state.loading
    },
    actions: {
        async saveEnrollments(studentId: string, coursesIds: string[]): Promise<void> {
            this.loading = true;
            try {
                const dto: SetStudentEnrollmentsDto = {
                    studentId,
                    coursesIds
                };

                await axios.put(`/Students/${studentId}/Enrollments`, dto);
                showSnackbar('Matrículas atualizadas com sucesso.', 'success');
            }
            catch (err) {
                showSnackbar('Não foi possível atualizar as matrículas.', 'error');
                this.error = 'Não foi possível atualizar as matrículas.';
                console.error(err);
            } finally {
                this.loading = false;
            }
        },
        async fetchCoursesByStudentId(studentId: string): Promise<CourseDto[]> {
            this.loading = true;
            try {
                const response = await axios.get<CourseDto[]>(`/Students/${studentId}/Enrollments/`);
                return response.data;
            }
            catch (err) {
                showSnackbar('Não foi possível carregar as matrículas.', 'error');
                this.error = 'Não foi possível carregar as matrículas.';
                console.error(err);
                return [];
            } finally {
                this.loading = false;
            }
        }
    }
})
