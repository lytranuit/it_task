<template>
  <div class="row">
    <div class="col-11">
      <EjsKanban id="kanban" ref="kanban" keyField="statusId" :dataSource="taskList" :cardSettings="cardSettings"
        min-height="600px" width="auto" height="auto" :allowDragAndDrop="true" :actionComplete='OnActionComplete'
        :dialogSettings="dialogSettings" :dialogOpen='dialogOpen'>
        <EjsKanbanColumns>
          <EjsKanbanColumn v-for="column in statusList" :key="column.id" :headerText="column.name" :keyField="column.id"
            :allowToggle="true" :template="'columnTemplate'">
          </EjsKanbanColumn>
        </EjsKanbanColumns>
        <template #columnTemplate="{ data }">

          <div style="cursor: pointer;    align-items: center;" @click="alert(data)">
            <TagHeader :status-id="data.keyField"></TagHeader> - {{ data.count }} công việc
          </div>
          <div class="d-inline-block ml-auto">
            <i class="fas fa-ellipsis-h" aria-haspopup="true" aria-controls="overlay_menu"
              @click="toggle_menu($event, data.keyField)"></i>
            <Menu ref="menu" id="overlay_menu" :model="items" :popup="true" />
          </div>

        </template>
        <template #cardTemplate="{ data }">
          <div class="e-card-priority" :class="'priority-' + data.priority">
            <div class="e-card-header">
              <div class="e-card-header-caption">
                <div class="e-card-header-title e-tooltip-text">#{{ data.id }} - {{ data.name }}</div>
              </div>
            </div>
            <div class="e-card-content e-tooltip-text">
              <div class="e-text">{{ data.description }}</div>
            </div>
            <div class="e-card-footer">
              <AvatarGroup>
                <Avatar v-for="assignee in data.assignees" :image="assignee.user.image_url" shape="circle"
                  :title="assignee.user.fullName" />
                <!-- <Avatar label="+2" shape="circle" size="large" style="background-color: '#9c27b0', color: '#ffffff'" /> -->
              </AvatarGroup>
            </div>
          </div>
        </template>
        <template #formTemplate="{ data }">
          <FormTask :projectId="project.id" @save="hideTask">
          </FormTask>
        </template>
      </EjsKanban>
    </div>
    <div class="col-1">
      <Button icon="pi pi-plus" severity="success" outlined @click="visibleForm = true"></Button>
    </div>
    <Dialog v-model:visible="visibleForm" modal :header="headerFormTask" :style="{ width: '50vw' }">
      <FormStatus @beforeSave="hide" :projectId="project.id">
      </FormStatus>
    </Dialog>
    <ConfirmDialog group="templating1" :pt="{
      root: { style: 'width:50vw' }
    }">
      <template #message="slotProps">
        <div class="row w-100">
          <div class="col-6">
            <div>Trạng này này sẽ xóa:</div>
            <div class="mt-2">
              <Tag rounded :style="{ background: menu.recordColor }">
                <span style="text-decoration:line-through ;">{{ menu.recordName }}</span>
              </Tag>
            </div>
          </div>
          <div class="col-6">
            <div>Chuyển sang trạng thái:</div>
            <select class="form-control mt-2" v-model="moveId">
              <template v-for="status in statusList">
                <option v-if="status.id != menu.recordId" :value="status.id">{{ status.name }}</option>
              </template>
            </select>
          </div>
        </div>
      </template>
    </ConfirmDialog>
  </div>
</template>

<script setup>
import { computed, createApp, h, onMounted, ref } from "vue";
// import the component
import { KanbanComponent as EjsKanban, ColumnsDirective as EjsKanbanColumns, ColumnDirective as EjsKanbanColumn } from '@syncfusion/ej2-vue-kanban';
// import { kanbanData } from '../../datasource.js';
import ConfirmDialog from 'primevue/confirmdialog';
import { useProject } from "../../stores/project";
import { storeToRefs } from "pinia";
import { useConfirm } from "primevue/useconfirm";
import projectStatusApi from '../../api/projectStatusApi'
import taskApi from "../../api/TaskApi"
import Button from "primevue/button";
import Dialog from "primevue/dialog";
import Avatar from 'primevue/avatar';
import AvatarGroup from 'primevue/avatargroup';   //Optional for grouping
import Tag from "primevue/tag";
import FormStatus from "../Task/FormStatus.vue";
import FormTask from "../Task/Form.vue";
import Menu from 'primevue/menu';
import { useRoute } from "vue-router";
import TagHeader from "./TagHeader.vue";
import { rand } from '../../utilities/rand';
const confirm = useConfirm();
const moveId = ref(0);
const storeProject = useProject();
const { statusList, taskList, project, taskEdit, statusEdit, key } = storeToRefs(storeProject);
const alert = (data) => {
  console.log(data)
}
const cardSettings = ref({
  contentField: "name",
  headerField: "id",
  template: 'cardTemplate',
});
const kanban = ref();
const dialogSettings = ref({
  template: 'formTemplate',
  width: "50vw"
});

