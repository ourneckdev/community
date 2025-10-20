<script setup lang="ts">
  import Logo from './Logo/Index.vue'
</script>

<template>
  <div id="header">
    <Logo/>
  </div>
</template>

<style scoped>
#header {
  position: fixed;
  width: 100%;
  border-bottom: 1px solid var(--color-green-highlight);
}
</style>
