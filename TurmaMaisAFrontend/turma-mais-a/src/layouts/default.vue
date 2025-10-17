<template>
  <v-app>
    <v-navigation-drawer>
      <v-list>
        <v-list-item to="/students" prepend-icon="mdi-package-variant" title="Alunos"></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar>
      <v-toolbar-title>{{authStore.organizationName}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn icon @click="authStore.logout()">
        <v-icon>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>

    <v-main>
      <v-container>
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
const authStore = useAuthStore();
</script>