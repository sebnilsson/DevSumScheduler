(function(window, document, undefined) {
    'use strict';
    $(function() {
        showInstructions();

        initScheduler();

        $('#reset-choices').click(function(event) {
            event.preventDefault();

            clearStore();

            if (window.location.reload) {
                window.location.reload(false);
            } else {
                window.location.href = window.location.pathname;
            }
        });
    });

    var selectedRowItemClass = 'selected-row-item';
    var hasSelectedItemClass = 'has-selected-item';

    var initScheduler = function() {
        if ($('#schedule-tables').length) {
            restoreSchedule();
        }

        $('.row-item:not(".row-item-single")').click(function(event) {
            if ($(event.target).is('a')) {
                return;
            }
            var $this = $(this);
            var $parent = $this.parent();

            var isSelected = $this.hasClass(selectedRowItemClass);

            $parent.children().removeClass(selectedRowItemClass);

            $this.toggleClass(selectedRowItemClass, !isSelected);
            $parent.toggleClass(hasSelectedItemClass, !isSelected);

            var id = $parent.attr('data-row-id');

            if (!isSelected) {
                var index = $parent.children('td').index($this);
                storeSchedule(id, index);
            } else {
                removeSchedule(id);
            }
        });

        $('#schedule-tables').addClass('ready');
    };

    var restoreSchedule = function() {
        var store = amplify.store();
        for (var key in store) {
            var value = parseInt(amplify.store(key));
            if (!isNaN(value)) {
                var siblingItems = $('tr[data-row-id="' + key + '"] td');
                if (siblingItems.length > 1) {
                    var $item = $(siblingItems[value]);
                    if ($item) {
                        $item.addClass(selectedRowItemClass);
                        $item.parent().addClass(hasSelectedItemClass);
                        continue;
                    }
                }
                removeSchedule(key);
            }
        }
    };

    var clearStore = function() {
        $('#schedule-tables').removeClass('ready');

        var store = amplify.store();
        for (var key in store) {
            amplify.store(key, null);
        }
    };

    var storeSchedule = function(key, index) {
        amplify.store(key, index);
    };

    var removeSchedule = function(key) {
        amplify.store(key, null);
    };

    var showInstructions = function() {
        $(document).on('click', '#messages div', function() {
            $(this).remove();
        });

        if (amplify.store('supressInstructions')) {
            return;
        }

        var instructionMessage = $('<div><p>Click on a session to highlight it.</p></div>').click(function() {
            amplify.store('supressInstructions', true);
        });

        $('#messages').append(instructionMessage);
    };
})(window, document);