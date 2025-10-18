<template>
  <v-app>
    <v-navigation-drawer>
      <v-list>
        <v-list-item>Módulo Acadêmico</v-list-item>
        <v-list-item to="/students" prepend-icon="mdi-account-school" title="Alunos"></v-list-item>
        <v-list-item to="/courses" prepend-icon="mdi-book-variant" title="Cursos"></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar>
      <v-toolbar-title>Gestão {{ authStore.organizationName }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn icon @click="openLogoutDialog()">
        <v-icon>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>

    <v-main>
      <v-container>
        <router-view />
      </v-container>
    </v-main>
  </v-app>

  <v-dialog v-model="logouDialog" max-width="500">
    <v-card title="Sair">
      <v-divider></v-divider>
      <v-container>
        <p>Deseja realmente sair?</p>
      </v-container>
      <v-card-actions class="bg-surface-light">
        <v-btn text="Cancelar" variant="plain" @click="closeLogoutDialog()"></v-btn>
        <v-spacer></v-spacer>
        <v-btn text="Sair" @click="authStore.logout()"></v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
const authStore = useAuthStore();
const logouDialog = ref<boolean>(false);
function openLogoutDialog() {
  logouDialog.value = true;
}
function closeLogoutDialog() {
  logouDialog.value = false;
}
</script>