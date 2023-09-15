import { ref, computed } from "vue";
import { defineStore } from "pinia";

export const useProject = defineStore("project", () => {
  const projects = ref([]);

  return {
    projects,
  };
});
