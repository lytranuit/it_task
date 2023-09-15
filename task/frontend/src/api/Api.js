import repository from "../service/repository";

const resoure = "api";
export default {

  SaveProject(params) {
    return repository
      .post(`/v1/${resoure}/SaveProject`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

  GetListProject() {
    return repository
      .get(`/v1/${resoure}/GetListProject`)
      .then((res) => res.data);
  },
  GetProject(projectId) {
    return repository
      .get(`/v1/${resoure}/GetProject`, { params: { projectId: projectId } })
      .then((res) => res.data);
  },
};
