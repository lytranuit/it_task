<template>
  <TreeSelect
    :options="roles"
    :multiple="multiple"
    :modelValue="modelValue"
    @update:modelValue="emit('update:modelValue', $event)"
  ></TreeSelect>
</template>

<script setup>
import { useAuth } from "../../stores/auth";
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
const props = defineProps({
  modelValue: {
    type: [String, Array],
  },
  multiple: {
    type: Boolean,
    default: false,
  },
});
const emit = defineEmits(["update:modelValue"]);
const store = useAuth();
const { roles } = storeToRefs(store);
onMounted(() => {
  store.fetchRoles();
});
</script>
