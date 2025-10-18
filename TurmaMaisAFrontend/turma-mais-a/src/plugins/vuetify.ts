/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { pt } from 'vuetify/locale'
// Composables
import { createVuetify } from 'vuetify'

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  theme: {
    defaultTheme: 'system',
  },
  locale: {
    locale: 'pt', // Define o português como idioma padrão
    fallback: 'en', // Idioma a ser usado caso uma tradução falhe
    messages: { pt }, // Passa o objeto de tradução importado
  },
})
