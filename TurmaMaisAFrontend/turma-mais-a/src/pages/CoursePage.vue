<template>
  <v-sheet border rounded>
    <v-data-table-server class="elevation-4" :headers="headers" :loading="courseStore.isLoadingCourses"
      :hide-default-footer="courseStore.totalCountCourses < 11" :items="courseStore.courses"
      :items-length="courseStore.totalCountCourses" @update:options="loadCourses" disable-sort>
      <template v-slot:top>
        <header-table table-name="Seus Cursos" icon="mdi-book-variant" @add="openCreateDialog()"> </header-table>
      </template>
      <template v-slot:item.actions="{ item }">
        <div class="d-flex ga-2 justify-end">
          <v-icon color="medium-emphasis" icon="mdi-pencil" size="small" title="Editar"
            @click="openEditDialog(item.id)"></v-icon>
          <v-icon color="medium-emphasis" icon="mdi-delete" size="small" title="Excluir"
            @click="openDeleteDialog(item.id, item.name)"></v-icon>
        </div>
      </template>
      <template v-slot:no-data>
        <no-items-table @add="openCreateDialog()" table-name="cursos"></no-items-table>
      </template>
    </v-data-table-server>
  </v-sheet>

  <v-dialog v-model="dialog" max-width="500" persistent>
    <v-card :title="`${isEditing ? 'Editar' : 'Adicionar'} Curso`">
      <v-divider></v-divider>
      <v-form ref="formCreateOrEditCourse" v-model="isFormValid" :loading="loadingItem">
        <v-container>
          <v-text-field v-model="formModel.name" label="Nome *"
            :rules="[rules.required, rules.maxLength(128)]"></v-text-field>
        </v-container>
        <v-card-actions class="bg-surface-light">
          <v-btn text="Cancelar" variant="plain" @click="closeCreateOrEditDialog()"></v-btn>

          <v-spacer></v-spacer>

          <v-btn text="Salvar" @click="save" color="green" :disabled="!isFormValid" variant="tonal"></v-btn>
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>

  <v-dialog v-model="dialogDelete" max-width="500" persistent>
    <v-card :title="`Excluir Curso`">
      <v-divider></v-divider>
      <v-container>
        <p>Deseja realmente excluir o curso '{{ courseName }}'?</p>
      </v-container>
      <v-card-actions class="bg-surface-light">
        <v-btn text="Cancelar" variant="tonal" @click="closeDeleteDialog()"></v-btn>
        <v-spacer></v-spacer>
        <v-btn text="Excluir" color="accent" @click="deleteItem(idToDelete)" variant="tonal"></v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { useCourseStore } from "@/stores/courses";
import type { CourseDto } from "@/types/course";
import type { DataTableHeader } from "vuetify";
import { rules } from "@/utils/rules";
import { isAxiosError } from "axios";
import { useSnackbar } from "@/composables/useSnackbar";

const courseStore = useCourseStore();
const { showSnackbar } = useSnackbar();

const formModel = ref<CourseDto>(createNewRecord());
const dialog = shallowRef<boolean>(false);
const isEditing = ref<boolean>(false);
const formCreateOrEditCourse = ref<HTMLFormElement | null>(null);
const isFormValid = ref<boolean>(false);
const loadingItem = ref<boolean>(false);
const dialogDelete = shallowRef<boolean>(false);
const idToDelete = ref<string>("");
const courseName = ref<string>("");
const headers = ref<Readonly<DataTableHeader[]>>([
  { title: "Nome", key: "name", sortable: false },
  { title: "Ações", key: "actions", sortable: false, align: "end" },
]);

function createNewRecord(): CourseDto {
  return {
    id: "",
    name: "",
  };
}

function loadCourses(options: { page: number; itemsPerPage: number }) {
  courseStore.fetchCourses(options.page, options.itemsPerPage);
}

function openCreateDialog() {
  formModel.value = createNewRecord();
  dialog.value = true;
}

async function openEditDialog(id: string) {
  isEditing.value = true;
  await courseStore.fetchCourseById(id);
  if (courseStore.course) {
    formModel.value = {
      id: courseStore.course.id,
      name: courseStore.course.name,
    };
    dialog.value = true;
  }
}

async function save() {
  const { valid } = await formCreateOrEditCourse.value?.validate();
  if (!valid) return;

  loadingItem.value = true;

  if (isEditing.value) {
    try {
      await courseStore.updateCourse({
        id: formModel.value.id,
        name: formModel.value.name,
      });
      dialog.value = false;
      setTimeout(() => {
        isEditing.value = false;
      }, 200);

      showSnackbar('Curso atualizado com sucesso.', 'success');
    } catch (error) {
      if (isAxiosError(error) && error.response?.data?.errorMessage) {
        showSnackbar(error.response.data.errorMessage, 'error');
      }
      console.error(error);
    } finally {
      loadingItem.value = false;
    }
  } else {
    try {
      await courseStore.createCourse({
        name: formModel.value.name,
      });
      dialog.value = false;
      showSnackbar('Curso criado com sucesso.', 'success');

    } catch (error) {
      if (isAxiosError(error) && error.response?.data?.errorMessage) {
        showSnackbar(error.response.data.errorMessage, 'error');
      }
      console.error(error);
    } finally {
      loadingItem.value = false;
    }
  }
}

function closeCreateOrEditDialog() {
  dialog.value = false;
  setTimeout(() => {
    isEditing.value = false;
  }, 150);
}

function openDeleteDialog(id: string, name: string) {
  dialogDelete.value = true;
  idToDelete.value = id;
  courseName.value = name;
}

async function deleteItem(id: string) {
  loadingItem.value = true;
  try {
    await courseStore.deleteStudent(id);
    closeDeleteDialog();
  }
  catch (error) {
    if (isAxiosError(error) && error.response?.data?.errorMessage) {
      showSnackbar(error.response.data.errorMessage, 'error');
    }
    console.error(error);
  }
  finally {
    loadingItem.value = false;
  }
}

function closeDeleteDialog() {
  dialogDelete.value = false;
  idToDelete.value = "";
  setTimeout(() => {
    courseName.value = "";
  }, 150);
}
</script>
