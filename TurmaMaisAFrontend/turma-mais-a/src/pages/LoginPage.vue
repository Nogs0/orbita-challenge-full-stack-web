<template>
  <v-container class="fill-height">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="elevation-12 pa-4">
          <v-card-title class="text-center">Login</v-card-title>
          <v-card-text>
            <v-alert v-if="showAlert" type="error" variant="tonal" closable class="mb-4"
              @update:model-value="showAlert = false">
              {{ errorMessage }}
            </v-alert>
            <v-form @submit.prevent="handleLogin" v-model="isFormValid" ref="formLogin" :loading="authStore.isLoading">
              <v-text-field v-model="username" label="Email *" type="email" prepend-inner-icon="mdi-email-outline"
                :rules="[rules.required, rules.maxLength(128), rules.email]"></v-text-field>
              <v-text-field v-model="password" label="Senha *" type="password" prepend-inner-icon="mdi-lock-outline"
                :rules="[rules.required, rules.maxLength(64)]"></v-text-field>
              <v-btn type="submit" color="green" block class="mt-2" :loading="authStore.isLoading"
                :disabled="!isFormValid">
                Entrar
              </v-btn>
            </v-form>
            <v-btn color="primary" block  to="/register" class="mt-5">
              Cadastrar-se
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "@/stores/auth";
import { rules } from "@/utils/rules";

const username = ref<string>("");
const password = ref<string>("");
const showAlert = ref<boolean>(false);
const errorMessage = ref<string>("");
const authStore = useAuthStore();
const formLogin = ref<HTMLFormElement | null>(null);
const isFormValid = ref<boolean>(false);
async function handleLogin() {
  const { valid } = await formLogin.value?.validate();

  if (!valid)
    return;

  const result = await authStore.login({
    username: username.value,
    password: password.value,
  });
  if (!result.success) {
    showAlert.value = true;
    errorMessage.value = result.message || `Erro desconhecido.`;
  }
}
</script>
