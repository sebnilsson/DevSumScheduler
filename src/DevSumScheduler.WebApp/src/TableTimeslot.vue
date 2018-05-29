<template>
    <tr>
        <th class="table-secondary text-center small p-2">
          {{ timeslot.startsAt }} - {{ timeslot.endsAt }}
        </th>
        <template v-for="(session, index) in timeslot.sessions">
            <TableSession :session="session" :sessionIndex="index" :timeslot="timeslot" :dayIndex="dayIndex" />
        </template>
    </tr>
</template>

<script lang="ts" src="./TableTimeslot"></script>