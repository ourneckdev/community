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
      <div class="buttons-container">

        <button class="non-mobile" title="Search" aria-label="Search">
          <u-icon name="mdi-search"/>
        </button>
        <button class="non-mobile-light" title="Switch to dark mode" aria-label="Switch to dark mode">
          <u-icon name="mdi-white-balance-sunny"/>
        </button>
        <button class="non-mobile-dark" title="Switch to light mode" aria-label="Switch to light mode">
          <u-icon name="mdi-moon-and-stars"/>
        </button>
        <button class="non-mobile" title="User profile" aria-label="User profile">
          <u-icon name="mdi-user-circle"/>
        </button>
        <button class="mobile-only" title="Show menu" aria-label="Show menu">
          <u-icon name="mdi-hamburger-menu"/>
        </button>
      </div>
    </div>
  </header>
</template>

<style scoped>
</style>
