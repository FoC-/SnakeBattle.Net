'use strict';

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

SBN.Edit = function (settings, Renderer, Snake, ImageLoader) {
    var imageLoader = ImageLoader.load(SBN.Contract.imageMap);
    var snake = Snake(settings.snake);
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
        imageLoader.then(function (images) {
            Renderer.render($kineticContainer, model, $selectorContainer, images);
        });
    };

    $container.on('click', selectors.insertButton, function () {
        var parent = $(this).closest('.chip');
        var $kineticContainer = $(chipView).insertBefore(parent).find(selectors.kineticContainer);
        imageLoader.then(function (images) {
            Renderer.render($kineticContainer, [], $selectorContainer, images);
        });
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
        var snakeTemplate = {
            id: settings.snake.id,
            name: name,
            chips: []
        };

        $container.find(selectors.kineticContainer).each(function () {
            snakeTemplate.chips[snakeTemplate.chips.length] = mapToModel($(this).data('model'));
        });
        snake.Save(snakeTemplate, function (data) {
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

    snake.Get(function (data) {
        $.each(data.chips, function (index, value) {
            addChip(value);
        });
    });
    imageLoader.then(function (images) {
        Renderer.renderSelector($selectorContainer, images);
    });
};
SBN.Show = function (settings, Battle, ImageLoader) {
    var imageLoader = ImageLoader.load(SBN.Contract.imageMap);
    var $container = $(settings.selectors.container);
    var $buttonStart = $(settings.selectors.buttonStart);
    var $selectSpeed = $(settings.selectors.selectSpeed);
    var speedSelector;
    if ($selectSpeed[0]) {
        speedSelector = function () {
            return $selectSpeed.val();
        };
    } else {
        speedSelector = function () {
            return settings.auto.speed;
        };
    }

    imageLoader.then(function (images) {
        Battle(settings.snakes).Get(function (replay) {
            var layer = SBN.Kinetic.renderBattleField($container, replay.battleField, images);
            var anim = SBN.Kinetic.animation(replay.frames, layer, images, speedSelector, function () {
                $buttonStart.html('Stop');
                anim.stop();
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
            if (settings.auto && settings.auto.play) {
                anim.start();
            }
        });
    });
};

SBN.Kinetic = {
    Configuration: {
        size: 50
    },
    createCell: function (layer, cell, images, selectorModel) {
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
    },
    element: function (config, layer, handler) {
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
    },
    render: function ($container, model, $selectorContainer, images) {
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

        $.each(map, function (i, col) {
            $.each(col, function (j, cell) {
                SBN.Kinetic.createCell(layer, cell, images, selectorModel);
            });
        });
    },
    renderSelector: function ($container, images) {
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

        stage.add(layer);
    },
    renderBattleField: function ($container, model, images) {
        var stage = new Kinetic.Stage({
            container: $container[0],
            width: 810,
            height: 810
        });
        var background = new Kinetic.Layer();
        var mainLayer = new Kinetic.Layer();
        stage.add(background);
        stage.add(mainLayer);

        $.each(model, function (index, cell) {
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

        return mainLayer;
    },
    animation: function (frames, layer, images, speedSelector, onFinish, onEach) {
        var frameNumber = 0,
            frameIndex = 0;
        var anim = new Kinetic.Animation(function (frame) {
            //var frameRate = Math.floor(frame.frameRate / 3);
            if (++frameNumber % speedSelector() === 0) {
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
    },
};

SBN.Service = {
    ImageLoader: new function () {
        var self = this,
            images = {},
            loaded = 0,
            total = 0,
            clb = undefined,
            isAsyncComplete = false;

        this.load = function (sources) {
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
            return self;
        };

        this.then = function (callback) {
            if (isAsyncComplete) {
                callback(images);
            } else {
                clb = callback;
            }
        };
    },
    Resource: new function () {
        var self = this;
        var endpoint = {};
        var ajaxCall = function (success, error, verb, data) {
            verb = verb || 'GET';

            var settings = {
                url: endpoint[verb],
                type: verb,
                contentType: "application/json",
                dataType: 'json',
                success: success || $.noop,
                error: error || $.noop
            };
            if (data) {
                settings.data = JSON.stringify(data);
            }

            $.ajax(settings);
        };
        this.Get = function (success) {
            ajaxCall(success);
        };
        this.Save = function (data, success, error) {
            ajaxCall(success, error, 'POST', data);
        };

        this.Snake = function (query) {
            endpoint.GET = '/api/Snake/' + (query ? 'Get?' + $.param(query) : 'Get');
            endpoint.POST = '/api/Snake/Save';
            return self;
        };
        this.Battle = function (query) {
            endpoint.GET = '/api/Battle/' + (query ? 'Get?' + $.param(query) : 'Demo');
            return self;
        };
    },
};

SBN.App = new function () {
    var FN_ARGS = /^function\s*[^\(]*\(\s*([^\)]*)\)/m;
    var FN_ARG_SPLIT = /,/;
    var FN_ARG = /^\s*(_?)(\S+?)\1\s*$/;
    var STRIP_COMMENTS = /((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg;
    var self = this;
    var dependencies = {};

    this.set = function (name, dependency) {
        dependencies[name] = dependency;
        return self;
    };
    this.run = function (target) {
        var text = target.toString().replace(STRIP_COMMENTS, '');
        var args = text.match(FN_ARGS)[1].split(FN_ARG_SPLIT);
        var params = args.map(function (value) {
            return dependencies[value.replace(FN_ARG, function (match, offset, name) { return name; })];
        });

        target.apply(target, params);
    };
};
SBN.App.set('Battle', SBN.Service.Resource.Battle);
SBN.App.set('Snake', SBN.Service.Resource.Snake);
SBN.App.set('ImageLoader', SBN.Service.ImageLoader);
SBN.App.set('Renderer', SBN.Kinetic);
