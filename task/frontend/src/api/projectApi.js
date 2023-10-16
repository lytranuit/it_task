import repository from "../service/repository";

const resoure = "project";
export default {

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
  GetList() {
    return repository
      .get(`/v1/${resoure}/GetList`)
      .then((res) => res.data);
  },
  Get(id) {
    return repository
      .get(`/v1/${resoure}/Get`, { params: { id: id } })
      .then((res) => res.data);
  }
};
