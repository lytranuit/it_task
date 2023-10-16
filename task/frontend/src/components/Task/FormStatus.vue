
<template>
    <form id="form-status">
        <div class="mb-3 row">
            <div class="col-9">
                <label for="name" class="form-label">Tên<span class="text-danger">*</span></label>
                <input type="text" class="form-control" id="name" required v-model="statusEdit.name">
            </div>
            <div class="col-3">
                <label for="color" class="form-label">Màu sắc</label>
                <input type="color" class="form-control" id="color" v-model="statusEdit.color">
            </div>
        </div>
        <div class="text-center">
            <Button type="submit" label="Lưu lại" icon="pi pi-save" size="small" @click="save" />
        </div>
    </form>
</template>
<script setup>
import Button from 'primevue/button';
import { onMounted, ref, } from "vue";
import projectStatusApi from '../../api/projectStatusApi';
import { useProject } from "../../stores/project";
import { storeToRefs } from 'pinia';

const storeProject = useProject();
const { statusEdit, statusList } = storeToRefs(storeProject);
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
    projectStatusApi.Save(statusEdit.value).then((res) => {
        statusEdit.value = {};
        var findIndex = statusList.value.findIndex((item) => {
            return item.id == res.id;
        });
        if (findIndex != -1) {
            statusList.value[findIndex] = res;
        } else {
            statusList.value.push(res);
        }
        emit("save");
    });
}
const valid = () => {
    if (!$("#form-status").valid()) {
        return false;
    }
    return true;
}
onMounted(() => {
    statusEdit.value.projectId = props.projectId;
    // console.log("mount");
    // console.log(data.value);
})
</script>