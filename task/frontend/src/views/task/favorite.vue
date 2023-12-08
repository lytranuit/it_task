<template>
    <ProjectGantt></ProjectGantt>
</template>
<script>
import { GanttComponent, Selection, Sort, DayMarkers } from "@syncfusion/ej2-vue-gantt";
import { ganttData } from '../../datasource';
import ProjectGantt from "../../components/Gantt/ProjectGantt.vue";
console.log(ganttData);
export default {
    components: {
    'ejs-gantt': GanttComponent,
    ProjectGantt
},
    data: function () {
        return {
            data: ganttData,
            height: '450px',
            taskFields: {
                id: 'TaskID',
                name: 'TaskName',
                startDate: 'StartDate',
                endDate: 'EndDate',
                child: 'subtasks',
            },
            check: 'true',
            projectStartDate: new Date('02/03/2019'),
            projectEndDate: new Date('03/23/2019'),
            timelineSettings: {
                topTier: {
                    format: 'MMM dd, yyyy',
                    unit: 'Week',
                },
                bottomTier: {
                    unit: 'Day',
                }
            },
            splitterSettings: {
                columnIndex: 0
            },
            labelSettings: {
                rightLabel: 'taskName',
            },
            yearformat: [
                { id: 'MMM "yy', format: 'Jan "18' },
                { id: 'y', format: '2018' },
                { id: 'MMMM, y', format: 'January, 18' },
            ],
            monthformat: [
                { id: 'MMM dd, yyyy', format: 'Jan 01, 2018' },
                { id: 'MMMM', format: 'January' },
                { id: 'MMM', format: 'Jan' },
            ],
            weekformat: [
                { id: 'MMM dd, yyyy', format: 'Jan 01, 2019' },
                { id: 'EEE MMM dd, "yy', format: 'Mon Jan 01, "19' },
                { id: 'EEE MMM dd', format: 'Mon Jan 01' },
            ],
            dayformat: [
                { id: '', format: 'M' },
                { id: 'EEE', format: 'Mon' },
                { id: 'dd', format: '01' }
            ],
            hourformat: [
                { id: 'hh', format: '00' },
                { id: 'hh : mm a', format: '00 : 00 AM' },
                { id: 'h : mm a', format: '0 : 00 AM' },
            ],
            unit: [
                { id: 'Year', unit: 'Year' },
                { id: 'Month', unit: 'Month' },
                { id: 'Week', unit: 'Week' },
                { id: 'Day', unit: 'Day' },
                { id: 'Hour', unit: 'Hour' }
            ],
            fields: { text: 'unit', value: 'id' },
            formatFields: { text: 'format', value: 'id' }
        };

    },
    methods: {
        topTierCick: function () {
            if (this.$refs.topTierCheckbox.ej2Instances.checked) {
                this.$refs.gantt.ej2Instances.timelineSettings.topTier.unit = 'Week';
                this.$refs.topTierCount.ej2Instances.enabled = true;
                this.$refs.topTierFormat.ej2Instances.enabled = true;
                this.$refs.topTierUnit.ej2Instances.enabled = true;
            } else {
                this.$refs.gantt.ej2Instances.timelineSettings.topTier.unit = 'None';
                this.$refs.topTierCount.ej2Instances.enabled = false;
                this.$refs.topTierFormat.ej2Instances.enabled = false;
                this.$refs.topTierUnit.ej2Instances.enabled = false;
            }
        },
        bottomTierCick: function () {
            if (this.$refs.bottomTierCheckbox.ej2Instances.checked) {
                this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.unit = 'Day';
                this.$refs.bottomTierCount.ej2Instances.enabled = true;
                this.$refs.bottomTierFormat.ej2Instances.enabled = true;
                this.$refs.bottomTierUnit.ej2Instances.enabled = true;
            } else {
                this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.unit = 'None';
                this.$refs.bottomTierCount.ej2Instances.enabled = false;
                this.$refs.bottomTierFormat.ej2Instances.enabled = false;
                this.$refs.bottomTierUnit.ej2Instances.enabled = false;
            }
        },
        topTierCountchange: function (e) {
            let count = e.value;
            this.$refs.gantt.ej2Instances.timelineSettings.topTier.count = count;
        },
        bottomTierCountchange: function (e) {
            let count = e.value;
            this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.count = count;
        },
        topUnitChange: function (e) {
            let unit = e.value;
            if (unit === 'Year') {
                this.$refs.topTierFormat.ej2Instances.dataSource = this.yearformat;
            } else if (unit === 'Month') {
                this.$refs.topTierFormat.ej2Instances.dataSource = this.monthformat;
            } else if (unit === 'Week') {
                this.$refs.topTierFormat.ej2Instances.dataSource = this.weekformat;
            } else if (unit === 'Day') {
                this.$refs.topTierFormat.ej2Instances.dataSource = this.dayformat;
            } else {
                this.$refs.topTierFormat.ej2Instances.dataSource = this.hourformat;
            }
            this.$refs.topTierFormat.ej2Instances.refresh();
            this.updateUnitWidth(unit, 'top');
            this.$refs.gantt.ej2Instances.timelineSettings.topTier.unit = unit;
        },
        bottomUnitChange: function (e) {
            let unit = e.value;
            this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.unit = unit;
            if (unit === 'Year') {
                this.$refs.bottomTierFormat.ej2Instances.dataSource = this.yearformat;
            } else if (unit === 'Month') {
                this.$refs.bottomTierFormat.ej2Instances.dataSource = this.monthformat;
            } else if (unit === 'Week') {
                this.$refs.bottomTierFormat.ej2Instances.dataSource = this.weekformat;
            } else if (unit === 'Day') {
                this.$refs.bottomTierFormat.ej2Instances.dataSource = this.dayformat;
            } else {
                this.$refs.bottomTierFormat.ej2Instances.dataSource = this.hourformat;
            }
            this.$refs.bottomTierFormat.ej2Instances.refresh();
            this.updateUnitWidth(unit, 'bottom');
            this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.unit = unit;
        },
        bottomFormatChange: function (e) {
            let format = e.value;
            this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.format = format.toString();
        },
        topFormatChange: function (e) {
            let format = e.value;
            this.$refs.gantt.ej2Instances.timelineSettings.topTier.format = format.toString();
        },
        unitWidthChange: function (e) {
            let width = e.value;
            this.$refs.gantt.ej2Instances.timelineSettings.timelineUnitSize = width;
        },
        updateUnitWidth: function (unit, tier) {
            let topUnit = tier === 'top' ? unit : this.$refs.gantt.ej2Instances.timelineSettings.topTier.unit;
            let bottomUnit = tier === 'bottom' ? unit : this.$refs.gantt.ej2Instances.timelineSettings.bottomTier.unit;
            let units = ['None', 'Hour', 'Day', 'Week', 'Month', 'Year'];
            let bootomCellUnit;
            let unitWidth;
            if (units.indexOf(topUnit) === 0 && units.indexOf(bottomUnit) === 0) {
                bootomCellUnit = 'Day';
            } else if (units.indexOf(topUnit) === 0 && units.indexOf(bottomUnit) > 0) {
                bootomCellUnit = bottomUnit;
            } else if (units.indexOf(topUnit) > 0 && units.indexOf(bottomUnit) === 0) {
                bootomCellUnit = topUnit;
            } else if (units.indexOf(topUnit) <= units.indexOf(bottomUnit)) {
                bootomCellUnit = topUnit;
            } else {
                bootomCellUnit = bottomUnit;
            }
            if (bootomCellUnit === 'Year') {
                unitWidth = 2000;
            } else if (bootomCellUnit === 'Month') {
                unitWidth = 300;
            } else if (bootomCellUnit === 'Week') {
                unitWidth = 150;
            } else if (bootomCellUnit === 'Day') {
                unitWidth = 33;
            } else if (bootomCellUnit === 'Hour') {
                unitWidth = 25;
            }
            this.$refs.unitWidth.ej2Instances.value = unitWidth;
        }
    },
    mounted: function () {
        console.log(this.data);
    },

    provide: {
        gantt: [Selection, Sort, DayMarkers]
    }
}
</script>
