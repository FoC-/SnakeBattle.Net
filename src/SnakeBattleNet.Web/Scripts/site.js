var SBN = SBN || {};
SBN.Contract = {
    colorMap: ['blue', 'green', 'grey', 'red', 'black']
};

SBN.Edit = function (settings, Renderer, SnakeService) {
    var selectors = settings.selectors;
    var template = $(selectors.template).html();
    var $container = $(selectors.container);

    function addChip(model) {
        var $kineticContainer = $(template).appendTo($container).find(selectors.kineticContainer);
        Renderer.render($kineticContainer, model);
    }

    $container.on('click', selectors.insertButton, function () {
        var parent = $(this).closest('.chip');
        var kineticContainer = $(template).insertAfter(parent).find(selectors.kineticContainer);
        Renderer.render(kineticContainer, []);
    });

    $container.on('click', selectors.deleteButton, function () {
        $(this).closest('.chip').remove();
    });

    $(selectors.addButton).on('click', function () {
        addChip([]);
    });

    $(selectors.saveButton).on('click', function () {
        var m = [];
        $container.find(selectors.kineticContainer).each(function () {
            m[m.length] = $(this).data('model');
        });
        alert(JSON.stringify(m));
        SnakeService.save(m, function (data) {
            alert('Response: ' + JSON.stringify(data));
        });
    });

    SnakeService.get(settings.snake, function (snake) {
        var chips = snake.chips;
        $.each(chips, function (index, value) {
            addChip(value);
        });
    });
};

SBN.Kinetic = {};
SBN.Kinetic.createCell = function (cell) {
    var size = 30; //50
    var fontSize = 18;
    var x = cell.position.x * size;
    var y = cell.position.y * size;
    var group = new Kinetic.Group();

    var rect = new Kinetic.Rect({
        x: x,
        y: y,
        width: size,
        height: size,
        fill: SBN.Contract.colorMap[cell.content.color],
        opacity: 0.65
    });
    group.add(rect);

    var content = new Kinetic.Text({
        x: x,
        y: y,
        text: cell.content.content,
        fontSize: fontSize,
        fontFamily: 'Calibri',
        fill: 'black',
        width: size,
        padding: (size - fontSize) / 2,
        align: 'center'
    });
    group.add(content);

    if (cell.content.exclude) {
        var crossLine1 = new Kinetic.Line({
            points: [x, y, x + size, y + size],
            stroke: 'black',
            strokeWidth: 1
        });
        var crossLine2 = new Kinetic.Line({
            points: [x + size, y, x, y + size],
            stroke: 'black',
            strokeWidth: 1
        });
        group.add(crossLine1);
        group.add(crossLine2);
    }

    return group;
};
SBN.Kinetic.render = function ($container, model) {
    $container.data('model', model);

    var stage = new Kinetic.Stage({
        container: $container[0],
        width: 350,
        height: 350
    });

    var layer = new Kinetic.Layer();

    $.each(model.cells, function (index, cell) {
        var group = SBN.Kinetic.createCell(cell);
        group.on('mousedown', function () {
            alert(JSON.stringify(cell));
        });
        layer.add(group);
    });

    stage.add(layer);
};

SBN.Service = {};
SBN.Service.Snake = {};
SBN.Service.Snake.get = function (query, success) {
    var url = '/api/Snake/Get' + (query ? '?' + $.param(query) : '');
    $.ajax({
        type: 'GET',
        url: url,
        contentType: "application/json",
        dataType: 'json',
        success: success || $.noop
    });
};
SBN.Service.Snake.save = function (snake, success) {
    $.ajax({
        type: 'POST',
        url: '/api/Snake/Save',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify(snake),
        success: success || $.noop
    });
};
