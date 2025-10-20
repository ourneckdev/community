<script lang="ts" setup>

</script>

<template>
  <header class="">
    <div class="header-container">
      <div class="logo-container">
        <router-link to="/" class="" title="Home Page">
          <logo-light class="block dark:hidden"/>
          <logo-dark class="hidden dark:block w-auto h-6"/>
        </router-link>
      </div>
      <div class="nav-container">
        <nav>
          <div style="position:relative">
            <ul>
              <li>
                <router-link to="/" >Submit a Report</router-link>
              </li>
              <li>
                <router-link to="/" >Active Reports</router-link>
              </li>
              <li>
                <router-link to="/" >Community</router-link>
              </li>
            </ul>
          </div>
        </nav>
      </div>
      <div class="flex items-right justify-end lg:flex-1 gap-1">
        <button class="relative lg:hidden">
          <u-icon name="mdi-hamburger-menu" size="30"/>
        </button>
      </div>
    </div>
  </header>
</template>

<style scoped>
</style>
