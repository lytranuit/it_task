<template>
    <EjsTreeGrid :dataSource="projectData" :treeColumnIndex="1" idMapping='TaskID' parentIdMapping='parentID'
        ref="treegrid">
        <EjsTreeGridColumns>
            <EjsTreeGridColumn field="TaskID" headerText="Task ID" :isPrimaryKey='true' width="70" textAlign="Right">
            </EjsTreeGridColumn>
            <EjsTreeGridColumn field="TaskName" headerText="Task Name" width="100"></EjsTreeGridColumn>
            <EjsTreeGridColumn field="StartDate" headerText="Start Date" format="yMd" width="100" textAlign="Right">
            </EjsTreeGridColumn>
            <EjsTreeGridColumn field="EndDate" headerText="Start Date" format="yMd" width="100" textAlign="Right">
            </EjsTreeGridColumn>
            <EjsTreeGridColumn field="Duration" headerText="Duration" width="90" textAlign="Right">
            </EjsTreeGridColumn>
        </EjsTreeGridColumns>
    </EjsTreeGrid>
</template>
  
<script setup>
import { onMounted } from "vue";
import { TreeGridComponent as EjsTreeGrid, ColumnsDirective as EjsTreeGridColumns, ColumnDirective as EjsTreeGridColumn } from '@syncfusion/ej2-vue-treegrid';
import { projectData } from '../../datasource.js';

onMounted(() => {
});
</script>
  