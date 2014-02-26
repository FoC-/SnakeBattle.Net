var SBN = SBN || {};
SBN.Contract = {
    colorMap: ['blue', 'green', 'grey', 'red', 'black'],
    imageMap: {
        ebody: '/img/eBody.png',
        ehead: '/img/eHead.png',
        empty: '/img/empty.png',
        etail: '/img/eTail.png',
        gateway: '/img/gateway.png',
        obody: '/img/oBody.png',
        ohead: '/img/oHead.png',
        otail: '/img/oTail.png',
        wall: '/img/wall.png'
    },
    content: ['empty', 'wall', 'head', 'body', 'tail', 'gateway']
};

SBN.Edit = function (settings, Renderer, SnakeService) {
    var selectors = settings.selectors;
    var template = $(selectors.template).html();
    var $container = $(selectors.container);

    function addChip(model) {
        var $kineticContainer = $(template).appendTo($container).find(selectors.kineticContainer);
        Renderer.render($kineticContainer, model);
    };

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

    Renderer.renderSelector($(selectors.selctorContainer));
};

SBN.Kinetic = {};
SBN.Kinetic.Configuration = {
    size: 30, //50
    fontSize: 18
};
SBN.Kinetic.createCell = function (cell) {
    var size = SBN.Kinetic.Configuration.size;
    var fontSize = SBN.Kinetic.Configuration.fontSize;
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
SBN.Kinetic.renderSelector = function ($container) {
    var model = {};
    $container.data('model', model);

    var size = SBN.Kinetic.Configuration.size;
    var elementNumber = 0;
    var stage = new Kinetic.Stage({
        container: $container[0],
        width: size * 14,
        height: size
    });
    var layer = new Kinetic.Layer();

    $.each(SBN.Contract.colorMap, function (index, value) {
        var rect = new Kinetic.Rect({
            x: elementNumber++ * size,
            y: 0,
            width: size,
            height: size,
            fill: value,
            opacity: 0.65
        });
        rect.on('mousedown', function () {
            model.color = index;
            stage.draw();
            alert(JSON.stringify(model));
        });
        layer.add(rect);
    });

    SBN.Service.ImageLoader.load(SBN.Contract.imageMap, function (images) {
        $.each(SBN.Contract.content, function (index, value) {
            var group = new Kinetic.Group();
            var x = elementNumber++ * size;
            var rect = new Kinetic.Rect({
                x: x,
                y: 0,
                width: size,
                height: size,
                fill: '#555',
                opacity: 0.65
            });
            group.add(rect);
            var image = new Kinetic.Image({
                x: x,
                y: 0,
                image: images[value] ? images[value] : images['e' + value],
                width: size,
                height: size
            });
            group.add(image);

            group.on('mousedown', function () {
                model.content = index;
                model.isSelf = false;
                stage.draw();
                alert(JSON.stringify(model));
            });
            layer.add(group);
            stage.draw();
        });
        $.each(SBN.Contract.content, function (index, value) {
            var img = images['o' + value];
            if (img) {
                var group = new Kinetic.Group();
                var x = elementNumber++ * size;
                var rect = new Kinetic.Rect({
                    x: x,
                    y: 0,
                    width: size,
                    height: size,
                    fill: '#555',
                    opacity: 0.65
                });
                group.add(rect);
                var image = new Kinetic.Image({
                    x: x,
                    y: 0,
                    image: img,
                    width: size,
                    height: size
                });
                group.add(image);

                group.on('mousedown', function () {
                    model.content = index;
                    model.isSelf = true;
                    stage.draw();
                    alert(JSON.stringify(model));
                });
                layer.add(group);
                stage.draw();
            }
        });
    });

    stage.add(layer);
};

SBN.Service = {};
SBN.Service.ImageLoader = {
    load: function (sources, callback) {
        var images = {},
         loaded = 0,
         total = 0;
        for (var source in sources) {
            images[source] = new Image();
            images[source].onload = function () {
                if (++loaded >= total) {
                    callback(images);
                }
            };
            total++;
        }
        for (var src in sources) {
            images[src].src = sources[src];
        }
    }
};

SBN.Service.Snake = {
    get: function (query, success) {
        var url = '/api/Snake/Get' + (query ? '?' + $.param(query) : '');
        $.ajax({
            type: 'GET',
            url: url,
            contentType: "application/json",
            dataType: 'json',
            success: success || $.noop
        });
    },
    save: function (snake, success) {
        $.ajax({
            type: 'POST',
            url: '/api/Snake/Save',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify(snake),
            success: success || $.noop
        });
    }
};
