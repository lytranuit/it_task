
<template>
    <form id="form-task">
        <div class="mb-3 row">
            <div class="col-9">
                <label for="name" class="form-label">Tên công việc <span class="text-danger">*</span></label>
                <input type="text" class="form-control" id="name" required v-model="taskEdit.name">
            </div>
            <div class="col-3">
                <label for="priority" class="form-label">Độ ưu tiên</label>
                <select class="form-control" v-model="taskEdit.priority">
                    <option value="1">Bình thường</option>
                    <option value="2">Cao</option>
                    <option value="3">Ưu tiên</option>
                </select>
            </div>
        </div>
        <div class="mb-3 row">
            <div class="col-12">
                <label for="description" class="form-label">Mô tả</label>
                <textarea class="form-control" id="description" rows="4" v-model="taskEdit.description"></textarea>
            </div>
        </div>

        <div class="mb-3 row">
            <div class="col-12">
                <label for="assignee" class="form-label">Người thực hiện</label>
                <UserTreeSelect name="createuser" :multiple="true" :required="true" v-model="taskEdit.list_assignee">
                </UserTreeSelect>
            </div>
        </div>
        <div class="mb-3 row">
            <div class="col-md-6">
                <label for="startDate" class="form-label">Ngày bắt đầu</label>
                <Calendar v-model="taskEdit.startDate" dateFormat="yy-mm-dd" class="date-custom" :manualInput="false"
                    showIcon showButtonBar/>
            </div>
            <div class="col-md-6">
                <label for="endDate" class="form-label">Ngày kết thúc</label>
                <Calendar v-model="taskEdit.endDate" dateFormat="yy-mm-dd" class="date-custom" :manualInput="false"
                    showIcon showButtonBar/>
            </div>
        </div>
        <div class="mb-3 row">
            <div class="col-md-6">
                <label for="baselineStartDate" class="form-label">Kế hoạch bắt đầu</label>
                <Calendar v-model="taskEdit.baselineStartDate" dateFormat="yy-mm-dd" class="date-custom"
                    :manualInput="false" showIcon showButtonBar/>
            </div>
            <div class="col-md-6">
                <label for="baselineEndDate" class="form-label">Kế hoạch kết thúc</label>
                <Calendar v-model="taskEdit.baselineEndDate" dateFormat="yy-mm-dd" class="date-custom" :manualInput="false"
                    showIcon showButtonBar/>
            </div>
        </div>
        <div class="text-center">
            <Button type="submit" label="Lưu lại" icon="pi pi-save" size="small" @click="save" />
        </div>
    </form>
</template>
<script setup>
import Button from 'primevue/button';
import Calendar from 'primevue/calendar';
import UserTreeSelect from '../TreeSelect/UserTreeSelect.vue';
import { computed, onMounted, ref, watch } from "vue";
import taskApi from '../../api/TaskApi';
import { useProject } from "../../stores/project";
import { storeToRefs } from 'pinia';
const gantt = ref();

const storeProject = useProject();
const { taskEdit, taskList, statusList } = storeToRefs(storeProject);
const props = defineProps({
    projectId: {
        type: Number,
        required: true
    },
});
const emit = defineEmits(["reset", "beforeSave", "save"]);
const save = (e) => {
    e.preventDefault();
    if (!valid())
        return;
    emit("beforeSave");
    taskApi.Save(taskEdit.value).then((res) => {
        taskEdit.value = {};
        taskList.value.push(res);

        if ($("#GanttContainer").length) {
            var obj = document.getElementById("GanttContainer").ej2_instances[0];
            obj.refresh();
        }
        emit("save");
    });
}

const valid = () => {
    if (!$("#form-task").valid()) {
        return false;
    }
    return true;
}
onMounted(() => {
    console.log(gantt);
    taskEdit.value.startDate = new Date();
    taskEdit.value.priority = 1;
    taskEdit.value.statusId = statusList.value[0].id;
    taskEdit.value.projectId = props.projectId;
    // console.log("mount");
    // console.log(data.value);
})
</script>