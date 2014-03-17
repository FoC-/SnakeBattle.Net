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
    var chipView = $(selectors.chipView).html();
    var alertView = $(selectors.alertView).html();
    var $container = $(selectors.container);
    var $selectorContainer = $(selectors.selctorContainer);
    var $nameInput = $(selectors.nameInput);

    var mapToModel = function (map) {
        var m = [];
        $.each(map, function (i, col) {
            $.each(col, function (j, cell) {
                if (cell.color) {
                    m[m.length] = cell;
                }
            });
        });
        return m;
    };

    function addChip(model) {
        var $kineticContainer = $(chipView).appendTo($container).find(selectors.kineticContainer);
        Renderer.render($kineticContainer, model, $selectorContainer);
    };

    $container.on('click', selectors.insertButton, function () {
        var parent = $(this).closest('.chip');
        var $kineticContainer = $(chipView).insertAfter(parent).find(selectors.kineticContainer);
        Renderer.render($kineticContainer, [], $selectorContainer);
    });

    $container.on('click', selectors.deleteButton, function () {
        $(this).closest('.chip').remove();
    });

    $(selectors.addButton).on('click', function () {
        addChip([]);
    });

    $(selectors.saveButton).on('click', function () {
        $(".alert").alert('close');

        var name = $nameInput.val();
        var snake = {
            id: settings.snake.id,
            name: name,
            chips: []
        };

        $container.find(selectors.kineticContainer).each(function () {
            snake.chips[snake.chips.length] = mapToModel($(this).data('model'));
        });
        SnakeService.save(snake, function (data) {
            alert('Response: ' + JSON.stringify(data));
        }, function (data) {
            if (data.statusText == 'OK') return;
            var $alertView = $(alertView);
            var $alertList = $alertView.find('dl');
            var responseText = $.parseJSON(data.responseText);
            $.each(responseText.modelState, function (key, value) {
                $alertList.append('<dt>' + key + '</dt>');
                $.each(value, function (index, message) {
                    $alertList.append('<dd>' + message + '</dd>');
                });
            });
            $selectorContainer.append($alertView);
        });
    });

    SnakeService.get(settings.snake, function (snake) {
        $.each(snake.chips, function (index, value) {
            addChip(value);
        });
    });
    Renderer.renderSelector($selectorContainer);
};

