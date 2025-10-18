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
    themes: {
      light: {
        dark: false,
        colors: {
          primary: '#1b2731',
          accent: '#9D1722',
          error: '#C9031A',
          info: '#2196F3',
          success: '#4CAF50',
          warning: '#FB8C00',
          background: '#fafafa'
        },
      },
    },
  },
  locale: {
    locale: 'pt',
    fallback: 'en',
    messages: { pt },
  },
})
