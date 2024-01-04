import repository from "../service/repository";

const resoure = "api";
export default {

  SaveTask(params) {
    return repository
      .post(`/v1/${resoure}/SaveTask`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

  DeleteTask(taskId) {
    return repository
      .post(`/v1/${resoure}/DeleteTask`, { taskId: taskId }, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  SaveProject(params) {
    return repository
      .post(`/v1/${resoure}/SaveProject`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

  DeleteProject(projectId) {
    return repository
      .post(`/v1/${resoure}/DeleteProject`, { projectId: projectId }, {
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
  assignTask() {
    return repository
      .get(`/v1/${resoure}/assignTask`)
      .then((res) => res.data);
  },
  myTask() {
    return repository
      .get(`/v1/${resoure}/myTask`)
      .then((res) => res.data);
  },
  myPoint() {
    return repository
      .get(`/v1/${resoure}/myPoint`)
      .then((res) => res.data);
  },

  topdiemnv(tungay, denngay, nhanviens) {
    return repository.post(`/v1/${resoure}/topdiemnv`,
      {
        tungay: tungay,
        denngay: denngay,
        nhanviens: nhanviens
      }, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
    ).then((res) => res.data);
  },
  congviectg(tungay, denngay, nhanviens) {
    return repository.post(`/v1/${resoure}/congviectg`,
      {
        tungay: tungay,
        denngay: denngay,
        nhanviens: nhanviens
      }, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
    ).then((res) => res.data);
  },
  tinhtrangcongviec(tungay, denngay, nhanviens) {
    return repository.post(`/v1/${resoure}/tinhtrangcongviec`,
      {
        tungay: tungay,
        denngay: denngay,
        nhanviens: nhanviens
      }, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
    ).then((res) => res.data);
  },
  congviecthuchien(nhanviens) {
    return repository.post(`/v1/${resoure}/congviecthuchien`,
      {
        nhanviens: nhanviens
      }, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
    ).then((res) => res.data);
  },

  departments() {
    return repository.get(`/v1/${resoure}/departments`).then((res) => res.data);
  },
};