SBN.Kinetic = {};
SBN.Kinetic.Configuration = {
    size: 50
};
SBN.Kinetic.createCell = function (layer, cell, images, selectorModel) {
    var size = SBN.Kinetic.Configuration.size;
    var x = cell.p.x * size;
    var y = cell.p.y * size;

    var element = new SBN.Kinetic.element({
        x: x,
        y: y,
        name: 'cell-selector',
        selected: false,
        color: SBN.Contract.colorMap[cell.color],
        size: SBN.Kinetic.Configuration.size
    }, layer, function () {
        element.get().remove();
        cell.color = selectorModel.color;
        cell.c = selectorModel.content;
        cell.exclude = selectorModel.exclude;
        cell.isSelf = selectorModel.isSelf;

        SBN.Kinetic.createCell(layer, cell, images, selectorModel);
        layer.draw();
    });

    var content = SBN.Contract.content[cell.c];
    var image = cell.isSelf ? images['o' + content] : images[content] || images['e' + content];
    element.putImage(image);

    if (cell.exclude) {
        element.putCross();
    }

    layer.add(element.get());
    layer.draw();
};
SBN.Kinetic.element = function (config, layer, handler) {
    var group = new Kinetic.Group();
    var background = new Kinetic.Rect({
        name: config.name,
        x: config.x,
        y: config.y,
        width: config.size,
        height: config.size,
        fill: config.color,
        stroke: 'black',
        strokeWidth: 10,
        strokeEnabled: config.selected
    });
    group.add(background);

    group.on('mousedown', function () {
        handler();

        var thisRect = background;
        var wasStroked = thisRect.strokeEnabled();
        var countElements = 0;
        layer.find('.' + config.name).each(function (shape) {
            shape.strokeEnabled(false);
            countElements++;
        });
        if (countElements === 1) {
            thisRect.strokeEnabled(!wasStroked);
        } else {
            thisRect.strokeEnabled(true);
        }

        layer.draw();
    });

    this.putImage = function (src) {
        var image = new Kinetic.Image({
            x: config.x + 5,
            y: config.y + 5,
            image: src,
            width: config.size - 10,
            height: config.size - 10
        });
        group.add(image);
        return this;
    };

    this.putCross = function () {
        var crossLine1 = new Kinetic.Line({
            points: [config.x, config.y, config.x + config.size, config.y + config.size],
            stroke: 'black',
            strokeWidth: 5
        });
        var crossLine2 = new Kinetic.Line({
            points: [config.x + config.size, config.y, config.x, config.y + config.size],
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
    var selectorModel = $selectorContainer.data('model');

    var stage = new Kinetic.Stage({
        container: $container[0],
        width: 350,
        height: 350
    });
    var layer = new Kinetic.Layer();
    var background = new Kinetic.Rect({
        x: 0,
        y: 0,
        width: 350,
        height: 350,
        fill: '#555',
        opacity: 0.1
    });
    layer.add(background);
    stage.add(layer);

    var map = [];
    for (var x = 0; x < 7; x++) {
        map[x] = [];
        for (var y = 0; y < 7; y++) {
            map[x][y] = { p: { x: x, y: y }, c: {} };
        }
    }
    $container.data('model', map);

    $.each(model, function (index, cell) {
        map[cell.p.x][cell.p.y] = cell;
    });

    new SBN.Service.ImageLoader(SBN.Contract.imageMap).then(function (images) {
        $.each(map, function (i, col) {
            $.each(col, function (j, cell) {
                SBN.Kinetic.createCell(layer, cell, images, selectorModel);
            });
        });
    });
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
            x: elementNumber++ * size + 5,
            y: 5,
            name: 'color-selector',
            selected: model.color === index,
            color: value,
            size: SBN.Kinetic.Configuration.size - 10
        }, stage, function () {
            model.color = index;
        });
        layer.add(element.get());
    });

    var excludeSelector = new SBN.Kinetic.element({
        x: elementNumber++ * size + 5,
        y: 5,
        name: 'exclude-selector',
        selected: model.exclude,
        color: '#555',
        size: SBN.Kinetic.Configuration.size - 10
    }, stage, function () {
        model.exclude = !model.exclude;
    });
    excludeSelector.putCross();
    layer.add(excludeSelector.get());

    new SBN.Service.ImageLoader(SBN.Contract.imageMap).then(function (images) {
        $.each(SBN.Contract.content, function (index, value) {
            var element = new SBN.Kinetic.element({
                x: elementNumber++ * size + 5,
                y: 5,
                name: 'content-selector',
                selected: model.content == index && model.isSelf == false,
                color: '#555',
                size: SBN.Kinetic.Configuration.size - 10
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
                    x: elementNumber++ * size + 5,
                    y: 5,
                    name: 'content-selector',
                    selected: model.content == index && model.isSelf == true,
                    color: '#555',
                    size: SBN.Kinetic.Configuration.size - 10
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
SBN.Service.ImageLoader = function (sources) {
    var images = {},
        loaded = 0,
        total = 0,
        clb = undefined,
        isAsyncComplete = false;

    for (var source in sources) {
        images[source] = new Image();
        images[source].onload = function () {
            if (++loaded >= total) {
                isAsyncComplete = true;
                clb && clb(images);
            }
        };
        total++;
    }
    for (var src in sources) {
        images[src].src = sources[src];
    }

    this.then = function (callback) {
        if (isAsyncComplete) {
            callback(images);
        } else {
            clb = callback;
        }
    };
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
    save: function (snake, success, error) {
        $.ajax({
            type: 'POST',
            url: '/api/Snake/Save',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify(snake),
            success: success || $.noop,
            error: error || $.noop
        });
    }
};

SBN.Show = function (settings) {
    var $container = $(settings.selectors.container);
    var $buttonStart = $(settings.selectors.buttonStart);
    var $selectSpeed = $(settings.selectors.selectSpeed);

    function render(battleField) {
        var stage = new Kinetic.Stage({
            container: $container[0],
            width: 810,
            height: 810
        });
        var background = new Kinetic.Layer();
        var mainLayer = new Kinetic.Layer();
        stage.add(background);
        stage.add(mainLayer);

        new SBN.Service.ImageLoader(SBN.Contract.imageMap).then(function (images) {
            $.each(battleField, function (index, cell) {
                var src = images[SBN.Contract.content[cell.c]];
                var image = new Kinetic.Image({
                    x: cell.p.x * 30,
                    y: cell.p.y * 30,
                    image: src,
                    width: 30,
                    height: 30
                });
                background.add(image);
            });
            background.draw();
        });

        return mainLayer;
    };

    function animation(frames, layer, images, onFinish, onEach) {
        var frameNumber = 0,
            frameIndex = 0;
        var anim = new Kinetic.Animation(function (frame) {
            //var frameRate = Math.floor(frame.frameRate / 3);
            if (++frameNumber % $selectSpeed.val() === 0) {
                layer.removeChildren();
                var frm = frames[frameIndex];
                if (frm) {
                    $.each(frm, function (key, value) {
                        $.each(value, function (index, cell) {
                            var content = SBN.Contract.content[cell.content];
                            var src = images[content] || images['o' + content];
                            var image = new Kinetic.Image({
                                x: cell.x * 30,
                                y: cell.y * 30,
                                image: src,
                                width: 30,
                                height: 30
                            });
                            layer.add(image);
                        });
                    });
                    layer.draw();
                    frameIndex++;
                    onEach();
                } else {
                    frameIndex--;
                    onFinish();
                }
            }
        }, layer);
        return anim;
    }
    new SBN.Service.ImageLoader(SBN.Contract.imageMap).then(function (images) {
        SBN.Service.Battle.get(settings.snakes, function (replay) {
            var layer = render(replay.battleField);
            var anim = animation(replay.frames, layer, images, function () {
                $buttonStart.html('Stop');
            }, $.noop);
            $buttonStart.on('click', function () {
                if (anim.isRunning()) {
                    $buttonStart.html('Start');
                    anim.stop();
                } else {
                    $buttonStart.html('Stop');
                    anim.start();
                }
            });
        });
    });
};
SBN.Service.Battle = {
    get: function (query, success) {
        var url = '/api/Battle/' + (query ? 'Get?' + $.param(query) : 'Demo');
        $.ajax({
            type: 'GET',
            url: url,
            contentType: "application/json",
            dataType: 'json',
            success: success || $.noop
        });
    }
};