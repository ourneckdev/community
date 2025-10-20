// https://nuxt.com/docs/api/configuration/nuxt-config
import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: {enabled: true},
  css: ['~/assets/css/main.css'],
  modules: [
    '@nuxt/fonts',
    '@nuxt/ui'
  ],
  app: {
    head: {
      title: 'Our Neck of the Woods | Notify your community of potential hazards',
      htmlAttrs: {
        lang: 'en',
      },
      charset: 'utf-16',
      viewport: 'width=device-width, initial-scale=1, maximum-scale=1',
    }
  },
  fonts: {
    adobe: {
      id: ['ijt1zhx']
    }
  },
  vite: {
    plugins: [
      tailwindcss(),
    ],
  },
})
