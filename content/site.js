(function(window, document, undefined) {
    'use strict';
    $(function() {
        showInstructions();

        initScheduler();

        initResetChoices();

        initFancybox();
    });

    var selectedRowItemClass = 'selected-row-item',
        hasSelectedItemClass = 'has-selected-item';

    function initFancybox() {
        $('.fancybox').fancybox({
            type: 'iframe'
        });
    }

    function initResetChoices() {
        $('#reset-choices').click(function(e) {
            e.preventDefault();

            var result = confirm('Are you sure?');
            if (!result) {
                return;
            }

            clearStore();

            if (window.location.reload) {
                window.location.reload(false);
            } else {
                window.location.href = window.location.pathname;
            }
        });
    }

    function initScheduler() {
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
    }

    function clearStore() {
        $('#schedule-tables').removeClass('ready');

        var store = amplify.store();
        for (var key in store) {
            if (store.hasOwnProperty(key)) {
                amplify.store(key, null);
            }
        }
    }

    function removeSchedule(key) {
        amplify.store(key, null);
    }

    function restoreSchedule() {
        var store = amplify.store();

        for (var key in store) {
            if (store.hasOwnProperty(key)) {
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
        }
    }

    function storeSchedule(key, index) {
        amplify.store(key, index);
    }

    function showInstructions() {
        if (!$('#schedule-tables').length) {
            return;
        }

        $(document).on('click', '#messages div', function() {
            $(this).remove();
        });

        if (amplify.store('supressInstructions')) {
            return;
        }

        var instructionMessage = $('<div class="alert alert-info"><p>Click on a session to highlight it.</p></div>').click(function() {
            amplify.store('supressInstructions', true);
        });

        $('#messages').append(instructionMessage);
    }
})(window, document);