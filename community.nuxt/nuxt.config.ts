// https://nuxt.com/docs/api/configuration/nuxt-config
import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: {enabled: true},
  css: ['~/assets/css/main.css'],
  modules: [
    '@nuxt/eslint',
    '@nuxt/image',
    '@nuxt/scripts',
    '@nuxt/test-utils',
    '@nuxt/ui',
    '@nuxt/content',
    '@nuxt/fonts'
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
  components: [
    {
      path: '~/components/common',
      pathPrefix: false
    },
    '~/components'
  ],
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
  tailwindcss: {
    config: {
      theme: {
        screens: {
          'mobile': '320px',
          'tablet': '640px',
          'laptop': '1024px',
        },
      }
    }
  }
})