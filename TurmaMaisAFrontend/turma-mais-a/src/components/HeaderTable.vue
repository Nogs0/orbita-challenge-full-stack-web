<template>
    <v-toolbar flat>
        <v-toolbar-title class="font-weight-bold">
            <v-icon color="medium-emphasis" :icon="icon" size="x-small" start></v-icon>
            {{ tableName }}
        </v-toolbar-title>

        <v-btn class="me-2" color="primary" prepend-icon="mdi-plus" text="Adicionar" @click="emit('add')"></v-btn>
    </v-toolbar>
</template>

<script setup lang="ts">
interface HeaderTableProps {
    tableName: string;
    icon: string;
}

defineProps<HeaderTableProps>();

interface Emits {
  (e: 'add'): void; 
}

const emit = defineEmits<Emits>();
</script>