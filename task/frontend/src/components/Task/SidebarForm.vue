
<template>
    <Sidebar v-model:visible="visibleSidebar" position="right" :style="'width:' + width + 'px;'">
        <template #container="{ closeCallback }">
            <div id="splitbarTaskContent" class="ng-tns-c404-37" :draggable="true" @drag="drag">
                <div class="resize-task-detail hidden-sidebar dis-flex justify-content-center align-items-center pointer wrap-carret-left"
                    @click="fullscreen">
                    <div class="pi pi-angle-left text-white"></div>
                </div>
            </div>
            <div class="p-sidebar-header" data-pc-section="header">
                <div class="d-flex w-100">
                    <div v-if="checkPermissionPoint(taskEdit.list_assignee)">
                        <span class="p-input-icon-left">
                            <i class="pi pi-box" />
                            <InputText v-model="taskEdit.point" placeholder="Chấm điểm" @change="save"
                                class="input_point" />
                        </span>
                    </div>
                    <div class="ml-auto">
                        <span style="font-size: 20px; cursor: pointer;" class="mr-2"
                            :class="{ 'text-warning': taskEdit.favorites != null && taskEdit.favorites.length }"
                            @click="toggle_favorite">
                            <i class="fas fa-star"></i>
                        </span>
                        <Button size="small" raised :outlined="taskEdit.progress < 100" @click="toggle"
                            :class="{ 'p-button-success': taskEdit.progress == 100 }">
                            <span class="">Hoàn thành | </span>
                            <span class="px-1">{{ taskEdit.progress }}%</span>
                            <i class="pi pi-chevron-down"></i>
                        </Button>
                        <OverlayPanel ref="op">
                            <div style="width: 200px;">
                                <div>
                                    <div>Tiến độ công việc</div>
                                    <Slider v-model="taskEdit.progress" class="mt-3" @slideend="save" />
                                </div>
                                <div class="mt-3 text-center">
                                    <InputText v-model.number="taskEdit.progress" style="width:50px;" @change="save" />
                                </div>
                            </div>
                        </OverlayPanel>
                        <Button type="button" @click="closeCallback" icon="pi pi-times" size="small" class="ml-2"
                            :outlined="true"></Button>
                    </div>
                </div>
            </div>
            <div class="p-sidebar-content" data-pc-section="content">
                <div class="mb-3 row">
                    <div class="col-md-9">
                        <label for="name" class="form-label">Tên công việc <span class="text-danger">*</span></label>
                        <input type="text" class="form-control" id="name" v-model="taskEdit.name" @change="save"
                            :readonly="!checkPermission(taskEdit)">
                    </div>
                    <div class="col-md-3">
                        <label for="priority" class="form-label">Độ ưu tiên</label>
                        <select class="form-control" v-model="taskEdit.priority" @change="save"
                            :readonly="!checkPermission(taskEdit)">
                            <option value="1">Bình thường</option>
                            <option value="2">Cao</option>
                            <option value="3">Ưu tiên</option>
                        </select>
                    </div>
                </div>

                <div class="mb-3 row" v-if="checkPermission(taskEdit)">
                    <div class="col-12">
                        <label for="assignee" class="form-label">Người thực hiện</label>
                        <UserTreeSelect name="createuser" :multiple="true" :required="true" v-model="taskEdit.list_assignee"
                            @update:modelValue="save">
                        </UserTreeSelect>
                    </div>
                </div>
                <!-- <div class="mb-3 row">
                    <div class="col-12">
                        <label for="attachments" class="form-label">Tập tin đính kèm</label>
                        
                    </div>
                </div> -->

                <div class="mb-3 row">
                    <div class="col-12">
                        <label for="description" class="form-label">Mô tả</label>
                        <textarea class="form-control" id="description" rows="4" v-model="taskEdit.description"
                            @change="save" :readonly="!checkPermission(taskEdit)"></textarea>
                    </div>
                </div>
                <div class="mb-3 row">
                    <div class="col-12">
                        <a href="#" @click="is_hanhoanthanh = !is_hanhoanthanh" style="color: #039be5;"><i
                                class="far fa-calendar-alt"></i> Hạn hoàn
                            thành</a>
                    </div>
                </div>
                <div class="mb-3 row" v-show="is_hanhoanthanh">
                    <div class="col-md-6">
                        <label for="startDate" class="form-label">Ngày bắt đầu</label>
                        <Calendar v-model="taskEdit.startDate" dateFormat="yy-mm-dd" class="date-custom" showIcon
                            showButtonBar @hide="save" :readonly="!checkPermission(taskEdit)" :show-time="true"
                            :stepMinute="60" :stepSecond="60" />
                    </div>
                    <div class="col-md-6">
                        <label for="endDate" class="form-label">Ngày kết thúc</label>
                        <Calendar v-model="taskEdit.endDate" dateFormat="yy-mm-dd" class="date-custom" showIcon
                            showButtonBar @hide="save" :readonly="!checkPermission(taskEdit)" :show-time="true"
                            :stepMinute="60" :stepSecond="60" />
                    </div>
                </div>

                <div class="mb-3 row">
                    <div class="col-12">
                        <a href="#" @click="is_kehoach = !is_kehoach" style="color: #039be5;"><i
                                class="far fa-calendar"></i> Kế hoạch</a>
                    </div>

                </div>
                <div class="mb-3 row" v-show="is_kehoach">
                    <div class="col-md-6">
                        <label for="baselineStartDate" class="form-label">Kế hoạch bắt đầu</label>
                        <Calendar v-model="taskEdit.baselineStartDate" dateFormat="yy-mm-dd" class="date-custom" showIcon
                            showButtonBar @hide="save" :readonly="!checkPermission(taskEdit)" :show-time="true"
                            :stepMinute="60" :stepSecond="60" />
                    </div>
                    <div class="col-md-6">
                        <label for="baselineEndDate" class="form-label">Kế hoạch kết thúc</label>
                        <Calendar v-model="taskEdit.baselineEndDate" dateFormat="yy-mm-dd" class="date-custom" showIcon
                            showButtonBar @hide="save" :readonly="!checkPermission(taskEdit)" :show-time="true"
                            :stepMinute="60" :stepSecond="60" />
                    </div>
                </div>

                <div class="mb-3 row">
                    <div class="col-12">
                        <TabView>
                            <TabPanel>
                                <template #header>
                                    <div class="d-flex align-items-center" style="gap: 5px;">
                                        <span class="pi pi-comments"></span>
                                        <span class="font-bold white-space-nowrap">Bình luận</span>
                                    </div>
                                </template>
                                <InputGroup>
                                    <input type="file" class="d-none" name='file[]' multiple @change="AddComment" />
                                    <Textarea placeholder="Thêm bình luận ở đây" v-model="comment" rows="1" />
                                    <Button label="" icon="pi pi-file" size="small" severity="success"
                                        @click="AddCommentFile"></Button>

                                    <Button label="Gửi" icon="pi pi-send" size="small" @click="AddComment"></Button>
                                </InputGroup>
                                <div class="mt-3">
                                    <ul class="list-unstyled" id="comment_box">
                                        <li class="media comment_box my-2" :data-read="comment.is_read"
                                            v-for="(comment, index) in comments">
                                            <img class="mr-3 rounded-circle" :src="comment.user.image_url" width="50"
                                                alt="" />
                                            <div class="media-body border-bottom" style="display: grid">
                                                <h6 class="mt-0 mb-1" style="font-size: 14px;">
                                                    {{ comment.user.fullName }}
                                                    <small class="text-muted">
                                                        -
                                                        {{ formatDate(comment.created_at, "HH:mm DD/MM/YYYY") }}</small>
                                                </h6>
                                                <div class="mb-2" style="white-space: pre-wrap">
                                                    {{ comment.comment }}
                                                </div>
                                                <div class="mb-2 attach_file file-box-content">
                                                    <div class="file-box" v-for="(file, index1) in comment.files">
                                                        <a :href="file.url" :download="file.name"
                                                            class="download-icon-link">
                                                            <i class="dripicons-download file-download-icon"></i>
                                                        </a>
                                                        <div class="text-center">
                                                            <i class="far fa-file text-danger"></i>
                                                            <h6 class="text-truncate" :title="file.name">
                                                                {{ file.name }}
                                                            </h6>
                                                            <small class="text-muted">{{ file.ext }}</small>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                    <div class="text-center load_more" @click="getComments()" v-if="show_more == true">
                                        <button class="btn btn-primary btn-sm px-5" style="cursor: pointer;">Xem thêm bình
                                            luận</button>
                                    </div>
                                </div>
                            </TabPanel>
                            <TabPanel header="">
                                <template #header>
                                    <div class="d-flex align-items-center" style="gap: 5px;">
                                        <span class="pi pi-forward "></span>
                                        <span class="font-bold white-space-nowrap">Hoạt động</span>
                                    </div>
                                </template>
                                <div class="activity" id="event">
                                    <div v-for="(event, index) in events">
                                        <i class="mdi" :class="{
                                            'mdi-checkbox-marked-circle-outline icon-success':
                                                event.type != 2,
                                            'mdi-close-circle icon-danger': event.type == 2,
                                        }"></i>
                                        <div class="time-item">
                                            <div class="item-info">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <span class="">{{
                                                        formatDate(event.created_at, "HH:mm DD/MM/YYYY")
                                                    }}</span>
                                                </div>
                                                <h5 v-html="event.event_content" style="font-size: 13px;"></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </TabPanel>

                        </TabView>
                    </div>
                </div>
            </div>
        </template>
        <!-- <template #header>
            asdxcvsd
            fsd
        </template> -->

    </Sidebar>
