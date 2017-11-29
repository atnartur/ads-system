function load(params) {
    $.get('', params).then(res => render(res));
}

function extendLink($html, cls, val) {
    $html.find('.' + cls).attr('href', $html.find('.' + cls).attr('href') + val);
}

function render(list) {
    console.log(list);
    let resultTemplate = '';
    let template = $('#template').html();
    list.forEach(function (row) {
        let $row = $(template);
        for (const key in row) {
            console.log(row[key]);
            $row.find('.' + key).text(typeof row[key] !== 'object' ? row[key] : row[key].map(a => a.Name).join(', '));
        }
        extendLink($row, 'deleteLink', row.Id);
        extendLink($row, 'editLink', row.Id);
        resultTemplate += $row[0].outerHTML;
    });
    $('#list').html(resultTemplate);
}

$(document).ready(load);