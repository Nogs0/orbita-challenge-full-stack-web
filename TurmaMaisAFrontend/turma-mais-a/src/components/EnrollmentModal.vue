<template>
    <v-dialog :model-value="show" persistent max-width="800px">
        <v-card :title="`Matricular Aluno ${studentName}`">
            <v-divider></v-divider>
            <v-container>
                <v-row align="center" class="mb-4">
                    <v-col cols="10">
                        <v-autocomplete v-model="selectedCourse" :items="availableCourses" item-title="name"
                            item-value="id" label="Pesquisar matéria para adicionar" variant="outlined"
                            density="compact" return-object hide-details :loading="isLoadingCourses">
                        </v-autocomplete>
                    </v-col>
                    <v-col cols="2">
                        <v-btn color="primary" block @click="addCourseToSelection" :disabled="!selectedCourse">
                            Adicionar
                        </v-btn>
                    </v-col>
                </v-row>

                <v-divider></v-divider>
                <v-data-table :headers="tableHeaders" :items="selectedCourses" density="compact"
                    no-data-text="Nenhuma matéria adicionada" hide-default-footer>
                    <template v-slot:item.actions="{ item }">
                        <v-icon color="error" icon="mdi-delete" size="small"
                            @click="removeCourseFromSelection(item.id)"></v-icon>
                    </template>
                </v-data-table>
            </v-container>

            <v-divider></v-divider>

            <v-card-actions class="bg-surface-light">
                <v-btn variant="tonal" @click="emit('close')">Cancelar</v-btn>
                <v-spacer></v-spacer>
                <v-btn variant="tonal" color="green" @click="saveEnrollments"
                    :loading="enrollmentStore.isLoadingEnrollments">
                    Salvar
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
// Supondo que você crie essas stores e tipos
import { useSnackbar } from '@/composables/useSnackbar';
import { useCourseStore } from '@/stores/courses';
import { useEnrollmentsStore } from '@/stores/enrollments';
import type { CourseDto } from '@/types/course';
import { isAxiosError } from 'axios';
import type { DataTableHeader } from 'vuetify';

interface EnrollmentModalProps {
    show: boolean;
    studentId: string;
    studentName: string;
}
const props = defineProps<EnrollmentModalProps>();

interface Emits {
    (e: 'close'): void;
    (e: 'saved'): void;
}
const emit = defineEmits<Emits>();

// --- Stores e Composables ---
const courseStore = useCourseStore();
const enrollmentStore = useEnrollmentsStore();
const { showSnackbar } = useSnackbar();

// --- Estado Local do Componente ---
const isLoadingCourses = ref(false);
const selectedCourse = ref<CourseDto | null>(null);
const availableCourses = ref<CourseDto[]>([]);
const selectedCourses = ref<CourseDto[]>([]);
const isLoading = ref(false);

const tableHeaders: Readonly<DataTableHeader[]> = [
    { title: 'Matéria', key: 'name', sortable: false },
    { title: 'Ações', key: 'actions', sortable: false, align: 'center', width: '100px' },
];

async function fetchAvailableCourses() {
    isLoadingCourses.value = true;
    availableCourses.value = await courseStore.fetchAllCourses();
    selectedCourses.value = await enrollmentStore.fetchCoursesByStudentId(props.studentId);
    isLoadingCourses.value = false;
}

function addCourseToSelection() {
    if (selectedCourse.value && !selectedCourses.value.find(c => c.id === selectedCourse.value!.id)) {
        selectedCourses.value.push(selectedCourse.value);
    }
    selectedCourse.value = null;
}

function removeCourseFromSelection(courseId: string) {
    selectedCourses.value = selectedCourses.value.filter(c => c.id !== courseId);
}

async function saveEnrollments() {
    if (!props.studentId) return;
    isLoading.value = true;
    const coursesIds = selectedCourses.value.map(c => c.id);
    try {
        await enrollmentStore.saveEnrollments(props.studentId, coursesIds);
        showSnackbar('Matrículas salvas com sucesso!', 'success');
        emit('saved');
        emit('close');
    }
    catch (error) {
        if (isAxiosError(error) && error.response?.data?.message) {
            showSnackbar(error.response.data.message, 'error');
        }
        console.error(error);
    }
    finally {
        isLoading.value = false;
    }
}

watch(() => props.show, (isVisible) => {
    if (isVisible) {
        fetchAvailableCourses();

        selectedCourses.value = [];
        selectedCourse.value = null;
    }
});
</script>