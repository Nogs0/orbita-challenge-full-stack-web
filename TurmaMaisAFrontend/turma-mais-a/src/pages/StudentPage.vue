<template>
  <v-sheet border rounded>
    <v-data-table-server class="elevation-3" :headers="headers" :loading="studentStore.isLoadingStudents"
      :hide-default-footer="studentStore.totalCountStudents < 11" :items="studentStore.students"
      :items-length="studentStore.totalCountStudents" @update:options="loadStudents" disable-sort>
      <template v-slot:top>
        <header-table table-name="Seus Alunos" icon="mdi-account-school" search-fields="Nome, RA ou CPF"
          @add="openCreateDialog()" @search="handleSearch">
        </header-table>
      </template>
      <template v-slot:item.actions="{ item }">
        <div class="d-flex ga-2 justify-center">
          <v-icon color="medium-emphasis" icon="mdi-book-cog" size="small" title="Matrículas"
            @click="openSetCoursesDialog(item.id, item.name)"></v-icon>
          <v-icon color="medium-emphasis" icon="mdi-pencil" size="small" title="Editar"
            @click="openEditDialog(item.id)"></v-icon>
          <v-icon color="medium-emphasis" icon="mdi-delete" size="small" title="Excluir"
            @click="openDeleteDialog(item.id, item.name)"></v-icon>
        </div>
      </template>
      <template v-slot:no-data>
        <no-items-table @add="openCreateDialog()" table-name="alunos"></no-items-table>
      </template>
    </v-data-table-server>
  </v-sheet>

  <v-dialog v-model="createOrEditDialog" max-width="500">
    <v-card :title="`${isEditing ? 'Editar' : 'Adicionar'} Aluno`">
      <v-divider></v-divider>
      <v-form @submit.prevent="save" ref="formCreateOrEditStudent" v-model="isFormValid" :loading="loadingItem">
        <v-container>
          <v-text-field v-model="formModel.name" label="Nome *"
            :rules="[rules.required, rules.maxLength(128)]"></v-text-field>

          <v-text-field v-model="formModel.email" label="Email *" maxlength="128"
            :rules="[rules.required, rules.maxLength(128), rules.email]"></v-text-field>

          <v-text-field v-if="isEditing" v-model="formModel.ra" label="RA *"
            :rules="[rules.required, rules.maxLength(14)]" :disabled="isEditing"></v-text-field>

          <v-text-field v-model="formModel.cpf" label="CPF *" :rules="[rules.required, rules.maxLength(14)]"
            :disabled="isEditing"></v-text-field>
        </v-container>
        <v-card-actions class="bg-surface-light">
          <v-btn text="Cancelar" variant="tonal" @click="closeCreateOrEditDialog()"></v-btn>
          <v-spacer></v-spacer>
          <v-btn type="submit" text="Salvar" color="green" :disabled="!isFormValid || loadingItem"></v-btn>
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>

  <enrollment-modal :show="setCoursesDialog" :student-id="selectedStudentId" :student-name="selectedStudentName"
    @close="closeSetCoursesDialog()" @saved="closeSetCoursesDialog()"></enrollment-modal>

  <v-dialog v-model="deleteDialog" max-width="500">
    <v-card title="Excluir Aluno">
      <v-divider></v-divider>
      <v-container>
        <p>Deseja realmente excluir o aluno '{{ studentName }}'?</p>
      </v-container>
      <v-card-actions class="bg-surface-light">
        <v-btn text="Cancelar" @click="closeDeleteDialog()" variant="tonal"></v-btn>
        <v-spacer></v-spacer>
        <v-btn text="Excluir" color="accent" @click="deleteItem(idToDelete)" variant="tonal"
          :disabled="loadingItem"></v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { useStudentStore } from "@/stores/students";
import type { StudentDto } from "@/types/student";
import type { DataTableHeader } from "vuetify";
import { rules } from "@/utils/rules";
import { formatCpf } from "@/utils/formatters";
import { useSnackbar } from "@/composables/useSnackbar";
import { ca } from "vuetify/locale";
import { isAxiosError } from "axios";

const { showSnackbar } = useSnackbar();
const loadingItem = ref<boolean>(false);
const studentStore = useStudentStore();

const formModel = ref<StudentDto>(createNewRecord());
const createOrEditDialog = shallowRef<boolean>(false);
const isEditing = ref<boolean>(false);
const formCreateOrEditStudent = ref<HTMLFormElement | null>(null);
const isFormValid = ref<boolean>(false);

