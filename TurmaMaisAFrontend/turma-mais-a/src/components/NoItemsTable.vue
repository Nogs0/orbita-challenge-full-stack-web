<template>
    <v-empty-state image="https://cdn.vuetifyjs.com/docs/images/components/v-empty-state/teamwork.png">
        <template v-slot:title>
            <div class="text-subtitle-2 mt-8">
                Gerencie seus {{tableName}}
            </div>
        </template>

        <template v-slot:text>
            <div class="text-caption">
                Registre e edite seus {{tableName}}
            </div>
        </template>

        <template v-slot:actions>
            <v-btn class="text-none" elevation="1" rounded="lg" size="small" text="Adicionar" @click="emit('add')"></v-btn>
        </template>
    </v-empty-state>
</template>

<script setup lang="ts">
interface NoItemsTabelProps {
    tableName: string
}
defineProps<NoItemsTabelProps>();
interface Emits {
    (e: 'add'): void;
}
const emit = defineEmits<Emits>();
</script>