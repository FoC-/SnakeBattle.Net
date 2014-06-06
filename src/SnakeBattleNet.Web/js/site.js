'use strict';

var SBN = SBN || {};
SBN.Contract = {
    colorMap: { Blue: 'blue', Green: 'green', Grey: 'grey', Red: 'red', Black: 'black' },
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
    content: { Empty: 'empty', Wall: 'wall', Head: 'head', Body: 'body', Tail: 'tail', Gateway: 'gateway' }
};
SBN.Resource = new function () {
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
};
SBN.AnimationStream = (function () {
    var stream;
    var createStream = function () {
        var frames = [],
        speed = 8,
        skipIndex = 0,
        currentFrame;
        var add = function (frame) {
            frames[frames.length] = frame;
        }
        var get = function () {
            if (++skipIndex % speed === 0) {
                skipIndex = 0;
                var tFrame = frames.shift();
                if (tFrame) {
                    currentFrame = tFrame;
                }
            }
            return currentFrame;
        };
        var setSpeed = function (spd) {
            speed = spd;
        };
        return {
            get: get,
            add: add,
            setSpeed: setSpeed
        }
    };
    return {
        getStream: function () {
            if (!stream) {
                stream = createStream();
            }
            return stream;
        }
    };
})();

SBN.AEM = new function () {
    var listeners = {};
    this.sub = function (name, func) {
        listeners[name] = listeners[name] || [];
        listeners[name][listeners[name].length] = func;
    };
    this.pub = function (e) {
        var cbs = listeners[e.name];
        if (cbs) {
            for (var j = 0; j < cbs.length; j++) {
                (function (f) {
                    setTimeout(function () {
                        f(e.msg, e);
                    }, 0);
                })(cbs[j]);
            }
        }
    }
};
SBN.AEM.sub('LoadImages', function (sources, header) {
    var images = {}, loaded = 0, total = 0;
    for (var source in sources) {
        images[source] = new Image();
        images[source].onload = function () {
            if (++loaded >= total) SBN.AEM.pub({ name: 'ImagesLoaded', msg: images, chanel: header.chanel });
        };
        total++;
    }
    for (var src in sources) {
        images[src].src = sources[src];
    }
});
SBN.AEM.sub('RenderChip', function (message) {
    var selectorModel = message.$selectorContainer.data('model');

    var stage = new Kinetic.Stage({
        container: message.$kineticContainer[0],
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
    message.$kineticContainer.data('model', map);

    $.each(message.model, function (index, cell) {
        map[cell.p.x][cell.p.y] = cell;
    });

    $.each(map, function (i, col) {
        $.each(col, function (j, cell) {
            SBN.Kinetic.createCell(layer, cell, message.images, selectorModel);
        });
    });
});
SBN.AEM.sub('RenderSelector', function (message) {
    var model = { content: 'Tail', color: 'Grey', isSelf: false, exclude: false };
    message.$selectorContainer.data('model', model);

    var size = SBN.Kinetic.Configuration.size;
    var elementNumber = 0;
    var stage = new Kinetic.Stage({
        container: message.$selectorContainer[0],
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
        element.putImage(message.images[value] || message.images['e' + value]);

        layer.add(element.get());
        stage.draw();
    });
    $.each(SBN.Contract.content, function (index, value) {
        var img = message.images['o' + value];
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
});
SBN.AEM.sub('SaveSnake', function (snake, header) {
    SBN.Resource.Snake().Save(snake, function (data) {
        SBN.AEM.pub({ name: 'SnakeSaveSucess', msg: data, chanel: header.chanel });
    }, function (error) {
        SBN.AEM.pub({ name: 'SnakeSaveError', msg: error, chanel: header.chanel });
    });
});
SBN.AEM.sub('GetSnake', function (id, header) {
    SBN.Resource.Snake({ id: id }).Get(function (data) {
        SBN.AEM.pub({ name: 'SnakeRecieved', msg: data, chanel: header.chanel });
    });
});
SBN.AEM.sub('GetBattle', function (snakes, header) {
    SBN.Resource.Battle(snakes).Get(function (data) {
        SBN.AEM.pub({ name: 'BattleRecieved', msg: data.events, chanel: header.chanel });
    });
});
SBN.AEM.sub('RenderEditSnake', function (settings) {
    var chanel = 'RenderEditSnake';
    var selectors = settings.selectors;
    var chipView = $(selectors.chipView).html();
    var alertView = $(selectors.alertView).html();
    var $container = $(selectors.container);
    var $selectorContainer = $(selectors.selctorContainer);
    var $nameInput = $(selectors.nameInput);
    var images;

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

    SBN.AEM.sub('SnakeSaveError', function (data) {
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
    SBN.AEM.sub('SnakeRecieved', function (snake) {
        SBN.AEM.pub({ name: 'RenderSelector', msg: { $selectorContainer: $selectorContainer, images: images }, chanel: chanel });

        function addChip(model) {
            var $kineticContainer = $(chipView).appendTo($container).find(selectors.kineticContainer);
            SBN.AEM.pub({ name: 'RenderChip', msg: { $kineticContainer: $kineticContainer, model: model, $selectorContainer: $selectorContainer, images: images }, chanel: chanel });
        };

        $container.on('click', selectors.insertButton, function () {
            var parent = $(this).closest('.chip');
            var $kineticContainer = $(chipView).insertBefore(parent).find(selectors.kineticContainer);
            SBN.AEM.pub({ name: 'RenderChip', msg: { $kineticContainer: $kineticContainer, model: [], $selectorContainer: $selectorContainer, images: images }, chanel: chanel });
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

            SBN.AEM.pub({ name: 'SaveSnake', msg: snakeTemplate, chanel: chanel });
        });

        $.each(snake.chips, function (index, value) {
            addChip(value);
        });
    });
    SBN.AEM.sub('ImagesLoaded', function (message, headers) {
        if (headers.chanel !== chanel) return;
        images = message;
        SBN.AEM.pub({ name: 'GetSnake', msg: settings.snake.id, chanel: chanel });
    });

    SBN.AEM.pub({ name: 'LoadImages', msg: SBN.Contract.imageMap, chanel: chanel });
});
SBN.AEM.sub('RenderBattle', function (settings) {
    var chanel = 'RenderBattle';
    var $container = $(settings.selectors.container);
    var $buttonStart = $(settings.selectors.buttonStart);
    var $selectSpeed = $(settings.selectors.selectSpeed);
    var images;

    if ($selectSpeed) {
        $selectSpeed.change(function () {
            SBN.AnimationStream.getStream().setSpeed($(this).val());
        });
    } else {
        SBN.AnimationStream.getStream().setSpeed(settings.auto.speed);
    }

    var imageSelector = function (content) {
        var c = SBN.Contract.content[content];
        if (images[c]) {
            return images[c];
        } else {
            return images['o' + c];
        }
    };

    var stage = new Kinetic.Stage({
        container: $container[0],
        width: 810,
        height: 810
    });

    var background = new Kinetic.Layer();
    var animationLayer = new Kinetic.Layer();
    stage.add(background);
    stage.add(animationLayer);

    var putContent = function (layer, x, y, content) {
        layer.add(new Kinetic.Image({
            x: x * 30,
            y: y * 30,
            image: imageSelector(content),
            width: 30,
            height: 30
        }));
    };

    var animation = new Kinetic.Animation(function () {
        var frame = SBN.AnimationStream.getStream().get();
        if (!frame) return;

        animationLayer.removeChildren();
        $.each(frame, function (key, value) {
            var len = value.length;
            if (len > 0) {
                var head = value[len - 1];
                if (head) putContent(animationLayer, head.x, head.y, 'Head');
            }
            if (len > 1) {
                var tail = value[0];
                if (tail) putContent(animationLayer, tail.x, tail.y, 'Tail');
            }
            if (len > 2) {
                for (var i = 1; i < len - 1; i++) {
                    var body = value[i];
                    putContent(animationLayer, body.x, body.y, 'Body');
                }
            }
        });
        animationLayer.draw();
    }, animationLayer);

    SBN.AEM.sub('AnimationStart', function () {
        animation.start();
    });
    SBN.AEM.sub('AnimationStop', function () {
        animation.stop();
    });
    SBN.AEM.sub('BattleRecieved', function (replay) {
        var snakes = {},
            snake;
        var event;
        while (event = replay.shift()) {
            switch (event.name) {
                case 'GameInit':
                    {
                        $.each(event.battleField, function (index, cell) {
                            putContent(background, cell.p.x, cell.p.y, cell.c);
                        });
                        background.draw();
                        break;
                    }
                case 'SnakeGrow':
                    {
                        snake = snakes[event.snake] || [];
                        snake[snake.length] = event.newHeadPosition;
                        snakes[event.snake] = snake;
                        break;
                    }
                case 'SnakeMove':
                    {
                        snake = snakes[event.snake] || [];
                        snake[snake.length] = event.newHeadPosition;
                        snake.splice(0, 1);
                        snakes[event.snake] = snake;
                        break;
                    }
                case 'SnakeBite':
                    {
                        snake = snakes[event.snake] || [];
                        var targetSnake = snakes[event.targetSnake] || [];
                        var tail = targetSnake.shift();
                        snake[snake.length] = tail;

                        snakes[event.snake] = snake;
                        snakes[event.targetSnake] = targetSnake;
                        break;
                    }
                case 'GameStartRound':
                    {
                        var snks = JSON.parse(JSON.stringify(snakes));
                        SBN.AnimationStream.getStream().add(snks);
                        break;
                    }
                default:
                    SBN.AEM.pub({ name: event.name, msg: event });
            }
        }

        if (settings.auto && settings.auto.play) {
            SBN.AEM.pub({ name: 'AnimationStart' });
        }
    });
    SBN.AEM.sub('ImagesLoaded', function (message, headers) {
        if (headers.chanel !== chanel) return;
        images = message;
        SBN.AEM.pub({ name: 'GetBattle', msg: settings.snakes, chanel: chanel });
    });
    SBN.AEM.pub({ name: 'LoadImages', msg: SBN.Contract.imageMap, chanel: chanel });

    $buttonStart.on('click', function () {
        if (animation.isRunning()) {
            $buttonStart.html('Start');
            SBN.AEM.pub({ name: 'AnimationStop' });
        } else {
            $buttonStart.html('Stop');
            SBN.AEM.pub({ name: 'AnimationStart' });
        }
    });
});

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
};
