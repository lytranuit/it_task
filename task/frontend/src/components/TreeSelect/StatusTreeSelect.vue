<template>
    <TreeSelect :options="statusList" :multiple="multiple" :normalizer="normalizer" :modelValue="modelValue" :name="name"
        :required="required" :append-to-body="appendToBody" @update:modelValue="emit('update:modelValue', $event)"
        zIndex="3000">
    </TreeSelect>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
import { useProject } from "../../stores/project";

import projectStatusApi from "../../api/projectStatusApi";
const props = defineProps({
    appendToBody: {
        type: Boolean,
        default: true,
    },
    modelValue: {
        type: [Number, Array],
    },
    multiple: {
        type: Boolean,
        default: false,
    },
    required: {
        type: Boolean,
        default: false,
    },
    name: {
        type: String,
        default: "statusList",
    },
    projectId: {
        type: Number,
        required: true
    },
});
const emit = defineEmits(["update:modelValue"]);
const store = useProject();
const { statusList } = storeToRefs(store);
const normalizer = (node) => {
    return {
        id: node.id,
        label: node.name,
    }
}
onMounted(() => {
    projectStatusApi.GetList(props.projectId).then((res) => {
        statusList.value = res;
    })
});
</script>
