<template>
  <div class="row justify-content-center mb-3">
    <div class="col-lg-3 align-self-center">
      <h2 class="mb-0">BÁO CÁO</h2>
    </div>
    <div class="col-lg-9">
      <div class="row">
        <div class="col-lg-8">
          <b>Nhân viên</b>
          <UserManagerTreeSelect :multiple="true" v-model:modelValue="nhanviens" @update:modelValue="refresh">
          </UserManagerTreeSelect>
        </div>
        <div class="col-lg-4">
          <b>Thời gian</b>
          <div>
            <div class="d-flex align-items-center" style="gap:10px" v-if="view == 'nam'">
              <select class="form-control" style="width:150px" v-model="nam" @change="changeNam">
                <option v-for="item of list_nam" :value="item" :key="item">{{ item }}</option>
              </select>
              <Button label="Từ ngày đến ngày" @click="view = 'tungaydenngay'" :outlined="true" size="small"></button>
            </div>
            <div class="d-flex align-items-center" style="gap:10px" v-if="view == 'tungaydenngay'">
              <Calendar v-model="dates" selectionMode="range" class="p-inputtext-sm w-100" :manualInput="true"
                dateFormat="dd/mm/yy" @hide="refresh" :maxDate="maxDate" />
              <Button label="Năm" class="" @click="view = 'nam'" :outlined="true" size="small"></button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col-xl-12">
      <draggable v-model="listsort" handle=".handle" :group="{ name: 'people', pull: 'clone', put: false }"
        ghost-class="ghost" item-key="id" class="row" @end="onEnd">
        <template #item="{ element }">

          <template v-if="element.name == 'topdiemnv'">

            <div class="col-xl-4">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex mt-0 mb-3">
                    <div class="header-title d-flex align-items-center">Điểm số nhân viên</div>
                    <div class="ml-auto">
                      <div class="handle"><i class="fas fa-arrows-alt"></i></div>
                    </div>
                  </div>
                  <div class="">
                    <Chart v-bind="chart1" />
                  </div>
                  <!--end /div-->
                </div>
                <!--end card-body-->
              </div>
              <!--end card-->
            </div>

          </template>
          <template v-else-if="element.name == 'congviectg'">

            <div class="col-xl-8">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex mt-0 mb-3">
                    <div class="header-title d-flex align-items-center">Số lượng công việc theo thời gian</div>
                    <div class="ml-auto">
                      <div class="handle"><i class="fas fa-arrows-alt"></i></div>
                    </div>
                  </div>
                  <div class="">
                    <Chart v-bind="chart2" />
                  </div>
                  <!--end /div-->
                </div>
                <!--end card-body-->
              </div>
              <!--end card-->
            </div>

          </template>

          <template v-else-if="element.name == 'tinhtrangcongviec'">

            <div class="col-xl-6">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex mt-0 mb-3">
                    <div class="header-title d-flex align-items-center">Tình trạng công việc</div>
                    <div class="ml-auto">
                      <div class="handle"><i class="fas fa-arrows-alt"></i></div>
                    </div>
                  </div>
                  <div class="">
                    <Chart v-bind="chart3" />
                  </div>
                  <!--end /div-->
                </div>
                <!--end card-body-->
              </div>
              <!--end card-->
            </div>

          </template>
          <template v-else-if="element.name == 'congviecthuchien'">

            <div class="col-xl-6">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex mt-0 mb-3">
                    <div class="header-title d-flex align-items-center">Công việc đang thực hiện</div>
                    <div class="ml-auto">
                      <div class="handle"><i class="fas fa-arrows-alt"></i></div>
                    </div>
                  </div>
                  <div class="">
                    <Chart v-bind="chart4" />
                  </div>
                  <!--end /div-->
                </div>
                <!--end card-body-->
              </div>
              <!--end card-->
            </div>

          </template>
        </template>
      </draggable>
    </div>
  </div>

  <Loading :waiting="waiting"></Loading>
</template>
<script setup>
import draggable from 'vuedraggable'
import Calendar from 'primevue/calendar';
import UserManagerTreeSelect from '../../components/TreeSelect/UserManagerTreeSelect.vue';
import Button from 'primevue/button';
import { onMounted, ref } from 'vue';
import Loading from '../../components/loading.vue';
import Api from '../../api/Api';
import Chart from 'primevue/chart';
import ChartDataLabels from 'chartjs-plugin-datalabels';

const waiting = ref(false);
const nhanviens = ref();
const currentDate = new Date();
const dates = ref([new Date(currentDate.getFullYear(), 0, 1), currentDate]);
const nam = ref(currentDate.getFullYear());
const list_nam = ref([2020, 2021, 2022, 2023, 2024]);
// const nam = ref(2022);
// const dates = ref([new Date(2022, 0, 1), new Date(2022, 11, 31)]);

const listsort = ref([{
  name: "congviectg"
}, {
  name: "topdiemnv"
}, {
  name: "tinhtrangcongviec"
}, {
  name: "congviecthuchien"
}]);

const chart1 = ref({
  type: "bar",
  options: {
    indexAxis: 'y',
    responsive: true,
    plugins: {
      datalabels: {
        color: 'white'
      },
      legend: {
        display: false,
      },
    },
  },
  plugins: [ChartDataLabels],
  height: 300
});

const chart2 = ref({
  type: "line",
  options: {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: "bottom"
      },
    },
  }
});

const chart3 = ref({
  type: "bar",
  options: {
    indexAxis: 'y',
    responsive: true,
    plugins: {
      datalabels: {
        color: 'white'
      },
      legend: {
        display: true,
        position: 'bottom'
      },
    },
    scales: {
      x: {
        stacked: true,
      },
      y: {
        stacked: true,
      }
    }
  },
  plugins: [ChartDataLabels],
});

const chart4 = ref({
  type: "bar",
  options: {
    indexAxis: 'y',
    responsive: true,
    plugins: {
      datalabels: {
        color: 'white'
      },
      legend: {
        display: false,
      },
    }
  },
  plugins: [ChartDataLabels],
});

const view = ref("nam");
const maxDate = ref(new Date());
const changeNam = () => {
  dates.value = [new Date(nam.value, 0, 1), new Date(nam.value, 11, 31)]
  refresh();
}
const load_topdiemnv = () => {
  Api.topdiemnv(dates.value[0], dates.value[1], nhanviens.value).then((res) => {
    chart1.value.data = res.data;
  });
}
const load_congviectg = () => {
  Api.congviectg(dates.value[0], dates.value[1], nhanviens.value).then((res) => {
    chart2.value.data = res.data;
  });
}
const load_tinhtrangcongviec = () => {
  Api.tinhtrangcongviec(dates.value[0], dates.value[1], nhanviens.value).then((res) => {
    chart3.value.data = res.data;
  });
}
const load_congviecthuchien = () => {
  Api.congviecthuchien(nhanviens.value).then((res) => {
    chart4.value.data = res.data;
  });
}
const refresh = () => {
  load_topdiemnv();
  load_congviectg();
  load_tinhtrangcongviec();
  load_congviecthuchien();
}

const onEnd = (e) => {
  // console.log(e);
  localStorage.setItem("sort", JSON.stringify(listsort.value));
}
onMounted(() => {
  const cacheSort = localStorage.getItem("sort") || "[]";
  let data = JSON.parse(cacheSort);
  if (data.length > 0) {
    listsort.value = data;
  }
  refresh();
});
</script>
