<template>
    <v-toolbar flat>
        <v-toolbar-title>
            <v-icon color="medium-emphasis" icon="mdi-book-multiple" size="x-small" start></v-icon>
            {{ tableName }}
        </v-toolbar-title>

        <v-btn class="me-2" prepend-icon="mdi-plus" rounded="lg" text="Adicionar" border @click="emit('add')"></v-btn>
    </v-toolbar>
</template>

<script setup lang="ts">
interface HeaderTableDto {
    tableName: string;
}

defineProps<HeaderTableDto>();

interface Emits {
  (e: 'add'): void; 
}

const emit = defineEmits<Emits>();
</script>