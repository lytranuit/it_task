<template>
    <beautiful-chat :participants="participants" :titleImageUrl="titleImageUrl" :onMessageWasSent="onMessageWasSent"
        :messageList="messageList" :newMessagesCount="newMessagesCount" :isOpen="isChatOpen" :close="closeChat"
        :open="openChat" :showEmoji="false" :showFile="false" :showEdition="false" :showDeletion="false"
        :deletionConfirmation="false" :showLauncher="true" :showCloseButton="true" :colors="colors"
        :alwaysScrollToBottom="alwaysScrollToBottom" :disableUserListToggle="true" :messageStyling="messageStyling">
        <template v-slot:header>
            ChatGPT
        </template>
    </beautiful-chat>
</template>
<script setup>
import { computed, watch, ref, onMounted } from "vue";
import { rand } from "../../utilities/rand";
const participants = ref([
    {
        id: 'chatgpt',
        name: 'ChatGpt',
        imageUrl: '/src/assets/images/chatgpt.png'
    }
])
const titleImageUrl = ref("/src/assets/images/chatgpt.png");
const messageList = ref([]);
const newMessagesCount = ref(0);
const isChatOpen = ref(false);
const alwaysScrollToBottom = ref(false);
const messageStyling = ref(false);
const colors = ref({
    header: {
        bg: '#4e8cff',
        text: '#ffffff'
    },
    launcher: {
        bg: '#4e8cff'
    },
    messageList: {
        bg: '#ffffff'
    },
    sentMessage: {
        bg: '#4e8cff',
        text: '#ffffff'
    },
    receivedMessage: {
        bg: '#eaeaea',
        text: '#222222'
    },
    userInput: {
        bg: '#f4f7f9',
        text: '#565867'
    }
});
const isThingking = ref(false);
const onMessageWasSent = async (message) => {
    if (isThingking.value == true)
        return false;
    messageList.value = [...messageList.value, message]
    try {
        isThingking.value = true;
        console.log(message);
        callOpenAI(message.data.text)
        // console.log('Stream ended.')
    } catch (error) {
        console.error(error)
        res.value = error.response.data.error.message
    } finally {
        isThingking.value = false;
    }
}
const callOpenAI = async (text) => {
    const userMessages = [{ role: 'user', content: text }]

    const requestData = JSON.stringify({
        model: 'gpt-3.5-turbo',
        messages: userMessages,
        stream: true,
    })

    const fetchOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${import.meta.env.VITE_OPEN_API_KEY}`,
        },
        body: requestData,
    }

    const response = await fetch('https://api.openai.com/v1/chat/completions', fetchOptions)
    const reader = response.body.getReader()
    let id = rand();
    var text = "";
    var newMessage = { id: id, author: 'chatgpt', type: 'text', data: { text } }
    // console.log(newMessage)
    messageList.value = [...messageList.value, newMessage]
    while (true) {
        const { done, value } = await reader.read()
        if (done) break

        const chunkStr = new TextDecoder('utf-8').decode(value)
        const lines = chunkStr
            .split('\n')
            .filter((line) => line !== '' && line.length > 0)
            .map((line) => line.replace(/^data: /, '').trim())
            .filter((line) => line !== '[DONE]')
            .map((line) => JSON.parse(line))
        for (const line of lines) {
            const {
                choices: [
                    {
                        delta: { content },
                    },
                ],
            } = line
            if (content) {
                const m = messageList.value.find(m => m.id === id);
                m.data.text += content
            }
        }
    }
}
const openChat = () => {
    // called when the user clicks on the fab button to open the chat
    isChatOpen.value = true
    newMessagesCount.value = 0
}
const closeChat = () => {
    // called when the user clicks on the botton to close the chat
    isChatOpen.value = false
}
</script>
<style>
.sc-header--img {
    width: 50px;
}

.sc-launcher {
    z-index: 100;
}
</style>