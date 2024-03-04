<template>
    <div class="row justify-content-center">
        <div class="col-md-12">
            <div class="text-right"> <b>Deadline:</b> {{ formatDate(endDate) }}</div>
            <MeterGroup :value="value" />
        </div>
    </div>
</template>
<script setup>
import MeterGroup from 'primevue/metergroup';
import { onMounted, ref } from "vue";
import Api from "../../api/Api";
import { useRoute } from "vue-router";
import { formatDate } from '../../utilities/util';
const route = useRoute();
const endDate = ref();
const value = ref([
    { label: 'Apps', color: '#34d399', value: 16 },
    { label: 'Messages', color: '#fbbf24', value: 8 },
    { label: 'Media', color: '#60a5fa', value: 24 },
    { label: 'System', color: '#c084fc', value: 10 }
]);
const summary1Project = () => {
    Api.summary1Project(route.params.id).then((res) => {
        value.value = res.data;
        endDate.value = res.endDate;
    });
}
onMounted(() => {
    summary1Project();
});
</script>