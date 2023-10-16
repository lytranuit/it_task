<template>
  <div class="schedule-vue-sample">
    <div class="col-md-12 control-section">
      <div class="content-wrapper">
        <ScheduleComponent id='Schedule' ref="ScheduleObj" height="650px" :eventSettings='eventSettings'>
          <ViewsDirective>
            <ViewDirective option="Day"></ViewDirective>
            <ViewDirective option="Week"></ViewDirective>
            <ViewDirective option="Month"></ViewDirective>
          </ViewsDirective>
        </ScheduleComponent>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ScheduleComponent, ViewDirective, ViewsDirective, Day, Week, Month, Agenda, Resize, DragAndDrop } from "@syncfusion/ej2-vue-schedule";
import { onMounted, provide, ref } from "vue";
import { useRoute } from "vue-router";
import taskApi from "../../api/TaskApi"
import { useProject } from "../../stores/project";
import { storeToRefs } from "pinia";
const storeProject = useProject();
const { taskList } = storeToRefs(storeProject);
const route = useRoute();
const eventSettings = ref({
  dataSource: taskList.value, fields: {
    id: 'id',
    subject: { name: 'name' },
    description: { name: 'description' },
    startTime: { name: 'startDate' },
    endTime: { name: 'endDate' },
    recurrenceRule: { name: 'recurrenceRule' }
  }
});

provide('schedule', [Day, Week, Month, Agenda, Resize, DragAndDrop]);
onMounted(() => {
  taskApi.GetList(route.params.id).then((res) => {
    taskList.value = res;
    taskList.value = taskList.value.map((item1) => {
      item1.list_assignee = item1.assignees.map((item) => {
        return item.userId;
      });
      return item1;
    });
  });
});
</script>

<style></style>