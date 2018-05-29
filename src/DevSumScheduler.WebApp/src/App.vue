<template>
    <div>
        <Navigation :days="days" :dayIndex="dayIndex" @day-change="dayChange" />

        <div class="container mt-5">
            <h1 v-show="day.title">
                {{ day.title }}
            </h1>
        </div>

        <div v-show="!isDataLoading" class="container px-0">
            <Table :day="day" :dayIndex="dayIndex" />
        </div>

        <div v-show="isDataLoading" class="fa-3x text-center">
            <i class="fas fa-spinner fa-pulse"></i>
        </div>

        <div v-show="hasDataLoadFailed" class="container">
            <div class="card bg-warning text-center">
                <div class="card-body">
                    <p class="card-text">
                        Failed to load data.
                    </p>
                    <button class="btn btn-light" @click="loadAllData">Reload</button>
                </div>
            </div>
        </div>

        <div class="container">
            <div class="card bg-light text-center">
                <div class="card-body">
                    <p class="card-text">
                        All data retrieved from <a href="http://www.devsum.se">the original DevSum-website</a>.
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts" src="./App"></script>