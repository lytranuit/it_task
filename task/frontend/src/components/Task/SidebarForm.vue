
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
                <span class="p-buttonset">
                    <Button size="small" raised :outlined="taskEdit.progress < 100" @click="toggle"
                        :class="{ 'p-button-success': taskEdit.progress == 100 }">
                        <span class="">Hoàn thành | </span>
                        <span class="px-1">{{ taskEdit.progress }}%</span>
                        <i class="pi pi-chevron-down"></i>
                    </Button>
                </span>
                <OverlayPanel ref="op">
                    <div style="width: 200px;">
                        <div>
                            <div>Tiến độ công việc</div>
                            <Slider v-model="taskEdit.progress" class="mt-3" />
                        </div>
                        <div class="mt-3 text-center">
                            <InputText v-model.number="taskEdit.progress" style="width:50px;" />
                        </div>
                    </div>
                </OverlayPanel>
            </div>
            <div class="p-sidebar-content" data-pc-section="content">
                <div class="mb-3 row">
                    <div class="col-9">
                        <label for="name" class="form-label">Tên công việc <span class="text-danger">*</span></label>
                        <input type="text" class="form-control" id="name" required v-model="taskEdit.name">
                    </div>
                    <div class="col-3">
                        <label for="priority" class="form-label">Độ ưu tiên</label>
                        <select class="form-control" v-model="taskEdit.priority">
                            <option value="1">Bình thường</option>
                            <option value="2">Cao</option>
                            <option value="3">Ưu tiên</option>
                        </select>
                    </div>
                </div>

                <div class="mb-3 row">
                    <div class="col-12">
                        <label for="assignee" class="form-label">Người thực hiện</label>
                        <UserTreeSelect name="createuser" :multiple="true" :required="true"
                            v-model="taskEdit.list_assignee">
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
                        <a href="#" @click="is_hanhoanthanh = !is_hanhoanthanh" style="color: #039be5;"><i
                                class="far fa-calendar-alt"></i> Hạn hoàn
                            thành</a>
                    </div>
                </div>
                <div class="mb-3 row" v-show="is_hanhoanthanh">
                    <div class="col-md-6">
                        <label for="startDate" class="form-label">Ngày bắt đầu</label>
                        <Calendar v-model="taskEdit.startDate" dateFormat="yy-mm-dd" class="date-custom"
                            :manualInput="false" showIcon showButtonBar />
                    </div>
                    <div class="col-md-6">
                        <label for="endDate" class="form-label">Ngày kết thúc</label>
                        <Calendar v-model="taskEdit.endDate" dateFormat="yy-mm-dd" class="date-custom" :manualInput="false"
                            showIcon showButtonBar />
                    </div>
                </div>

                <div class="mb-3 row">
                    <div class="col-12">
                        <a href="#" @click="is_kehoach = !is_kehoach" style="color: #039be5;"><i
                                class="far fa-calendar"></i> Kế
                            hoạch</a>
                    </div>

                </div>
                <div class="mb-3 row" v-show="is_kehoach">
                    <div class="col-md-6">
                        <label for="baselineStartDate" class="form-label">Kế hoạch bắt đầu</label>
                        <Calendar v-model="taskEdit.baselineStartDate" dateFormat="yy-mm-dd" class="date-custom"
                            :manualInput="false" showIcon showButtonBar />
                    </div>
                    <div class="col-md-6">
                        <label for="baselineEndDate" class="form-label">Kế hoạch kết thúc</label>
                        <Calendar v-model="taskEdit.baselineEndDate" dateFormat="yy-mm-dd" class="date-custom"
                            :manualInput="false" showIcon showButtonBar />
                    </div>
                </div>

                <div class="mb-3 row">
                    <div class="col-12">
                        <TabView>
                            <TabPanel header="Bình luận">
                                <div class="row">
                                    <div class="col">
                                        <Textarea v-model="chat" rows="2" cols="30"></Textarea>
                                    </div>
                                    <div class="col">
                                        <FileUpload name="attachments[]" url="/v1/task/fileupload"
                                            @before-upload="onAdvancedUpload" :showUploadButton="false" mode="basic"
                                            :showCancelButton="false" chooseLabel="Chọn" :multiple="true" :auto="true"
                                            :maxFileSize="1000000" :pt="{ chooseButton: { class: 'p-button-sm' } }">
                                        </FileUpload>
                                    </div>
                                    <div class="col">
                                        <Button label="Gửi" icon="pi pi-send" size="small"></Button>
                                    </div>
                                </div>
                            </TabPanel>
                            <TabPanel header="Hoạt động">

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

import ConfirmDialog from 'primevue/confirmdialog';
import { useConfirm } from "primevue/useconfirm";
const confirm = useConfirm();
const chat = ref();
const storeProject = useProject();
const { taskEdit, taskList, statusList, visibleSidebar, width } = storeToRefs(storeProject);

const emit = defineEmits(["reset", "beforeSave", "save"]);
const is_kehoach = ref(false);
const is_hanhoanthanh = ref(false);
const save = (e) => {
    e.preventDefault();
    if (!valid())
        return;
    emit("beforeSave");
    taskApi.Save(taskEdit.value).then((res) => {
        if (taskEdit.value.id > 0) {
            var find = taskList.value.findIndex((item) => {
                return item.id == taskEdit.value.id;
            });

            taskList.value[find] = res;
        } else {
            taskList.value.push(res);
        }

        taskEdit.value = {};
        emit("save");
    });
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
const onAdvancedUpload = (e) => {
    e.formData.append('projectId', taskEdit.value.projectId);
    return e;
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
    if (!$("#form-task").valid()) {
        return false;
    }
    return true;
}
onMounted(() => {
})
</script>

<style scoped>
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