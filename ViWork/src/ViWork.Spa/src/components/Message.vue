<template>
    <div class="message" :class="type">
        {{ prefixedMessage }}
    </div>
</template>

<script>
export default {
    props: {
        type: {
            type: String,
            required: false,
            default: "success"
        },
        
        message: {
            type: String,
            required: true
        }
    },
    
    computed: {
        prefixedMessage() {
            if(this.type == "error") return "Attention! " + this.message;
            else if(this.type == "success") return "Cool! " + this.message;
            else return this.message;
        }
    }
}
</script>

<style lang="scss">
    .message {
        &.success {
            color: green;
        }

        &.error {
            color: red;
        }
    }
</style>