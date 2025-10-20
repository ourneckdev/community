<script setup lang="ts">
  import LogoLight from "./LogoLight.vue";
  import LogoDark from "./LogoDark.vue";
</script>

<template>
  <div id="logo">
    <LogoLight class="dark:hidden"/>
    <LogoDark class="dark:visible"/>
  </div>
</template>

<style scoped>
#logo {
  svg {
    height: 10vh;
  }
}
</style>