</template>
<script setup>
import InputGroup from 'primevue/inputgroup';
import Textarea from 'primevue/textarea';
import FileUpload from 'primevue/fileupload';
import Slider from 'primevue/slider';
import InputText from 'primevue/inputtext';
import OverlayPanel from 'primevue/overlaypanel';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import Button from 'primevue/button';
import Calendar from 'primevue/calendar';
import Sidebar from 'primevue/sidebar';
import UserTreeSelect from '../TreeSelect/UserTreeSelect.vue';
import { computed, onMounted, ref, watch } from "vue";
import taskApi from '../../api/TaskApi';
import { useProject } from "../../stores/project";
import { storeToRefs } from 'pinia';
import { kanbanData } from '../../datasource';
import { rand } from '../../utilities/rand';
import { formatDate } from '../../utilities/util'

import ConfirmDialog from 'primevue/confirmdialog';
import { useConfirm } from "primevue/useconfirm";
import { useAuth } from '../../stores/auth';

const storeAuth = useAuth();
const { is_admin, user, is_manager, list_users } = storeToRefs(storeAuth);
const confirm = useConfirm();
const comments = ref([]);
const comment = ref();
const show_more = ref(true);
const storeProject = useProject();
const { taskEdit, taskList, statusList, visibleSidebar, width, key } = storeToRefs(storeProject);

