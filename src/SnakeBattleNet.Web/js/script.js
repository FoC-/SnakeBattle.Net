//Global variables
var imageObjects = new Array();
var scale = 10;
var context;
var battleReplay;
var currentStep = 0;

function StartReplay(replayLink) {
    $.ajax({
        url: replayLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { battleReplay = data; },
        complete: function () { InitResources(); },
        async: false
    });
}

function InitResources() {
    var canvas = document.getElementById("canva");
    canvas.width = battleReplay.FieldWidth * scale;
    canvas.height = battleReplay.FieldHeight * scale;
    context = canvas.getContext("2d");
    PreloadTextures(battleReplay.UniqueToShortIdMap);
    window.setTimeout("animate();", 1000);
}

function PreloadTextures(textureIdMap) {
    for (var i = 0; i < textureIdMap.length; i++) {
        imageObjects[textureIdMap[i].S] = new Image();
        imageObjects[textureIdMap[i].S].src = '/Manager/GetTexture/' + textureIdMap[i].L;
        //imageObjects[textureIdMap[i].S].src = 'img/' + textureIdMap[i].L + '.bmp';
    }
}

function animate() {
    DrawEvent();
    // request new frame
    requestAnimFrame(function () { animate(); });
}

function DrawEvent() {
    if (currentStep < battleReplay.events.length) {
        var event = battleReplay.events[currentStep];
        var element = determElement(event.E);
        context.drawImage(imageObjects[event.I], element, event.D * 10, 10, 10, event.X * scale, event.Y * scale, scale, scale);
    }
    currentStep++;
}

function determElement(elementCode) {
    var e = 0;
    switch (elementCode) {
        case 0: //head
            e = 0;
            break;
        case 1: //body
            e = 10;
            break;
        case 2: //tail
            e = 20;
            break;
        case 3: //wall
            e = 0;
            break;
        case 4: //empty
            e = 10;
            break;
        case 5: //gate
            e = 20;
            break;
    }
    return e;
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