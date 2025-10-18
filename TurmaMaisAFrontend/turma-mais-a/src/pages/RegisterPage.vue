<template>
  <v-container class="fill-height">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="elevation-12 pa-4">
          <v-card-title class="text-center">Cadastre-se</v-card-title>
          <v-card-text>
            <v-alert v-if="showAlert" type="error" variant="tonal" closable class="m-4"
              @update:model-value="showAlert = false">
              {{ errorMessage }}
            </v-alert>
            <v-form @submit.prevent="handleRegister" v-model="isFormValid" ref="formRegisterUser">
              <v-text-field v-model="formModel.fullName" label="Nome completo *" type="text"
                prepend-inner-icon="mdi-account-outline"
                :rules="[rules.required, rules.minLength(3), rules.maxLength(128)]"></v-text-field>
              <v-text-field v-model="formModel.organizationName" label="Organização *" type="text"
                prepend-inner-icon="mdi-account-school-outline"
                :rules="[rules.required, rules.minLength(3)]"></v-text-field>
              <v-text-field v-model="formModel.email" label="Email *" type="email"
                prepend-inner-icon="mdi-email-outline"
                :rules="[rules.required, rules.minLength(5), rules.maxLength(128), rules.email]"></v-text-field>
              <v-text-field v-model="formModel.password" label="Senha *" type="password"
                prepend-inner-icon="mdi-lock-outline"
                :rules="[rules.required, rules.minLength(8), rules.maxLength(64), rules.passwordStrength]"></v-text-field>
              <v-text-field v-model="formModel.confirmPassword" label="Confirme a senha *" type="password"
                prepend-inner-icon="mdi-lock-outline"
                :rules="[rules.required, rules.minLength(8), rules.maxLength(64), rules.passwordMatch(formModel.password)]"></v-text-field>
              <v-btn type="submit" color="green" block class="mt-2" :loading="authStore.isLoading"
                :disabled="!isFormValid">
                Cadastrar
              </v-btn>
            </v-form>
            <v-btn color="success" block to="/login" class="mt-5">
              Fazer login
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { useAuthStore } from "@/stores/auth";
import { rules } from "@/utils/rules";
import type { RegisterDto } from "@/types/auth";

function createNewRecord(): RegisterDto {
  return {
    fullName: '',
    email: '',
    organizationName: '',
    password: '',
    confirmPassword: ''
  };
}

const formModel = ref<RegisterDto>(createNewRecord())
const showAlert = ref<boolean>(false);
const errorMessage = ref<string>("");
const authStore = useAuthStore();
const formRegisterUser = ref<HTMLFormElement | null>(null);
const isFormValid = ref<boolean>(false)

async function handleRegister() {
  const { valid } = await formRegisterUser.value?.validate();

  if (!valid)
    return;

  const result = await authStore.register(formModel.value);

  if (!result.success) {
    showAlert.value = true;
    errorMessage.value = result.message || `Erro desconhecido.`;
  }
}
</script>
