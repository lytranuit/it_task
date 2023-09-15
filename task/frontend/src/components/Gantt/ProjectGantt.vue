<template>
  <EjsGantt ref='gantt' :dataSource="ganttData" id="GanttContainer" :taskFields="taskFields"></EjsGantt>
</template>

<script setup>
import { onMounted, ref } from "vue";
// import the component
import { GanttComponent as EjsGantt, ColumnsDirective as EjsGanttColumns, ColumnDirective as EjsGanttColumn } from '@syncfusion/ej2-vue-gantt';
import { ganttData } from '../../datasource.js';

const taskFields = ref({
  id: 'TaskID',
  name: 'TaskName',
  startDate: 'StartDate',
  endDate: 'EndDate',
  duration: 'Duration',
  progress: 'Progress',
  child: 'subtasks'
});
onMounted(() => {
});
</script>
