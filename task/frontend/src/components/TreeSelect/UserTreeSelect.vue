<template>
  <TreeSelect :options="users" :multiple="multiple" :normalizer="normalizer" :modelValue="modelValue" :name="name"
    :required="required" :append-to-body="appendToBody" @update:modelValue="emit('update:modelValue', $event)" zIndex="3000">
  </TreeSelect>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
import { useAuth } from "../../stores/auth";
const props = defineProps({
  appendToBody: {
    type: Boolean,
    default: true,
  },
  modelValue: {
    type: [String, Array],
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
    default: "user",
  },
});
const emit = defineEmits(["update:modelValue"]);
const store = useAuth();
const { users } = storeToRefs(store);
const normalizer = (node) => {
  return {
    id: node.id,
    label: node.name,
  }
}
onMounted(() => {

  store.fetchUsers().then(() => {
  });
});
</script>
