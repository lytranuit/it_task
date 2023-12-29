<template>
  <div class="row justify-content-center">
    <div class="col-md-4">
      <div class="card">
        <div class="card-body">
          <h4 class="header-title mt-0 mb-3 d-flex align-items-center">
            Việc tôi giao
          </h4>
          <Chart type="doughnut" v-bind="chart1" />
        </div><!--end card-body-->
      </div>

    </div>

    <div class="col-md-4">
      <div class="card">
        <div class="card-body">
          <h4 class="header-title mt-0 mb-3 d-flex align-items-center">
            Việc của tôi
          </h4>
          <Chart type="doughnut" v-bind="chart2" />
        </div><!--end card-body-->
      </div>
    </div>
    <div class="col-md-4">
      <div class="card">
        <div class="card-body">
          <h4 class="header-title mt-0 mb-3 d-flex align-items-center">
            Điểm số
          </h4>
          <div class="text-center" style="
    padding: 50px 0px; font-size:20px;
"><span style="font-size: 30px; color: green; font-weight: bold;">{{ point }}</span> / {{ task }} công việc</div>
        </div><!--end card-body-->
      </div>
    </div>
    <div class="col-12">

      <DashboardGantt></DashboardGantt>
    </div>

  </div>
  <Loading :waiting="waiting"></Loading>
</template>
<script setup>
import { onMounted, ref } from "vue";
import Chart from "primevue/chart";
import Loading from "../components/loading.vue";
import { labelCenter } from "../service/chartPlugin";
import DashboardGantt from "../components/Gantt/DashboardGantt.vue";
import Api from "../api/Api";
import Button from "primevue/button";
const point = ref(0);
const task = ref(0);
const waiting = ref(false);
const chart1 = ref({
  width: 400,
  plugins: [labelCenter],
  data: {
    labels: ["Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn"],
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
const chart2 = ref({
  width: 400,
  plugins: [labelCenter],
  data: {
    labels: ["Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn"],
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

const assignTask = () => {
  Api.assignTask().then((res) => {
    chart1.value.data = res.data;
    chart1.value.options.plugins.labelCenter.labels[0].text = res.count;
  });

}

const myTask = () => {
  Api.myTask().then((res) => {
    chart2.value.data = res.data;
    chart2.value.options.plugins.labelCenter.labels[0].text = res.count;
  });

}


const myPoint = () => {
  Api.myPoint().then((res) => {
    point.value = res.point;
    task.value = res.task;
  });

}
onMounted(() => {
  assignTask();
  myTask();
  myPoint();
});
</script>
