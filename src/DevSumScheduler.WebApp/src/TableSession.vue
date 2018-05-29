<template>
    <td :class="{ 'bg-info text-white': !timeslot.isSelectable && !session.isSelected, 'bg-success text-white': session.isSelected, 'clickable': timeslot.isSelectable,
        'small p-2 popover-element': true }"
        ref="popoverElement">
        <div>
            <div>
                <strong>
                    {{ session.title }}
                </strong>
            </div>
            <div>
                <em v-show="session.speakerTitle">
                    {{ session.speakerTitle }}
                </em>
            </div>
        </div>
        <div class="hidden">
            <div ref="popoverContent">
                <button v-show="timeslot.isSelectable" @click="showSpeaker"
                        title="Details" class="btn btn-light btn-sm mr-1">
                    <i class="fas fa-info-circle"></i> Details
                </button>
                <button v-show="timeslot.isSelectable && !session.isSelected" @click="selectSession"
                        title="select" class="btn btn-success btn-sm">
                    <i class="far fa-check-square"></i> Select
                </button>
                <button v-show="timeslot.isSelectable && session.isSelected" @click="removeSession"
                        title="select" class="btn btn-warning btn-sm">
                    <i class="far fa-trash-alt"></i> Remove
                </button>
            </div>
        </div>
    </td>
</template>

<script lang="ts" src="./TableSession"></script>