
<template>
    <form id="form-project">
        <div class="row">
            <div class="mb-3 col-12">
                <label for="name" class="form-label">Tên dự án<span class="text-danger">*</span></label>
                <input type="text" class="form-control" id="name" required v-model="data.name">
            </div>
            <div class="mb-3 col-12">
                <label for="description" class="form-label">Mô tả</label>
                <textarea class="form-control" id="description" rows="4" v-model="data.description"></textarea>
            </div>
            <div class="mb-3 col-12">
                <label for="assignee" class="form-label">Người tham gia</label>
                <UserTreeSelect name="createuser" :multiple="true" :required="true" v-model="data.list_assignee"
                    :append-to-body="false">
                </UserTreeSelect>
            </div>
            <div class="col-12 text-center">
                <Button type="submit" label="Lưu lại" icon="pi pi-save" size="small" @click="saveProject" />
            </div>
        </div>
    </form>
</template>
<script setup>
import Button from 'primevue/button';
import UserTreeSelect from '../TreeSelect/UserTreeSelect.vue';
import { computed, onMounted, ref } from "vue";
import projectApi from '../../api/projectApi';
import { useProject } from "../../stores/project";
import { storeToRefs } from 'pinia';
const storeProject = useProject();
const { projects } = storeToRefs(storeProject);

const emit = defineEmits(["reset", "beforeSave", "save"]);
const props = defineProps({
    value: {
        type: Object,
        default: {}
    },
});
const data = computed(() => props.value);
const saveProject = (e) => {
    e.preventDefault();
    if (!valid())
        return;
    emit("beforeSave");
    projectApi.Save(data.value).then((res) => {
        var find = projects.value.findIndex((item) => {
            return item.id == res.id;
        });
        console.log(find);
        if (find != -1) {
            projects.value[find] = res;
        } else {
            projects.value.push(res);
        }

        emit("save");
    });
}
const valid = () => {
    if (!$("#form-project").valid()) {
        return false;
    }
    return true;
}
onMounted(() => {
    // console.log("mount");
    // console.log(data.value);
})

</script>