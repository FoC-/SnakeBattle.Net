var app = angular.module('app', ['ngResource', 'ui.sortable']);

app.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {
    $routeProvider.when('/', {
        templateUrl: '/SPA/Home',
        controller: 'HomeCtrl',
    });
    $routeProvider.when('/Snake/Edit/:id', {
        templateUrl: '/SPA/Edit',
        controller: 'SnakeEditCtrl',
    });
    $routeProvider.otherwise({
        redirectTo: '/'
    });

    // Specify HTML5 mode (using the History APIs) or HashBang syntax.
    $locationProvider.html5Mode(false).hashPrefix('!');

    var authoriseRedirect = ['$q', '$window', function ($q, $window) {
        var success = function (response) {
            return response;
        };

        var error = function (response) {
            if (response.status === 401) {
                $window.location.href = '/';
                return $q.reject(response);
            } else {
                return $q.reject(response);
            }
        };

        return function (promise) {
            return promise.then(success, error);
        };
    }];
    $httpProvider.responseInterceptors.push(authoriseRedirect);
}]);

app.factory("Snake", function ($resource) {
    return $resource('/api/snake/:id');
});

app.factory('ImageLoader', function ($q) {
    var get = function (src, scope) {
        var deferred = $q.defer();

        function safeApply(fn) {
            if (!scope.$$phase) {
                scope.$apply(fn);
            }
            else {
                fn();
            }
        }

        var image = new Image();
        image.onload = function () {
            safeApply(function () {
                deferred.resolve(image);
            });
        };

        image.onerror = function () {
            deferred.reject('Failed to load image');
        };
        image.src = src + '?' + new Date().getTime();
        return deferred.promise;
    };
    return {
        get: get
    };
});

app.directive('cSelector', function (ImageLoader) {
    return {
        restrict: 'A',
        scope: {
            image: '=',
            ar: '=',
            map: '='
        },
        link: function ($scope, $element) {
            var image = ImageLoader.get($scope.image + '?' + new Date().getTime(), $scope);

            var stage = new Kinetic.Stage({
                container: $element[0],
                width: 300,
                height: 100
            });
            var layer = new Kinetic.Layer();
            stage.add(layer);
            image.then(function (realImage) {
                angular.forEach($scope.map, function (region) {
                    var img = new Kinetic.Image(angular.extend({ image: realImage }, region));
                    img.on('click', function () {
                        $scope.$apply(function () {
                            $scope.ar = region.key;
                        });
                    });
                    layer.add(img);
                    stage.draw();
                });
            });
        }
    };
});

app.directive('cDraw', function () {
    return {
        restrict: 'A',
        scope: {
            map: '=',
            val: '=',
            hr: '=',
            wr: '='
        },
        link: function ($scope, $element) {
            var stage = new Kinetic.Stage({
                container: $element[0],
                width: 300,
                height: 100
            });
            var layer = new Kinetic.Layer();
            stage.add(layer);

            var width = 300 / scope.wr,
                height = 100 / scope.hr;

            function putValueOnMap(xv, yv, vv, model) {
                var len = model.length,
                    i;
                for (i = 0; i < len; i++) {
                    if (model[i].x === xv && model[i].y === yv) {
                        scope.$apply(function () {
                            model[i].v = vv;
                        });
                        return;
                    }
                }
                scope.$apply(function () {
                    model[len] = { x: xv, y: yv, v: vv };
                });
            }

            function putSpriteOnCanvas(xv, yv, color) {
                context.fillStyle = color ? color : "yellow";
                context.fillRect(xv * width, yv * height, width, height);
            }

            element.bind('click', function (event) {
                var x = Math.floor(event.offsetX / width),
                    y = Math.floor(event.offsetY / height);
                putSpriteOnCanvas(x, y);
                putValueOnMap(x, y, scope.val, scope.map);
            });

            scope.$watch('map', function () {
                var len = scope.map.length,
                    i;
                for (i = 0; i < len; i++) {
                    putSpriteOnCanvas(scope.map[i].x, scope.map[i].y, scope.map[i].v);
                }
            }, true);
        }
    };
});

function HomeCtrl($scope, Snake) {
    $scope.snakes = Snake.query();

    $scope.delete = function (snake) {
        Snake.delete({ id: snake.id }, function () {
            $scope.snakes.splice($scope.snakes.indexOf(snake), 1);
        });
    };
}

function SnakeEditCtrl($scope, $location, $routeParams, Snake) {
    $scope.snake = Snake.get({ id: $routeParams.id });

    $scope.save = function () {
        $scope.snake.$save(function () {
            $location.path('/');
        });
    };

    //$scope.elementSelector = 'http://localhost:19514/Content/images/glyphicons-halflings.png';
    $scope.elementSelector = 'http://glue.readthedocs.org/en/0.2/_images/famfamfam2.png';
    
    $scope.map = [];
    $scope.map[$scope.map.length] = { x: 0, y: 0, width: 50, height: 50, crop: { x: 0, y: 0, width: 50, height: 50 }, key: 'red' };
    $scope.map[$scope.map.length] = { x: 50, y: 0, width: 50, height: 50, crop: { x: 50, y: 0, width: 50, height: 50 }, key: 'blue' };
    $scope.map[$scope.map.length] = { x: 0, y: 50, width: 50, height: 50, crop: { x: 0, y: 50, width: 50, height: 50 }, key: 'green' };
    $scope.map[$scope.map.length] = { x: 50, y: 50, width: 50, height: 50, crop: { x: 50, y: 50, width: 50, height: 50 }, key: 'white' };

    $scope.addNew = function () {
        $scope.snake.brainModules.push({});
    };

    $scope.insert = function (array, element, input) {
        array.splice(array.indexOf(element), 0, input);
    };

    $scope.remove = function (array, element) {
        array.splice(array.indexOf(element), 1);
    };

    $scope.sortableOptions = {
    };
}