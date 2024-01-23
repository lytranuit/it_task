<template>
  <EjsGantt :key="key" ref='gantt' :dataSource="taskList" :treeColumnIndex='1' id="GanttContainer"
    :taskFields="taskFields" :editSettings="editSettings" :toolbar="toolbar" :allowSelection="true"
    :enableContextMenu="true" :contextMenuItems="contextMenuItems" :allowRowDragAndDrop="true"
    :actionComplete="actionComplete" :includeWeekend="true" :columns="columns" :splitterSettings="splitterSettings"
    :timelineSettings="timelineSettings" height="600px" :taskbarTemplate="'taskbarTemplate'"
    :labelSettings="labelSettings" :eventMarkers="eventMarkers" :renderBaseline="true" rowHeight="50"
    :allowUnscheduledTasks='true' :enableImmutableMode="true" :project-start-date="projectStartDate"
    :project-end-date="projectEndDate">
    <EjsGanttColumns>
      <EjsGanttColumn field='id' headerText='Task ID' textAlign='Left'>

      </EjsGanttColumn>
      <EjsGanttColumn field='name' headerText='Task Name' width='250'>
      </EjsGanttColumn>
      <EjsGanttColumn field='stauts' headerText='Nhóm công việc' :template="'statusTemplate'"></EjsGanttColumn>
      <EjsGanttColumn field='state' headerText='Tình trạng' :template="'stateTemplate'"></EjsGanttColumn>
      <EjsGanttColumn field='assignees' headerText='Người thực hiện' :template="'listAssignTemplate'"></EjsGanttColumn>
      <EjsGanttColumn field='progress' headerText='Tiến độ' :template="'prgressTemplate'">
      </EjsGanttColumn>
      <EjsGanttColumn field='point' headerText='Chấm điểm'></EjsGanttColumn>
    </EjsGanttColumns>
    <template v-slot:taskbarTemplate="{ data }">
      <div class="e-gantt-child-taskbar-inner-div e-gantt-child-taskbar"
        :class="{ 'e-hide': data.taskData.startDate == null, 'bg-danger': data.taskData.is_overdue }"
        :style="'height:22px;background: #d1d3e9;border: 1px solid #5869c5;'">
        <div class="e-gantt-child-progressbar-inner-div e-gantt-child-progressbar"
          :style="'height: 100%;width:' + data.progress + '%;background:#22C55E'">
          <span class="e-task-label" style="line-height: 21px;">{{ data.taskData.name }}</span>
        </div>
      </div>
    </template>
    <template v-slot:rightLabelTemplate="{ data }">
      <div v-if="data.taskData.assignees">
        <AvatarGroup>
          <Avatar v-for="assignee in data.taskData.assignees" :image="assignee.user.image_url" shape="circle"
            :title="assignee.user.fullName" />
          <!-- <Avatar label="+2" shape="circle" size="large" style="background-color: '#9c27b0', color: '#ffffff'" /> -->
        </AvatarGroup>
      </div>
    </template>
    <template v-slot:prgressTemplate="{ data }">
      <span>{{ data.progress }}%</span>
    </template>
    <template v-slot:listAssignTemplate="{ data }">
      <template v-if="data.taskData.assignees.length > 0">
        <AvatarGroup>
          <Avatar v-for="item in data.taskData.assignees" :key="item.id" :image="item.user.image_url"
            :title="item.user.fullName" size="small" shape="circle" />
          <!-- <Avatar label="+" shape="circle" size="small" /> -->
        </AvatarGroup>
      </template>
    </template>
    <template v-slot:statusTemplate="{ data }">
      <Tag :value="data.taskData.status.name" :style="{ background: data.taskData.status.color }">
      </Tag>
    </template>
    <template v-slot:stateTemplate="{ data }">
      <div class="d-flex">
        <template v-if="data.taskData.is_overdue && data.progress != 100">
          <Knob valueColor='#ed7d31' v-model="data.progress" valueTemplate="" :size="20" readonly />
          <div class="align-self-center ml-2" style="color:#ed7d31">
            Quá hạn
          </div>
        </template>
        <template v-else-if="data.taskData.is_overdue && data.progress == 100">
          <Knob valueColor='#01b0f1' v-model="data.progress" valueTemplate="" :size="20" readonly />
          <div class="align-self-center ml-2" style="color:#01b0f1">
            Hoàn thành trễ hạn
          </div>
        </template>
        <template v-else-if="data.progress == 0">
          <Knob v-model="data.taskData.progress" valueTemplate="" :size="20" readonly />
          <div class="align-self-center ml-2">
            Chưa thực hiện
          </div>
        </template>
        <template v-else-if="data.progress == 100">
          <Knob valueColor='#28aa37' v-model="data.taskData.progress" valueTemplate="" :size="20" readonly />
          <div class="align-self-center ml-2">
            Hoàn thành
          </div>

        </template>
        <template v-else>
          <Knob valueColor='#28aa37' v-model="data.progress" valueTemplate="" :size="20" readonly />
          <div class="align-self-center ml-2">
            Hoàn thành <span>{{ data.progress }}%</span>
          </div>
        </template>
      </div>
      <!-- <span>{{ data.progress }}%</span> -->
    </template>
  </EjsGantt>
  <div class="_kqsw1n9t _1e0c1txw _1pbyasm9 _1reoewfl _18m9ewfl _4cvresu3 _lcxvglyw">
    <span class="p-buttonset">
      <Button label="Weeks" size="small" :outlined="view != 'Week'" @click="viewWeek" />
      <Button label="Months" size="small" :outlined="view != 'Month'" @click="viewMonth" />
      <Button label="Year" size="small" :outlined="view != 'Year'" @click="viewYear" />
    </span>

  </div>
  <SidebarForm v-model:visible="visibleFormTask" v-if="visibleFormTask"></SidebarForm>

  <Dialog modal :style="{ width: '50vw' }" :breakpoints="{ '1199px': '75vw', '575px': '95vw' }">
    <template #header>
      <span class="p-buttonset">
        <Button size="small" raised :outlined="taskEdit.progress < 100" @click="toggle"
          :class="{ 'p-button-success': taskEdit.progress == 100 }">
          <span class="">Hoàn thành | </span>
          <span class="px-1">{{ taskEdit.progress }}%</span>
          <i class="pi pi-chevron-down"></i>
        </Button>
      </span>
      <OverlayPanel ref="op">
        <div style="width: 200px;">
          <div>
            <div>Tiến độ công việc</div>
            <Slider v-model="taskEdit.progress" class="mt-3" />
          </div>
          <div class="mt-3 text-center">
            <InputText v-model.number="taskEdit.progress" style="width:50px;" />
          </div>
        </div>
      </OverlayPanel>
    </template>
    <FormTask :projectId="projectId" @save="hideTask">
    </FormTask>
  </Dialog>
