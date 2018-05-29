<template>
    <div>
        <div v-show="day.timeslots.length && day.locations.length" class="table-responsive">
            <table class="table table-bordered">
                <thead class="thead-dark">
                    <tr>
                        <th>
                            &nbsp;
                        </th>
                        <th v-for="location in day.locations" :style="{ width: columnWidth }">
                            {{ location }}
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="timeslot in day.timeslots">
                        <TableTimeslot :timeslot="timeslot" :dayIndex="dayIndex" />
                    </template>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts" src="./Table"></script>