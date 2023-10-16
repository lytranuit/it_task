<template v-slot:columnTemplate="{ data }">
    <div class="columnTemplate" v-if="data.ganttProperties.resourceNames">
        <img :src="'https://ej2.syncfusion.com/vue/demos/source/gantt/images/' + data.ganttProperties.resourceNames + '.png'"
            style="height: 40px;" />
    </div>
</template>
  
<script setup>
</script>