</template>

<script setup>
import { onMounted, provide, ref, watch } from "vue";
import { getValue } from '@syncfusion/ej2-base';
import OverlayPanel from 'primevue/overlaypanel';
// import the component
import {
  GanttComponent as EjsGantt, ColumnsDirective as EjsGanttColumns, ColumnDirective as EjsGanttColumn, Edit, RowDD, Selection, Toolbar,
  ContextMenu, DayMarkers, Sort
} from '@syncfusion/ej2-vue-gantt';
// import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
// import { ganttData } from '../../datasource.js';
import FormTask from "../Task/Form.vue";
import ColumnTemplate from "../Task/ColumnTemplate.vue";
import { useProject } from "../../stores/project";
import { storeToRefs } from "pinia";
import taskApi from '../../api/TaskApi'
import { useRoute } from "vue-router";
import Button from "primevue/button";
import Avatar from 'primevue/avatar';
import Slider from 'primevue/slider';
import InputText from 'primevue/inputtext';
import AvatarGroup from 'primevue/avatargroup';   //Optional for grouping
import Dialog from "primevue/dialog";
import { rand } from '../../utilities/rand';
import Tag from "primevue/tag";
import Knob from 'primevue/knob';
import SidebarForm from "../Task/SidebarForm.vue";

const storeProject = useProject();
const { taskList, taskEdit, key } = storeToRefs(storeProject);
const op = ref();
const toggle = (event) => {
  op.value.toggle(event);
}
const alert = (data) => {
  console.log(data);
}
const route = useRoute();
const contextMenuItems = ref([
  "AutoFitAll",
  "AutoFit",
  "DeleteTask",
  // "Add",
  "Save",
  "Cancel",
  "SortAscending",
  "SortDescending",
]);
const taskFields = ref({
  id: 'id',
  name: 'name',
  startDate: 'startDate',
  endDate: 'endDate',
  parentID: 'parentId',
  baselineStartDate: 'baselineStartDate',
  baselineEndDate: 'baselineEndDate',
  progress: 'progress',

});
const gantt = ref();
const view = ref("Week");
const timelineSettings = ref({
  showTooltip: true,
  timelineUnitSize: 33,
  timelineViewMode: 'Week',
  updateTimescaleView: true,
});
// watch(taskList, () => {
//   console.log(taskList.value)
// }, { deep: true, immediate: true });
const viewWeek = () => {
  view.value = "Week";
  gantt.value.ej2Instances.timelineSettings.timelineViewMode = "Week";
  gantt.value.ej2Instances.timelineSettings.timelineUnitSize = 33;
  // gantt.value.ej2Instances.timelineSettings.topTier.unit = "Week";
  // timelineSettings.value.timelineUnitSize = 33;
  // console.log(timelineSettings.value);
  // var obj = document.getElementById("GanttContainer").ej2_instances[0];
  // obj.refresh();
}
const viewMonth = () => {
  view.value = "Month";
  gantt.value.ej2Instances.timelineSettings.timelineViewMode = "Month"
  gantt.value.ej2Instances.timelineSettings.timelineUnitSize = 80;
}
const viewYear = () => {
  view.value = "Year";
  gantt.value.ej2Instances.timelineSettings.timelineViewMode = "Year"
  gantt.value.ej2Instances.timelineSettings.timelineUnitSize = 80;
}
const columns = ref([
  { field: 'id', headerText: 'ID', textAlign: 'Left', allowSorting: false, maxWidth: "50px" },
  { field: 'name', headerText: 'Task Name', textAlign: 'Left', allowSorting: false },
  { field: 'rank', headerText: 'Rank', textAlign: 'Left', visible: false, },
  { field: 'progress', headerText: 'Tiến độ', textAlign: 'Left', visible: false, },
  { field: 'baselineStartDate', headerText: 'Planned start time' },
  { field: 'baselineEndDate', headerText: 'Planned end time' },
]);
const sortSettings = ref({ columns: [{ field: 'rank', direction: 'Ascending' }], allowUnsort: true });
const splitterSettings = ref({
  position: "80%",
  // minimum: "200",
  // separatorSize: 5,
  view: 'Default'
});
const projectStartDate = ref(new Date(2023, 8, 1));
const projectEndDate = ref(new Date(2023, 12, 31));
const editSettings = ref({
  allowAdding: false,
  allowEditing: true,
  allowDeleting: true,
  allowTaskbarEditing: false,
  showDeleteConfirmDialog: true,
  mode: "Dialog"
});
const labelSettings = ref({
  rightLabel: "rightLabelTemplate",
})
const eventMarkers = ref([
  {
    day: new Date(),
    label: ''
  }
]);
const toolbar = ['Add', 'Update', 'Delete', 'Cancel', "ZoomIn", "ZoomOut", "ZoomToFit", "PrevTimeSpan", "NextTimeSpan"];
const selectionSettings = ref({
  type: 'Multiple'
});
// const actionBegin = (e) => {
//   console.log(e);
//   if (e.requestType == "beforeOpenEditDialog") {
//     $("#GanttContainer_Tab").html("<div>tran</div>")
//   }
//   return true;
// }
const projectId = ref();