const emit = defineEmits(["reset", "beforeSave", "save"]);
const is_kehoach = ref(false);
const is_hanhoanthanh = ref(false);
const events = ref([]);
const save = () => {
    if (!valid())
        return;
    emit("beforeSave");
    taskApi.Save(taskEdit.value).then((res) => {
        if (taskEdit.value.id > 0) {
            var find = taskList.value.findIndex((item) => {
                return item.id == taskEdit.value.id;
            });
            // console.log(find)
            taskList.value[find] = res;
            // console.log(taskList);
        } else {
            taskList.value.push(res);
        }
        key.value = rand();
        // taskEdit.value = {};
        emit("save");
    });
}
const toggle_favorite = () => {

    let type = "add";
    if (taskEdit.value.favorites.length)
        type = "delete";

    taskApi.Favorite({ type: type, taskId: taskEdit.value.id }).then((res) => {
        var find = taskList.value.findIndex((item) => {
            return item.id == taskEdit.value.id;
        });
        taskList.value[find] = res.data;
        taskEdit.value = res.data;
    })
}
const AddCommentFile = () => {
    $("[name='file[]']").click();
}
const AddComment = () => {

    var files = $("[name='file[]']")[0].files;
    if ((comment.value == null || comment.value.trim() == "") && !files.length) {
        alert("Chưa nhập nội dung bình luận!")
        return false;
    }
    var formData = new FormData();
    formData.append('taskId', taskEdit.value.id);
    if (!(comment.value == null || comment.value.trim() == "")) {
        formData.append('comment', comment.value);
    }
    for (var file of files) {
        formData.append("file", file);
    }
    comment.value = null;
    taskApi.AddComment(formData).then((res) => {
        var comment = res.comment;
        comments.value.unshift(comment);
    });
    return true;
}
const checkPermission = (task) => {
    if (is_admin.value)
        return true;
    if (task.created_by == user.value.id) {
        return true;
    }
    return false;
}
const op = ref();
const toggle = (event) => {
    op.value.toggle(event);
}
const drag = (e) => {
    console.log(e);
    if (e.screenX == 0)
        return;
    width.value = screen.width - e.screenX;
    // $(".p-sidebar").css('width', width + "px");
}
const fullscreen = () => {
    width.value = screen.width;
}
const deleteTask = (e) => {
    e.preventDefault();
    confirm.require({
        message: 'Bạn có muốn xóa công việc này?',
        header: 'Xóa công việc',
        accept: () => {
            emit("beforeSave");
            taskApi.Delete(taskEdit.value.id).then((res) => {

                var find = taskList.value.findIndex((item) => {
                    return item.id == taskEdit.value.id;
                });
                taskList.value.splice(find, 1);
                taskEdit.value = {};
                emit("save");
            });
        },
        acceptLabel: "Xác nhận",
        acceptIcon: "pi pi-trash",
        acceptClass: "p-button-danger",
        rejectLabel: "Hủy",
    });

}
const valid = () => {
    if (!taskEdit.value.name) {
        return false;
    }
    return true;
}
const getComments = async () => {
    /// Lấy comments
    var task_id = taskEdit.value.id;
    var from_id;
    if (comments.value.length > 0) {
        from_id = comments.value[comments.value.length - 1].id;
    }
    var ress = await taskApi.morecomment(task_id, from_id);
    var list_comments = ress.comments;
    if (list_comments.length == 10) {
        list_comments.pop();
        show_more.value = true;
    } else {
        show_more.value = false;
    }
    comments.value = comments.value.concat(list_comments);
}

