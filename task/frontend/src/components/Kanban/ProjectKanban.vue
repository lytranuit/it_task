<template>
  <EjsKanban id="kanban" keyField="Status" :dataSource="kanbanData" :cardSettings="cardSettings">
    <EjsKanbanColumns>
      <EjsKanbanColumn headerText="To Do" keyField="Open"></EjsKanbanColumn>
      <EjsKanbanColumn headerText="In Progress" keyField="InProgress"></EjsKanbanColumn>
      <EjsKanbanColumn headerText="Testing" keyField="Testing"></EjsKanbanColumn>
      <EjsKanbanColumn headerText="Done" keyField="Close"></EjsKanbanColumn>
    </EjsKanbanColumns>
  </EjsKanban>
</template>

<script setup>
import { onMounted, ref } from "vue";
// import the component
import { KanbanComponent as EjsKanban, ColumnsDirective as EjsKanbanColumns, ColumnDirective as EjsKanbanColumn } from '@syncfusion/ej2-vue-kanban';
import { kanbanData } from '../../datasource.js';

const cardSettings = ref({
  contentField: "Summary",
  headerField: "Id",
});
onMounted(() => {
});
</script>
