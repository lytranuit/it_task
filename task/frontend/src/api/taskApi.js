import repository from "../service/repository";

const resoure = "task";
export default {

  Rank(id, issueId, type) {
    return repository
      .post(`/v1/${resoure}/Rank`, { id: id, issueId: issueId, type: type }, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

  Save(params) {
    return repository
      .post(`/v1/${resoure}/Save`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

  Delete(id) {
    return repository
      .post(`/v1/${resoure}/Delete`, { id: id }, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  GetList(projectId) {
    return repository
      .get(`/v1/${resoure}/GetList`, { params: { projectId: projectId } })
      .then((res) => res.data);
  },
  Get(id) {
    return repository
      .get(`/v1/${resoure}/Get`, { params: { id: id } })
      .then((res) => res.data);
  },
};
