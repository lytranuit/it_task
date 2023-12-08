<template>
  <div class="row justify-content-center">
    <div class="col-md-6">
      <div class="card">
        <div class="card-body">
          <h4 class="header-title mt-0 mb-3 d-flex align-items-center">
            Việc tôi giao
          </h4>
          <Chart type="doughnut" v-bind="chart1" height="400" />
        </div><!--end card-body-->
      </div>

    </div>

    <div class="col-md-6">
      <div class="card">
        <div class="card-body">
          <h4 class="header-title mt-0 mb-3 d-flex align-items-center">
            Việc của tôi
          </h4>
          <Chart type="doughnut" v-bind="chart2" height="400" />
        </div><!--end card-body-->
      </div>
    </div>
    <div class="col-12">
      <TaskDatatable></TaskDatatable>
    </div>

  </div>
  <Loading :waiting="waiting"></Loading>
</template>
<script setup>
import { onMounted, ref } from "vue";
import Chart from "primevue/chart";
import Loading from "../components/loading.vue";
import { labelCenter } from "../service/chartPlugin";
import TaskDatatable from "../components/Datatable/TaskDatatable.vue";
const execution_success = ref(0);
const execution_fail = ref(0);
const execution_amount = ref(0);
const execution_wait = ref(0);
const waiting = ref(false);
const chart1 = ref({
  width: 500,
  plugins: [labelCenter],
  data: {
    labels: ["Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn"],
    datasets: [
      {
        data: [30, 70, 0, 0, 0]
      }
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
      tooltip: {
        enabled: false
      },

      labelCenter: {
        labels: [
          {
            text: "100",
            font: {
              size: 30,
              unit: "em",
              style: "bold"
            }
          },
          {
            text: `công việc`,
            font: {
              size: 25,
              unit: "em",
            }
          }
        ]
      }
    }
  }
})
const chart2 = ref({
  width: 500,
  plugins: [labelCenter],
  data: {
    labels: ["Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn"],
    datasets: [
      {
        data: [30, 70, 0, 0, 0]
      }
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
      tooltip: {
        enabled: false
      },

      labelCenter: {
        labels: [
          {
            text: "100",
            font: {
              size: 30,
              unit: "em",
              style: "bold"
            }
          },
          {
            text: `công việc`,
            font: {
              size: 25,
              unit: "em",
            }
          }
        ]
      }
    }
  }
})
const datatableUser = ref([]);
const datatableProcess = ref([]);
onMounted(() => {

});
</script>
