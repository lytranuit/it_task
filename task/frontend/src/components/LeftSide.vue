<template>
  <div class="left-sidenav">
    <ul class="metismenu left-sidenav-menu">
      <li v-if="is_admin">
        <a href="#">
          <i class="fas fa-tasks"></i>
          <span>Dự án</span>
          <span class="menu-arrow"><i class="mdi mdi-chevron-right"></i></span>
          <CreateProject></CreateProject>
        </a>
        <ul class="nav-second-level" aria-expanded="false">
          <li class="nav-item" v-for="project in projects" :key="project.id">
            <router-link class="nav-link" :to="'/project/' + project.id"><i class="ti-control-record"></i>{{ project.name
            }}</router-link>
          </li>
        </ul>
      </li>

      <li v-if="is_admin">
        <router-link class="nav-link" to="/history">
          <i class="fas fa-history"></i>
          <span>Audittrails</span>
        </router-link>
      </li>

      <li v-if="is_admin">
        <a href="javascript: void(0);">
          <i class="ti-briefcase"></i>
          <span>{{ $t("system") }}</span>
          <span class="menu-arrow"><i class="mdi mdi-chevron-right"></i></span>
        </a>

        <ul class="nav-second-level" aria-expanded="false">
          <li class="nav-item">
            <router-link class="nav-link" to="/user"><i class="ti-control-record"></i>{{ $t("user") }}</router-link>
          </li>
        </ul>
      </li>
    </ul>
  </div>
</template>
<script setup>
import CreateProject from './CreateProject.vue'
import { useAuth } from "../stores/auth";
import { useProject } from "../stores/project";
import { onMounted, ref } from "vue";
import { useLayout } from "../layouts/composables/layout";
import { storeToRefs } from "pinia";
import projectApi from '../api/projectApi';


const { initMetisMenu, initActiveMenu } = useLayout();
const store = useAuth();
const storeProject = useProject();
const { is_admin } = storeToRefs(store);
const { projects } = storeToRefs(storeProject);
const keyCreateProject = ref(0);
const reset = () => {
  keyCreateProject.value++;
  console.log(keyCreateProject.value);
}
onMounted(() => {
  initMetisMenu();
  initActiveMenu();
  projectApi.GetList().then((data) => {
    projects.value = data;
  });
  // store.fetchUsers();
});
</script>
