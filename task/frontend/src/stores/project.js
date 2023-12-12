import { ref, computed } from "vue";
import { defineStore } from "pinia";

export const useProject = defineStore("project", () => {
  const projects = ref([]);

  const project = ref({});
  const taskEdit = ref({});
  const statusEdit = ref({});
  const statusList = ref([]);
  const visibleFormTask = ref(false);
  const headerFormTask = ref("Công việc mới");
  const visibleSidebar = ref(false);
  const width = ref(700);
  const resetTask = () => {
    taskEdit.value = {};
  }
  const resetStatus = () => {
    statusEdit.value = {};
  }
  const taskList = ref([]);
  // const ganttData = computed(() => {
  //   return taskList.value;
  // })
  const key = ref("xcvsdf");
  return {
    project,
    projects,
    taskEdit,
    visibleFormTask,
    headerFormTask,
    taskList,
    visibleSidebar,
    width,
    // ganttData,
    statusList,
    statusEdit,
    key,
    resetTask,
    resetStatus,
  };
});
