
<template>
    <span class="float-right" @click.stop="show($event)"><i class="fas fa-plus-circle"></i></span>
    <Dialog v-model:visible="visible" modal header="Dự án mới" :style="{ width: '50vw' }">
        <div class="row">
            <div class="mb-3 col-12">
                <label for="name" class="form-label">Tên dự án</label>
                <input type="text" class="form-control" id="name" required v-model="data.name">
            </div>
            <div class="mb-3 col-12">
                <label for="description" class="form-label">Mô tả</label>
                <textarea class="form-control" id="description" rows="4" v-model="data.description"></textarea>
            </div>
            <div class="mb-3 col-12">
                <label for="assignee" class="form-label">Người tham gia</label>
                <UserTreeSelect name="createuser" :multiple="true" :required="true" v-model="data.list_assignee">
                </UserTreeSelect>
            </div>
            <div class="col-12">
                <Button type="submit" label="Lưu lại" icon="pi pi-save" @click="saveProject" />
            </div>
        </div>
    </Dialog>
</template>
<script setup>
import Button from 'primevue/button';
import UserTreeSelect from './TreeSelect/UserTreeSelect.vue';
import Dialog from "primevue/dialog";
import { computed, onMounted, ref } from "vue";
import Api from '../api/Api';
import { useProject } from "../stores/project";
import { storeToRefs } from 'pinia';
const storeProject = useProject();
const { projects } = storeToRefs(storeProject);

const emit = defineEmits(["reset"]);
const props = defineProps({
    value: {
        type: Object,
        default: {}
    },
});
const data = ref(props.value);
const visible = ref(false);
const show = (e) => {
    e.preventDefault();
    visible.value = true;
}
const saveProject = (e) => {
    e.preventDefault();
    visible.value = false;
    Api.SaveProject(data.value).then((res) => {
        projects.value.push(res);
        data.value = {};
    });
}
onMounted(() => {
    // console.log(1);
})
</script>