const getEvents = async () => {
    /// Lấy comments
    var task_id = taskEdit.value.id;
    var ress = await taskApi.getEvents(task_id);
    events.value = ress.events
}
const getTask = async () => {
    /// Lấy comments
    var task_id = taskEdit.value.id;
    var ress = await taskApi.Get(task_id);
    taskEdit.value = ress;
}
const checkPermissionPoint = (list_assignee) => {
    if (is_admin.value)
        return true;
    if (!is_manager.value)
        return false;
    if (sameMembers(list_assignee, list_users.value))
        return true;
    return false;

}
const containsAll = (arr1, arr2) =>
    arr2.every(arr2Item => arr1.includes(arr2Item))
const sameMembers = (arr1, arr2) =>
    containsAll(arr1, arr2) && containsAll(arr2, arr1);
onMounted(() => {
    getComments();
    getEvents();
    getTask();
})

</script>

<style scoped>
.input_point {
    width: 130px;
}

@media (max-width: 525px) {
    .input_point {
        width: 80px;
    }
}

#splitbarTaskContent {
    left: -2px;
    position: absolute;
    top: 0;
    height: calc(100%);
    width: 4px;
    z-index: 4;
    cursor: col-resize;
}

#splitbarTaskContent:hover {
    border-left: 2px solid #5db7ff;
}

#splitbarTaskContent .resize-task-detail {
    background-color: #2196f3;
    position: absolute;
    top: 50%;
    left: -8px;
    height: 24px;
    border-radius: 4px;
    width: 18px;
}
</style>