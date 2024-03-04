<template>
    <div class="row justify-content-center">
        <div class="col-md-12">
            <Chart type="doughnut" v-bind="chart1" />
        </div>
    </div>
    <Loading :waiting="waiting"></Loading>
</template>
<script setup>
import { onMounted, ref } from "vue";
import Chart from "primevue/chart";
import Loading from "../Loading.vue";
import { labelCenter } from "../../service/chartPlugin"
import Api from "../../api/Api";
import { useRoute } from "vue-router";

const route = useRoute();
const waiting = ref(false);
const chart1 = ref({
    width: 400,
    plugins: [labelCenter],
    data: {
        labels: ["Hoàn thành", "Đang thực hiện", "Chưa thực hiện", "Quá hạn"],
        datasets: [
        ],
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: true,
                position: 'right'
            },

            labelCenter: {
                labels: [
                    {
                        text: 0,
                        font: {
                            size: 20,
                            unit: "em",
                            style: "bold"
                        }
                    },
                    {
                        text: `công việc`,
                        font: {
                            size: 18,
                            unit: "em",
                        }
                    }
                ]
            }
        }
    }
})

const summaryProject = () => {
    Api.summaryProject(route.params.id).then((res) => {
        chart1.value.data = res.data;
        chart1.value.options.plugins.labelCenter.labels[0].text = res.count;
    });

}
onMounted(() => {
    summaryProject();
});
</script>