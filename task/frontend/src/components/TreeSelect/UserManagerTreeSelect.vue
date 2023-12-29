<template>
  <TreeSelect :options="options" :multiple="multiple" :modelValue="modelValue" :name="name" :required="required"
    :append-to-body="appendToBody" @update:modelValue="update" :flat="flat">
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
  flat: {
    type: Boolean,
    default: true,
  },
});
const emit = defineEmits(["update:modelValue"]);
const store = useAuth();
const { list_users } = storeToRefs(store);
const options = computed(() => {
  let data = list_users.value.map((node) => {
    return {
      id: node.userManager.id,
      label: node.userManager.fullName + "<" + node.userManager.email + ">",
    }
  });
  // console.log(data);
  data.unshift({
    id: "all",
    label: 'Select All',
  });
  return data;
})
const update = (value) => {
  // console.log(value);
  // check 'all' value is available in user id's array
  if (value.includes('all')) {
    value = [];
    options.value.map(function (item) {
      if (item.id != 'all') {
        // push each item id in user id's array
        value.push(item.id);
      }
    });
  }
  emit('update:modelValue', value)
}
onMounted(() => {
  console.log(list_users.value);
});
</script>