const hideTask = (e) => {
  visibleFormTask.value = false;
  key.value = rand();
}
const visibleFormTask = ref(false);
const actionComplete = (e) => {
  // console.log(e);
  if (e.requestType == "delete") {
    var data = e.data;
    for (var item of data) {
      taskApi.Delete(item.id);
    }
  } else if (e.requestType == "save") {
    var data = e.modifiedTaskData;

    for (var key in data) {
      var item = data[key];
      if (e.action == "TaskbarEditing") {
        var ganttProperties = e.modifiedRecords[key].ganttProperties;
        item.startDate = ganttProperties.startDate;
        item.endDate = ganttProperties.endDate;
      }
      taskApi.Save(item);
    }
  } else if (e.requestType == "rowDropped") { /// ORDER
    var data = e.data;
    var dropRecord = e.dropRecord;
    taskApi.Rank(data[0].id, dropRecord.id, e.dropPosition);
  } else if (e.requestType == "openEditDialog") {
    $("#GanttContainer_dialog").parent().remove();
    // console.log(e.data);
    var data = e.data.taskData;
    taskEdit.value = data;
    projectId.value = data.projectId;
    visibleFormTask.value = true;
  }
  return true;
}
provide('gantt', [Edit, RowDD, Selection, ContextMenu, Toolbar, DayMarkers, Sort]);
onMounted(() => {
  taskApi.GetList(route.params.id).then((res) => {
    // console.log(res);
    taskList.value = res;
    gantt.value.ej2Instances.fitToProject();
  });
});
// watch(taskList, () => {
//   // console.log(gantt.value.updateDataSource);
//   // gantt.value.refresh();
//   var obj = document.getElementById("GanttContainer").ej2_instances[0];
//   console.log(obj);
//   obj.refresh();
//   // gantt.value.dataSource = taskList.value;
// }, { deep: true })
</script>
<style>
@import '~@syncfusion/ej2-base/styles/material.css';
@import '~@syncfusion/ej2-buttons/styles/material.css';
@import '~@syncfusion/ej2-calendars/styles/material.css';
@import '~@syncfusion/ej2-dropdowns/styles/material.css';
@import '~@syncfusion/ej2-inputs/styles/material.css';
@import '~@syncfusion/ej2-navigations/styles/material.css';
@import '~@syncfusion/ej2-lists/styles/material.css';
@import '~@syncfusion/ej2-layouts/styles/material.css';
@import '~@syncfusion/ej2-popups/styles/material.css';
@import '~@syncfusion/ej2-splitbuttons/styles/material.css';
@import '~@syncfusion/ej2-grids/styles/material.css';
@import '~@syncfusion/ej2-richtexteditor/styles/material.css';
@import '~@syncfusion/ej2-treegrid/styles/material.css';
@import "~@syncfusion/ej2-vue-gantt/styles/material.css";

.e-gantt-splitter {
  width: 100% !important;
}

.e-custom {
  background-color: #6d619b;
  border: 1px solid #3f51b5;
  height: 100%;
  border-radius: 5px;
  text-overflow: ellipsis;
  display: flex !important;
}

.e-custom .e-task-label {
  align-self: center;
}

.e-hide .e-taskbar-main-container {
  display: none;

}

.moments,
.face-mask,
.oscar {
  position: relative;
  top: 14px;
  left: 5px;
  padding-right: 4px;
}

.e-milestone-top {
  border-bottom-color: #7ab748 !important;
  border-bottom: 1px solid #3f51b5;
}

.e-milestone-bottom {
  border-top-color: #7ab748 !important;
  border-top: 1px solid #3f51b5;
}
</style>