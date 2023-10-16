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
  const resetTask = () => {
    taskEdit.value = {};
  }
  const resetStatus = () => {
    statusEdit.value = {};
  }
  const taskList = ref([]);
  const ganttData = computed(() => {
    return taskList.value;
  })
  return {
    project,
    projects,
    taskEdit,
    visibleFormTask,
    headerFormTask,
    taskList,
    ganttData,
    statusList,
    statusEdit,
    resetTask,
    resetStatus,
  };
});
