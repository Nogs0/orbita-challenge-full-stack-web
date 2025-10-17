<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6" class="text-center">
        <v-icon size="128" color="primary" class="mb-4">
          mdi-alert-circle-outline
        </v-icon>
        <h1 class="display-3 font-weight-bold mb-4">404 - Página Não Encontrada</h1>
        <v-btn color="primary" variant="flat" to="/" size="large">
          <v-icon start>mdi-home</v-icon>
          Voltar para a Página Inicial
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
</script>

<style scoped>
</style>