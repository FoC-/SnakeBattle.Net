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
    var $selectorContainer = $(selectors.selctorContainer);


    function addChip(model) {
        var $kineticContainer = $(template).appendTo($container).find(selectors.kineticContainer);
        Renderer.render($kineticContainer, model, $selectorContainer);
    };

    $container.on('click', selectors.insertButton, function () {
        var parent = $(this).closest('.chip');
        var kineticContainer = $(template).insertAfter(parent).find(selectors.kineticContainer);
        Renderer.render(kineticContainer, [], $selectorContainer);
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
    Renderer.renderSelector($selectorContainer);
};

SBN.Kinetic = {};
SBN.Kinetic.Configuration = {
    size: 30,//50,
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
SBN.Kinetic.element = function (config, stage, handler) {
    var group = new Kinetic.Group();
    var background = new Kinetic.Rect({
        name: config.name,
        x: config.x,
        y: 0,
        width: config.size,
        height: config.size,
        fill: config.color,
        stroke: 'black',
        strokeWidth: 10
    });
    group.add(background);

    group.on('mousedown', function () {
        handler();

        var thisRect = background;
        var wasStroked = thisRect.strokeEnabled();
        var countElements = 0;
        stage.find('.' + config.name).each(function (shape) {
            shape.strokeEnabled(false);
            countElements++;
        });
        if (countElements === 1) {
            thisRect.strokeEnabled(!wasStroked);
        } else {
            thisRect.strokeEnabled(true);
        }

        stage.draw();
    });

    this.putImage = function (src) {
        var image = new Kinetic.Image({
            x: config.x,
            y: 0,
            image: src,
            width: config.size,
            height: config.size
        });
        group.add(image);
        return this;
    };

    this.putCross = function () {
        var crossLine1 = new Kinetic.Line({
            points: [config.x, 0, config.x + config.size, config.size],
            stroke: 'black',
            strokeWidth: 5
        });
        var crossLine2 = new Kinetic.Line({
            points: [config.x + config.size, 0, config.x, config.size],
            stroke: 'black',
            strokeWidth: 5
        });
        group.add(crossLine1);
        group.add(crossLine2);
        return this;
    };

    this.get = function () {
        return group;
    };
};
SBN.Kinetic.render = function ($container, model, $selectorContainer) {
    $container.data('model', model);
    var selected = $selectorContainer.data('model');

    var stage = new Kinetic.Stage({
        container: $container[0],
        width: 350,
        height: 350
    });

    var layer = new Kinetic.Layer();

    $.each(model.cells, function (index, cell) {
        var group = SBN.Kinetic.createCell(cell);
        group.on('mousedown', function () {
            cell.content = selected;
            stage.draw();
            alert(JSON.stringify(cell));
        });
        layer.add(group);
    });

    stage.add(layer);
};
SBN.Kinetic.renderSelector = function ($container) {
    var model = { content: 3, color: 2, isSelf: false, exclude: false };
    $container.data('model', model);

    var size = SBN.Kinetic.Configuration.size;
    var elementNumber = 0;
    var stage = new Kinetic.Stage({
        container: $container[0],
        width: size * 15,
        height: size
    });
    var layer = new Kinetic.Layer();

    $.each(SBN.Contract.colorMap, function (index, value) {
        var element = new SBN.Kinetic.element({
            x: elementNumber++ * size,
            name: 'color-selector',
            color: value,
            size: SBN.Kinetic.Configuration.size
        }, stage, function () {
            model.color = index;
        });
        layer.add(element.get());
    });

    var excludeSelector = new SBN.Kinetic.element({
        x: elementNumber++ * size,
        name: 'exclude-selector',
        color: '#555',
        size: SBN.Kinetic.Configuration.size
    }, stage, function () {
        model.exclude = !model.exclude;
    });
    excludeSelector.putCross();
    layer.add(excludeSelector.get());

    SBN.Service.ImageLoader.load(SBN.Contract.imageMap, function (images) {
        $.each(SBN.Contract.content, function (index, value) {
            var element = new SBN.Kinetic.element({
                x: elementNumber++ * size,
                name: 'content-selector',
                color: '#555',
                size: SBN.Kinetic.Configuration.size
            }, stage, function () {
                model.content = index;
                model.isSelf = false;
            });
            element.putImage(images[value] || images['e' + value]);

            layer.add(element.get());
            stage.draw();
        });
        $.each(SBN.Contract.content, function (index, value) {
            var img = images['o' + value];
            if (img) {
                var element = new SBN.Kinetic.element({
                    x: elementNumber++ * size,
                    name: 'content-selector',
                    color: '#555',
                    size: SBN.Kinetic.Configuration.size
                }, stage, function () {
                    model.content = index;
                    model.isSelf = true;
                });
                element.putImage(img);

                layer.add(element.get());
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
