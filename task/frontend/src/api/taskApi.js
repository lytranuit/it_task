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
  AddComment(chat) {
    return repository
      .post(`/v1/${resoure}/AddComment`, chat, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  morecomment(taskId, from_id) {
    return repository
      .get(`/v1/${resoure}/morecomment`, {
        params: { taskId: taskId, from_id: from_id },
      })
      .then((res) => {  
        return res.data
      });
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
