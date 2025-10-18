<template>
  <v-sheet border rounded>
    <!-- <v-data-table :items="studentStore.students"></v-data-table> -->
    <v-data-table-server 
      :headers="headers" 
      :loading="studentStore.isLoadingStudents"
      :hide-default-footer="(studentStore.totalCountStudents < 11)" 
      :items="studentStore.students"
      :items-length="studentStore.totalCountStudents" 
      @update:options="loadStudents" 
      disable-sort>
      <template v-slot:top>
        <header-table table-name="Seus Alunos" @add="openCreateDialog()">
        </header-table>
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
        <no-items-table @add="openCreateDialog()" table-name="alunos"></no-items-table>
      </template>
    </v-data-table-server>
  </v-sheet>

  <v-dialog v-model="dialog" max-width="500" persistent>
    <v-card :title="`${isEditing ? 'Editar' : 'Adicionar'} Aluno`">
      <v-divider></v-divider>
      <v-form ref="formCreateOrEditStudent" v-model="isFormValid">
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
          <v-btn text="Cancelar" variant="plain" @click="closeCreateOrEditDialog()"></v-btn>

          <v-spacer></v-spacer>

          <v-btn text="Salvar" @click="save" color="green" :disabled="!isFormValid"></v-btn>
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>

  <v-dialog v-model="dialogDelete" max-width="500" persistent>
    <v-card :title="`Excluir Aluno`">
      <v-divider></v-divider>
      <v-container>
        <p>Deseja realmente excluir o aluno '{{ studentName }}'?</p>
      </v-container>
      <v-card-actions class="bg-surface-light">
        <v-btn text="Cancelar" variant="plain" @click="closeDeleteDialog()"></v-btn>
        <v-spacer></v-spacer>
        <v-btn text="Excluir" color="red" @click="deleteItem(idToDelete)" variant="tonal"></v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useStudentStore } from '@/stores/students';
import type { StudentDto } from '@/types/student';
import type { DataTableHeader } from 'vuetify';
import { rules } from "@/utils/rules";

const studentStore = useStudentStore();

const formModel = ref<StudentDto>(createNewRecord())
const dialog = shallowRef<boolean>(false);
const isEditing = ref<boolean>(false)
const formCreateOrEditStudent = ref<HTMLFormElement | null>(null);
const isFormValid = ref<boolean>(false);

const dialogDelete = shallowRef<boolean>(false);
const idToDelete = ref<string>('');
const studentName = ref<string>('');
const itemsPerPage = ref(10);
const headers = ref<Readonly<DataTableHeader[]>>([
  { title: 'Nome', key: 'name', sortable: false },
  { title: 'RA', key: 'ra', sortable: false },
  { title: 'CPF', key: 'cpf', sortable: false },
  { title: 'Ações', key: 'actions', sortable: false, align: "end" }
]);

function createNewRecord(): StudentDto {
  return {
    id: '',
    ra: '',
    name: '',
    email: '',
    cpf: ''
  }
}

function loadStudents(options: { page: number, itemsPerPage: number }) {
  studentStore.fetchStudents(options.page, options.itemsPerPage);
}

function openCreateDialog() {
  formModel.value = createNewRecord();
  dialog.value = true;
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
      cpf: studentStore.student.cpf
    }
  }
  dialog.value = true
}

async function save() {
  const { valid } = await formCreateOrEditStudent.value?.validate();

  if (!valid)
    return;

  if (isEditing.value) {
    await studentStore.updateStudent({
      id: formModel.value.id,
      name: formModel.value.name,
      email: formModel.value.email
    });
    setTimeout(() => {
      isEditing.value = false;
    }, 200);
  } else {
    await studentStore.createStudent({
      name: formModel.value.name,
      email: formModel.value.email,
      cpf: formModel.value.cpf
    });
  }

  dialog.value = false
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
  studentName.value = name;
}

async function deleteItem(id: string) {
  await studentStore.deleteStudent(id);
  closeDeleteDialog();
}

function closeDeleteDialog() {
  dialogDelete.value = false;
  idToDelete.value = '';
  studentName.value = '';
}

</script>