const menu = ref();
const items = ref([

  {
    label: 'Left',
    icon: 'pi pi-arrow-left',
    command: () => {
      var columnId = menu.value.recordId;
      var findIndex = statusList.value.findIndex((item) => {
        return item.id == columnId;
      });
      var find = statusList.value[findIndex];
      var prev = statusList.value[findIndex - 1];
      statusList.value[findIndex] = prev;
      statusList.value[findIndex - 1] = find;
      projectStatusApi.Rank(columnId, prev.id, "topSegment");
    }
  },
  {
    label: 'Right',
    icon: 'pi pi-arrow-right',
    command: () => {
      var columnId = menu.value.recordId;
      var findIndex = statusList.value.findIndex((item) => {
        return item.id == columnId;
      });
      var find = statusList.value[findIndex];
      var next = statusList.value[findIndex + 1];
      statusList.value[findIndex] = next;
      statusList.value[findIndex + 1] = find;
      projectStatusApi.Rank(columnId, next.id, "bottomSegment");

    }
  }, {
    label: 'Edit',
    icon: 'pi pi-pencil',
    command: (e) => {
      var columnId = menu.value.recordId;
      showEdit({ keyField: columnId });
    }
  },
  {
    label: 'Delete',
    icon: 'pi pi-trash',
    command: () => {
      var columnId = menu.value.recordId;
      var findIndex = statusList.value.findIndex((item) => {
        return item.id == columnId;
      });
      menu.value.recordName = statusList.value[findIndex].name;
      menu.value.recordColor = statusList.value[findIndex].color;
      moveId.value = statusList.value[0].id;

      if (findIndex == 0 && statusList.value.length > 1)
        moveId.value = statusList.value[1].id;

      confirm.require({
        group: 'templating1',
        message: '',
        header: 'Chuyển công việc',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          projectStatusApi.Delete(columnId, moveId.value).then(() => {
            statusList.value = statusList.value.filter((item) => {
              return item.id != columnId
            })
            taskList.value = taskList.value.map((item) => {
              if (item.statusId == columnId) {
                item.statusId = moveId.value;
              }
              return item;
            })
          });
        },
        acceptLabel: "Delete",
        acceptIcon: "pi pi-trash",
        acceptClass: "p-button-danger",
        rejectLabel: "Cancle",
      });
      // confirm.require({
      //   group: 'templating',
      //   header: 'Move work from TEST column',
      //   icon: 'pi pi-question-circle',
      //   acceptIcon: 'pi pi-trash',
      //   rejectIcon: 'pi pi-times',
      //   accept: () => {

      //   },
      //   reject: () => {

      //   }
      // });

    }
  }
]);

const toggle_menu = (event, id) => {
  // console.log(menu);
  menu.value.recordId = id;
  /////
  var findIndex = statusList.value.findIndex((item) => {
    return item.id == id;
  });
  items.value[0].disabled = false;
  items.value[1].disabled = false;
  if (findIndex == 0) {
    items.value[0].disabled = true;
  } else if (findIndex == statusList.value.length - 1) {
    items.value[1].disabled = true;
  }
  ///////
  menu.value.toggle(event);
};
const dialogOpen = (args) => {
  var data = args.data;
  taskEdit.value = data;
  var element = args.element;
  $(element).css("width", "50vw");
  $(".e-footer-content", element).remove();
}
const visibleForm = ref(false);
const headerFormTask = ref("Tạo mới");

const hide = (e) => {
  visibleForm.value = false;
}
const saving = ref(false);
const hideTask = (e) => {
  kanban.value.ej2Instances.closeDialog();
  key.value = rand();
}
const OnActionComplete = (e) => {
  // console.log(e);
  if (e.requestType == "cardChanged") {
    var data = e.changedRecords;

    for (var key in data) {
      var item = data[key];
      taskApi.Save(item);
    }
  }
}
const showEdit = (data) => {
  var id = data.keyField;
  projectStatusApi.Get(id).then(function (res) {
    statusEdit.value = res;
    visibleForm.value = true;
  })
}
const route = useRoute();
onMounted(() => {
  taskApi.GetList(route.params.id).then((res) => {
    taskList.value = res;
    // taskList.value = taskList.value.map((item1) => {
    //   item1.list_assignee = item1.assignees.map((item) => {
    //     return item.userId;
    //   });
    //   return item1;
    // });
  });
});
</script>

<style>
@import '../../../node_modules/@syncfusion/ej2-vue-kanban/styles/material.css';

.e-header-text {
  cursor: pointer;
}

.e-header-title {
  align-items: center;
}

.e-card-priority {
  border-left: 3px solid;
}

.priority-1 {
  border-left-color: gray;
}

.priority-2 {
  border-left-color: wheat;
}

.priority-3 {
  border-left-color: red;
}
</style>