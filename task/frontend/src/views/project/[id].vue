<template>
  <div>
    <div class="row clearfix">
      <div class="col-12">
        <h5>
          {{ project.name }}
          <Avatar icon="pi pi-user-edit" style="cursor: pointer;" shape="circle" @click="toggle" />
          <TieredMenu ref="menu" id="overlay_tmenu" :model="items" popup />
        </h5>
      </div>
    </div>
    <TabView>
      <TabPanel header="Tổng quan">
        <div style="height: 75vh;">

        </div>
      </TabPanel>
      <TabPanel header="List">
        <ProjectTreeGrid></ProjectTreeGrid>
      </TabPanel>
      <TabPanel header="Board">
        <ProjectKanban></ProjectKanban>
      </TabPanel>
      <TabPanel header="Timeline">
        <ProjectGantt></ProjectGantt>
      </TabPanel>
    </TabView>
  </div>
</template>

<script setup>
import { onMounted, ref, watch, computed, reactive } from "vue";
// import the component
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import TieredMenu from 'primevue/tieredmenu';
import Avatar from 'primevue/avatar';

import ProjectTreeGrid from '../../components/TreeGrid/ProjectTreeGrid.vue';
import ProjectKanban from '../../components/Kanban/ProjectKanban.vue';
import ProjectGantt from '../../components/Gantt/ProjectGantt.vue';
import Api from "../../api/Api";
import { useRoute } from "vue-router";
import { projectData } from "../../datasource";
const menu = ref();
const route = useRoute();
const project = ref({});
const items = ref([

  {
    label: 'Edit',
    icon: 'pi pi-fw pi-pencil',
  },
  {
    separator: true
  },
  {
    label: 'Delete',
    icon: 'pi pi-fw pi-trash'
  }
]);
const toggle = (event) => {
  menu.value.toggle(event);
};
onMounted(() => {
  Api.GetProject(route.params.id).then((res) => {
    project.value = res;
  });
  // console.log(1);
});
</script>

<style>
@import '../../../node_modules/@syncfusion/ej2-base/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-buttons/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-calendars/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-dropdowns/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-inputs/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-navigations/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-popups/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-splitbuttons/styles/material.css';
@import "../../../node_modules/@syncfusion/ej2-grids/styles/material.css";
@import "../../../node_modules/@syncfusion/ej2-treegrid/styles/material.css";
</style>

<style>
@import '../../../node_modules/@syncfusion/ej2-layouts/styles/material.css';
@import '../../../node_modules/@syncfusion/ej2-vue-kanban/styles/material.css';
</style>

<style>  ;
  @import '../../../node_modules/@syncfusion/ej2-lists/styles/material.css';
  @import '../../../node_modules/@syncfusion/ej2-grids/styles/material.css';
  @import '../../../node_modules/@syncfusion/ej2-richtexteditor/styles/material.css';
  @import '../../../node_modules/@syncfusion/ej2-treegrid/styles/material.css';
  @import "../../../node_modules/@syncfusion/ej2-vue-gantt/styles/material.css";

  .e-gantt-splitter {
    width: 100% !important;
  }
</style>