<template>
    <div class="col-lg-12 control-section">
        <div>
            <ejs-gantt ref="gantt" id="ColumnTemplate" :dataSource="data" :height="height" :highlightWeekends="true"
                :taskFields="taskFields" :labelSettings="labelSettings" :splitterSettings="splitterSettings"
                :columns="columns" :resourceFields="resourceFields" :resources="resources" :rowHeight="60"
                :projectStartDate="projectStartDate" :projectEndDate="projectEndDate">
                <e-columns>
                    <e-column field="TaskID" headerText="Task ID" textAlign="Left"></e-column>
                    <e-column field="TaskName" headerText="Task Name" width="250"></e-column>
                    <e-column field="resources" :template="'columnTemplate'" width="250">

                    </e-column>
                    <e-column field="StartDate" headerText="Start Date" width="150"></e-column>
                    <e-column field="Duration" headerText="Duration" width="150"></e-column>
                    <e-column field="Progress" headerText="Progress" width="150"></e-column>
                </e-columns>
                <template #columnTemplate="{ data }">
                    <div class="columnTemplate" v-if="data.ganttProperties.resourceNames">
                        <img :src="'https://ej2.syncfusion.com/vue/demos/source/gantt/images/' +
                            data.ganttProperties.resourceNames +
                            '.png'
                            " style="height: 40px" />
                        <div style="display: inline-block; width: 100%; position: relative">
                            {{ data.ganttProperties.resourceNames }}2323
                        </div>
                    </div>
                </template>
            </ejs-gantt>
        </div>
    </div>
</template>
  
<script lang="ts">
import {
    GanttComponent,
    ColumnDirective,
    ColumnsDirective,
    Selection,
} from '@syncfusion/ej2-vue-gantt';
import { templateData, editingResources } from '../../datasource';
import { isNullOrUndefined } from '@syncfusion/ej2-base';

export default {
    components: {
        'ejs-gantt': GanttComponent,
        'e-column': ColumnDirective,
        'e-columns': ColumnsDirective,
    },
    data: () => {
        return {
            data: templateData,
            height: '450px',
            taskFields: {
                id: 'TaskID',
                name: 'TaskName',
                startDate: 'StartDate',
                endDate: 'EndDate',
                duration: 'Duration',
                progress: 'Progress',
                resourceInfo: 'resources',
                dependency: 'Predecessor',
                child: 'subtasks',
            },
            columns: [
                { field: 'TaskID', width: 140 },
                { field: 'TaskName', width: 250 },
                { field: 'resources' },
                { field: 'StartDate' },
                { field: 'EndDate' },
                { field: 'Duration' },
                { field: 'Predecessor' },
                { field: 'Progress' },
            ],
            resourceFields: {
                id: 'resourceId',
                name: 'resourceName',
            },
            resources: editingResources,
            labelSettings: {
                leftLabel: 'TaskName',
            },
            splitterSettings: {
                columnIndex: 4,
            },
            projectStartDate: new Date('03/24/2019'),
            projectEndDate: new Date('07/06/2019'),
        };
    },
    provide: {
        gantt: [Selection],
    },
    methods: {},
};
</script>
  