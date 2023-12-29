<template>
  <div class="left-sidenav">
    <ul class="metismenu left-sidenav-menu">
      <li v-if="list_users.length > 0">
        <router-link class="nav-link" to="/admin">
          <i class="fas fa-eye"></i>
          <span>Quản trị</span>
        </router-link>
      </li>
      <li>
        <router-link class="nav-link" to="/">
          <i class="fas fa-chart-bar"></i>
          <span>Tổng quan</span>
        </router-link>
      </li>
      <li>
        <a href="#">
          <i class="fas fa-book-open"></i>
          <span>Dự án</span>
          <span class="menu-arrow"><i class="mdi mdi-chevron-right"></i></span>
          <CreateProject v-if="is_admin || is_manager"></CreateProject>
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

    <!-- <div class="button-col">

      <span class="btn-text">Thu gọn</span>
    </div> -->
  </div>
</template>
<script setup>
import CreateProject from '../../components/CreateProject.vue'
import { useAuth } from "../../stores/auth";
import { useProject } from "../../stores/project";
import { onMounted, ref } from "vue";
import { useLayout } from "../composables/layout";
import { storeToRefs } from "pinia";
import projectApi from '../../api/projectApi';


const { initMetisMenu, initActiveMenu } = useLayout();
const store = useAuth();
const storeProject = useProject();
const { is_admin, list_users, is_manager } = storeToRefs(store);
const { projects } = storeToRefs(storeProject);
onMounted(() => {
  initMetisMenu();
  initActiveMenu();
  projectApi.GetList().then((data) => {
    projects.value = data;
  });
  // store.fetchUsers();
});
</script>
