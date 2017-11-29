const params = {
    search: null,
    zoneId: null
};

let timeout;

function load() {
    NProgress.start();
    $.get('?', params).then(res => {
        render(res);
        NProgress.done();
    });
}

function extendLink($html, cls, val) {
    $html.find('.' + cls).attr('href', $html.find('.' + cls).attr('href') + val);
}

function render(list) { 
    let resultTemplate = '';
    let template = $('#template').html();
    list.forEach(row => {
        let $row = $(template);
        
        for (const key in row) 
            $row.find('.' + key).text(typeof row[key] !== 'object' ? row[key] : row[key].map(a => a.Name).join(', '));
        
        extendLink($row, 'deleteLink', row.Id);
        extendLink($row, 'editLink', row.Id);
        
        resultTemplate += $row[0].outerHTML;
    });
    $('#list').html(resultTemplate);
}

function filtersInit() {
    $('.filter').on('keyup change', function () {
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            params[$(this).attr('id')] = $(this).val();
            load()
        }, 500);
    });
}

$(document).ready(function () {
    filtersInit();
    load();
});