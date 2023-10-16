
<template>
    <Tag :value="status.name" :style="{ background: status.color }">
    </Tag>
</template>
<script setup>

import { storeToRefs } from 'pinia';
import Tag from 'primevue/tag';
import { useProject } from "../../stores/project";
import { computed, onMounted } from 'vue';
const props = defineProps({
    statusId: Number
});
const storeProject = useProject();
const { statusList } = storeToRefs(storeProject);
const status = computed(() => {
    return statusList.value.find((item) => {
        return props.statusId == item.id;
    }) || {}
});

onMounted(() => {
    // console.log(status);
})
</script>