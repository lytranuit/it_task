<template>
  <EjsGantt ref='gantt' :dataSource="ganttData" :treeColumnIndex='1' id="GanttContainer" :taskFields="taskFields"
    :editSettings="editSettings" :toolbar="toolbar" :allowSelection="true" :enableContextMenu="true"
    :contextMenuItems="contextMenuItems" :allowRowDragAndDrop="true" :actionComplete="actionComplete"
    :includeWeekend="true" :columns="columns" :splitterSettings="splitterSettings" :timelineSettings="timelineSettings"
    height="600px" :taskbarTemplate="'taskbarTemplate'" :labelSettings="labelSettings" :eventMarkers="eventMarkers"
    :renderBaseline="true" rowHeight="50" :allowUnscheduledTasks='true' :projectStartDate="projectStartDate"
    :projectEndDate="projectEndDate">
    <EjsGanttColumns>
      <EjsGanttColumn field='id' headerText='Task ID' textAlign='Left'></EjsGanttColumn>
      <EjsGanttColumn field='name' headerText='Task Name' width='250'></EjsGanttColumn>
    </EjsGanttColumns>
    <template v-slot:taskbarTemplate="{ data }">
      <div class="e-gantt-parent-taskbar e-custom" :style="'background-color:' + data.taskData.status.color"
        :class="{ 'e-hide': data.taskData.startDate == null }">
        <span class="e-task-label">{{ data.taskData.name }}</span>
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

  </EjsGantt>
  <div data-testid="layout-controller.ui.bottom-right-corner.container.styled-container"
    class="_kqsw1n9t _1e0c1txw _1pbyasm9 _1reoewfl _18m9ewfl _4cvresu3 _lcxvglyw"
    style="right: 24px; bottom: 24px; position: absolute;">
    <span class="p-buttonset">
      <Button label="Weeks" size="small" :outlined="view != 'Week'" @click="viewWeek" />
      <Button label="Months" size="small" :outlined="view != 'Month'" @click="viewMonth" />
      <Button label="Year" size="small" :outlined="view != 'Year'" @click="viewYear" />
    </span>

  </div>
</template>

<script setup>
import { onMounted, provide, ref, watch } from "vue";
// import the component
import {
  GanttComponent as EjsGantt, ColumnsDirective as EjsGanttColumns, ColumnDirective as EjsGanttColumn, Edit, RowDD, Selection, Toolbar,
  ContextMenu, DayMarkers, Sort
} from '@syncfusion/ej2-vue-gantt';
// import { DataManager, WebApiAdaptor } from "@syncfusion/ej2-data";
// import { ganttData } from '../../datasource.js';
import ColumnTemplate from "../Task/ColumnTemplate.vue";
import { useProject } from "../../stores/project";
import { storeToRefs } from "pinia";
import taskApi from '../../api/TaskApi'
import { useRoute } from "vue-router";
import Button from "primevue/button";
import Avatar from 'primevue/avatar';
import AvatarGroup from 'primevue/avatargroup';   //Optional for grouping
const storeProject = useProject();
const { ganttData, taskList } = storeToRefs(storeProject);

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
  baselineEndDate: 'baselineEndDate'
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
  console.log(timelineSettings.value);
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
  { field: 'baselineStartDate', headerText: 'Planned start time' },
  { field: 'baselineEndDate', headerText: 'Planned end time' },
]);
const sortSettings = ref({ columns: [{ field: 'rank', direction: 'Ascending' }], allowUnsort: true });
const splitterSettings = ref({
  position: "50%",
  // minimum: "200",
  // separatorSize: 5,
  view: 'Default'
});
const projectStartDate = ref(new Date(2023, 8, 25));
const editSettings = ref({
  allowAdding: false,
  allowEditing: true,
  allowDeleting: true,
  allowTaskbarEditing: true,
  showDeleteConfirmDialog: true
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
const toolbar = ['Add', 'Update', 'Delete', 'Cancel', "ZoomIn", "ZoomOut"];
const selectionSettings = ref({
  type: 'Multiple'
});
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
  }
  return true;
}
provide('gantt', [Edit, RowDD, Selection, ContextMenu, Toolbar, DayMarkers, Sort]);
onMounted(() => {
  taskApi.GetList(route.params.id).then((res) => {
    console.log(res);
    taskList.value = res;
    console.log(taskList.value[1].startDate);
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