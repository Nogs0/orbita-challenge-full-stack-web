<template>
  <v-container class="fill-height">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-12 pa-4">
          <v-card-title class="text-center">Login</v-card-title>
          <v-card-text>
            <v-alert
              v-if="showAlert"
              type="error"
              variant="tonal"
              closable
              class="mb-4"
              @update:model-value="showAlert = false"
            >
              {{ errorMessage }}
            </v-alert>
            <v-form @submit.prevent="handleLogin">
              <v-text-field
                v-model="username"
                label="Email"
                type="email"
                variant="outlined"
                prepend-inner-icon="mdi-email-outline"
              ></v-text-field>
              <v-text-field
                v-model="password"
                label="Senha"
                type="password"
                variant="outlined"
                prepend-inner-icon="mdi-lock-outline"
              ></v-text-field>
              <v-btn
                type="submit"
                color="primary"
                block
                class="mt-2"
                :loading="isLoading"
              >
                Entrar
              </v-btn>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "@/stores/auth";

const username = ref<string>("");
const password = ref<string>("");
const isLoading = ref<boolean>(false);
const showAlert = ref<boolean>(false);
const errorMessage = ref<string>("");
const authStore = useAuthStore();

async function handleLogin() {
  if (!username.value || !password.value) {
    showAlert.value = true;
    errorMessage.value = `Por favor, preencha todos os campos.`;
    return;
  }

  isLoading.value = true;
  const result = await authStore.login({
    username: username.value,
    password: password.value,
  });
  if (!result.success) {
    showAlert.value = true;
    errorMessage.value = result.message || `Erro desconhecido.`;
  }
  isLoading.value = false;
}
</script>