const deleteDialog = shallowRef<boolean>(false);
const idToDelete = ref<string>("createOrEditDialog");
const studentName = ref<string>("");

const setCoursesDialog = shallowRef<boolean>(false);
const selectedStudentId = ref<string>("");
const selectedStudentName = ref<string>("");

const currentPage = ref<number>(1);
const currentItemsPerPage = ref<number>(1);

const headers = ref<Readonly<DataTableHeader[]>>([
  { title: "Nome", key: "name", sortable: false },
  { title: "RA", key: "ra", sortable: false },
  { title: "CPF", key: "cpf", sortable: false },
  { title: "Ações", key: "actions", sortable: false, align: "center", width: "10%" },
]);

watch(
  () => formModel.value.cpf,
  (newValue: string) => {
    const cpfFormatted = formatCpf(newValue);
    if (cpfFormatted !== newValue) {
      formModel.value.cpf = cpfFormatted;
    }
  }
);

function createNewRecord(): StudentDto {
  return {
    id: "",
    ra: "",
    name: "",
    email: "",
    cpf: "",
  };
}

function loadStudents(options: { page: number; itemsPerPage: number }) {
  studentStore.fetchStudents(options.page, options.itemsPerPage);
  currentPage.value = options.page;
  currentItemsPerPage.value = options.itemsPerPage;
}

function handleSearch(searchTerm: string) {
  studentStore.fetchStudents(currentPage.value, currentItemsPerPage.value, searchTerm);
}

function openCreateDialog() {
  formModel.value = createNewRecord();
  createOrEditDialog.value = true;
}

async function openEditDialog(id: string) {
  isEditing.value = true;
  await studentStore.fetchStudentById(id);
  if (studentStore.student) {
    formModel.value = {
      id: studentStore.student.id,
      name: studentStore.student.name,
      email: studentStore.student.email,
      ra: studentStore.student.ra,
      cpf: studentStore.student.cpf,
    };
  }
  createOrEditDialog.value = true;
}

async function save() {
  if (loadingItem.value)
    return;

  loadingItem.value = true;
  const { valid } = await formCreateOrEditStudent.value?.validate();

  if (!valid) {
    loadingItem.value = false;
    return;
  }
  
  if (isEditing.value) {
    try {
      await studentStore.updateStudent({
        id: formModel.value.id,
        name: formModel.value.name,
        email: formModel.value.email,
      });
      setTimeout(() => {
        isEditing.value = false;
      }, 200);
      showSnackbar("Aluno atualizado com sucesso.", "success");
      createOrEditDialog.value = false;
    } catch (error) {
      if (isAxiosError(error) && error.response?.data?.message) {
        showSnackbar(error.response.data.message, "error");
      }
      console.error(error);
    } finally {
      loadingItem.value = false;
    }
  } else {
    try {
      await studentStore.createStudent({
        name: formModel.value.name,
        email: formModel.value.email,
        cpf: formModel.value.cpf,
      });
      setTimeout(() => {
        isEditing.value = false;
      }, 200);
      showSnackbar("Aluno cadastrado com sucesso.", "success");
      createOrEditDialog.value = false;
    } catch (error) {
      if (isAxiosError(error) && error.response?.data.message) {
        showSnackbar(error.response.data.message, "error");
      }
      console.error(error);
    } finally {
      loadingItem.value = false;
    }
  }
}

function closeCreateOrEditDialog() {
  createOrEditDialog.value = false;
  setTimeout(() => {
    isEditing.value = false;
  }, 150);
}

function openSetCoursesDialog(studentId: string, name: string) {
  selectedStudentId.value = studentId;
  selectedStudentName.value = name;
  setCoursesDialog.value = true;
}

function closeSetCoursesDialog() {
  setCoursesDialog.value = false;
  setTimeout(() => {
    selectedStudentId.value = "";
    selectedStudentName.value = "";
  }, 200);
}

function openDeleteDialog(id: string, name: string) {
  deleteDialog.value = true;
  idToDelete.value = id;
  studentName.value = name;
}

async function deleteItem(id: string) {
  loadingItem.value = true;
  try {
    await studentStore.deleteStudent(id);
    closeDeleteDialog();
  } catch (error) {
    if (isAxiosError(error) && error.response?.data.message) {
      showSnackbar(error.response.data.message, "error");
    }
    console.error(error);
  } finally {
    loadingItem.value = false;
  }
}

function closeDeleteDialog() {
  deleteDialog.value = false;
  idToDelete.value = "";
  setTimeout(() => {
    studentName.value = "";
  }, 150);
}
</script>
