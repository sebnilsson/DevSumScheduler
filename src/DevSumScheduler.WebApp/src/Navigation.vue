<template>
    <nav class="navbar navbar-expand-sm navbar-dark bg-dark">
        <span class="navbar-brand">DevSum Scheduler</span>
      
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbar-content">
          <span class="navbar-toggler-icon"></span>
        </button>

        <div v-show="days" class="collapse navbar-collapse" id="navbar-content">
            <ul class="navbar-nav">
                <li v-for="(day, index) in days" :class="{ 'active': (index === dayIndex), 'nav-item clickable': true }">
                    <a @click="changeDay(index)" class="nav-link">
                        {{ day.title }}
                    </a>
                </li>
            </ul>
        </div>
    </nav>
</template>

<script lang="ts">
  
  import { Vue, Component, Prop } from "vue-property-decorator";
  
  @Component
  export default class Navigation extends Vue {
      @Prop()
      days: string[];
      @Prop()
      dayIndex: number;
      
      changeDay (index: number) {
          this.$emit('day-change', index);
      }
  }

</script>