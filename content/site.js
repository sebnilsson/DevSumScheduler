$(function () {
    'use strict';
    if (!Modernizr.localstorage) {
        $('body').addClass('no-localstorage');
    } else {
        $('body').addClass('has-localstorage');
        initScheduler();

        showInstructions();

        $('#reset-choices').click(function () {
            localStorage.clear();
            if (window.location.reload) {
                window.location.reload(false);
            } else {
                window.location.href = window.location.pathname;
            }
        });
    }
});

var selectedRowItemClass = 'selected-row-item';
var hasSelectedItemClass = 'has-selected-item';

var initScheduler = function () {
    'use strict';
    restoreSchedule();

    $('.row-item:not(".single-row-item")').click(function () {
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
};

var restoreSchedule = function () {
    'use strict';
    for (var key in localStorage) {
        var value = parseInt(localStorage[key]);
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
            removeSchedule(key, value);
        }
    }
};

var storeSchedule = function (key, index) {
    'use strict';
    localStorage[key] = index;
};

var removeSchedule = function (key, index) {
    'use strict';
    delete localStorage[key];
};

var showInstructions = function () {
    $(document).on('click', '#messages div', function () {
        $(this).remove();
    });

    if (localStorage.supressInstructions === 'true') {
        return;
    }

    var instructionMessage = $('<div><p>Click on a session to highlight it.</p></div>').click(function () {
        localStorage.supressInstructions = true;
    });

    $('#messages').append(instructionMessage);
};