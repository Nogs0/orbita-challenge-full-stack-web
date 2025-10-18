<template>
  <v-snackbar
    v-model="isVisible"
    :color="color"
    timeout="2000"
    location="bottom right"
    variant="elevated"
  >
    {{ text }}
    <template v-slot:actions>
      <v-btn icon @click="isVisible = false">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script setup lang="ts">
import { useSnackbar } from '@/composables/useSnackbar';

// Pega o estado global do nosso composable
const { isVisible, text, color } = useSnackbar();
</script>