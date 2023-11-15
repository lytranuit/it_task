<template>
  <div>
    <ConfirmDialog></ConfirmDialog>

    <Loading :waiting="waiting"></Loading>
    <Teleport to="body">
      <Dialog v-model:visible="visible" modal header="Cập nhật" :style="{ width: '50vw' }">
        <Form @beforeSave="hide" :value="project"></Form>
      </Dialog>
    </Teleport>
    <div class="row clearfix">
      <div class="col-12">
        <h5>
          {{ project.name }}
          <Avatar icon="pi pi-user-edit" style="cursor: pointer;" shape="circle" @click="toggle" />
          <TieredMenu ref="menu" id="overlay_tmenu" :model="items" popup />
          <Button label="Thêm công việc" icon="pi pi-plus" class="ml-3 float-right" size="small" severity="success"
            @click="visibleFormTask = true"></Button>
          <Dialog v-model:visible="visibleFormTask" modal :header="headerFormTask" :style="{ width: '50vw' }">
            <FormTask @beforeSave="hide" :projectId="project.id"></FormTask>
          </Dialog>
        </h5>
      </div>
    </div>
    <TabView lazy>
      <TabPanel header="Timeline">
        <ProjectGantt></ProjectGantt>
      </TabPanel>
      <TabPanel header="Board">
        <ProjectKanban></ProjectKanban>
      </TabPanel>
      <TabPanel header="Summary">

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

import ProjectKanban from '../../components/Kanban/ProjectKanban.vue';
import ProjectCalendar from "../../components/Calendar/ProjectCalendar.vue"
import ProjectGantt from '../../components/Gantt/ProjectGantt.vue';
import projectApi from "../../api/projectApi";
import projectStatusApi from "../../api/projectStatusApi";
import { useRoute, useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import Loading from "../../components/Loading.vue"
import { storeToRefs } from "pinia";
import { useProject } from "../../stores/project";

import Form from '../../components/Project/Form.vue';
import FormTask from '../../components/Task/Form.vue';
import Dialog from "primevue/dialog";
import Button from "primevue/button";
const confirm = useConfirm();
const waiting = ref(false);
const menu = ref();
const route = useRoute();
const router = useRouter();
const visible = ref(false);
const storeProject = useProject();
const { projects, project, taskList, taskEdit, headerFormTask, visibleFormTask, ganttData, statusList } = storeToRefs(storeProject);
const items = ref([

  {
    label: 'Edit',
    icon: 'pi pi-fw pi-pencil',
    command: function () {
      visible.value = true;
    }
  },
  {
    separator: true
  },
  {
    label: 'Delete',
    icon: 'pi pi-fw pi-trash text-danger',
    command: function () {
      confirm.require({
        message: "Bạn có muốn xóa dự án này?",
        header: "Xác nhận",
        icon: "pi pi-exclamation-triangle",
        accept: () => {

          projectApi.Delete(project.value.id).then(() => {
            projects.value = projects.value.filter((item) => {
              return item.id != project.value.id
            });
            if (projects.value.length > 0) {
              console.log(projects.value[0]);
              var first_project = projects.value[0];
              router.push("/project/" + first_project.id);
            } else {
              location.href = "/";
            }
          });
        },
      });
    }
  }
]);

const hide = (e) => {
  visible.value = false;
  visibleFormTask.value = false;
}
const toggle = (event) => {
  menu.value.toggle(event);
};
// const info = () => {
//   console.log(taskList.value);
//   console.log(ganttData.value);
// }
onMounted(() => {
  waiting.value = true;
  Promise.all([projectApi.Get(route.params.id), projectStatusApi.GetList(route.params.id)]).then((res) => {
    waiting.value = false;
    // console.log(res[1]);
    project.value = res[0];
    project.value.list_assignee = project.value.assignees.map((item) => {
      return item.userId;
    });
    statusList.value = res[1];
  });
  // console.log(1);
});

</script>

