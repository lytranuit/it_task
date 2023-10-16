
<template>
    <span class="float-right" @click.stop="show($event)"><i class="fas fa-plus-circle"></i></span>
    <Dialog v-model:visible="visible" modal header="Dự án mới" :style="{ width: '50vw' }">
        <Form @beforeSave="hide" :value="data"></Form>
    </Dialog>
</template>
<script setup>
import Dialog from "primevue/dialog";
import { onMounted, ref } from "vue";
import Form from './Project/Form.vue';

const emit = defineEmits(["reset"]);
const visible = ref(false);
const data = ref({});
const show = (e) => {
    e.preventDefault();
    visible.value = true;
}
const hide = (e) => {
    visible.value = false;
    data.value = {};
}
onMounted(() => {
    // console.log(1);
})
</script>