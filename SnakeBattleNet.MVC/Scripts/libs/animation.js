function init() {
    var canvas = document.getElementById("canva");

    var snake = {
        segmentLength: 2,
        numSegments: 11,
        segments: [{
            // tail
            x: 50,
            y: 50
        }, {
            // head
            x: 50,
            y: 50
        }]
    };

    var imageObject = new Image();
    imageObject.onload = function () {
        animate(canvas, snake, imageObject);
    };
    imageObject.src = 'Content/snake.bmp';
};

function animate(canvas, snake, imageObject) {
    var context = canvas.getContext("2d");

    // update
    updateSnake(canvas, snake);

    // clear
    context.clearRect(0, 0, canvas.width, canvas.height);

    // draw
    drawSnake(context, snake, imageObject);

    // request new frame
    requestAnimFrame(function () { animate(canvas, snake, imageObject); });
}

function updateSnake(canvas, snake) {
    var segments = snake.segments;
    var head = segments[segments.length - 1];

    var newHeadX = head.x + 1;
    var newHeadY = head.y;

    // add new segment
    segments.push({
        x: newHeadX,
        y: newHeadY
    });

    if (segments.length > snake.numSegments) {
        segments.shift();
    }
}

function drawSnake(context, snake, imageObject) {
    var segments = snake.segments;
    var tail = segments[0];

    var s = 10;
    context.drawImage(imageObject, 0, 0, s, s, tail.x, tail.y, s, s);
}

window.requestAnimFrame = (function (callback) {
    return window.requestAnimationFrame ||
                window.webkitRequestAnimationFrame ||
                window.mozRequestAnimationFrame ||
                window.oRequestAnimationFrame ||
                window.msRequestAnimationFrame ||
                function (callback) {
                    window.setTimeout(callback, 1000 / 60);
                };
})();