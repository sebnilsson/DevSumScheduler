$(function () {
    'use strict';
    if (!Modernizr.localstorage) {
        $('body').addClass('no-localstorage');
    } else {
        $('body').addClass('has-localstorage');
        initScheduler();
    }
});

var initScheduler = function () {
    'use strict';
    restoreSchedule();

    $('.row-item:not(".single-row-item")').click(function () {
        var $this = $(this);
        var $parent = $this.parent();

        $parent.children().removeClass('selected-row-item');
        $this.addClass('selected-row-item');

        var id = $parent.attr('data-row-id');
        var index = $parent.children('td').index($this);

        storeSchedule(id, index);
    });
};

var restoreSchedule = function () {
    'use strict';

    for (var key in localStorage) {
        var value = parseInt(localStorage[key]);
        if (!isNaN(value)) {
            var item = $('tr[data-row-id="' + key + '"] td')[value];
            $(item).addClass('selected-row-item');
        }
    }
};

var storeSchedule = function (id, index) {
    'use strict';

    localStorage[id] = index;
};