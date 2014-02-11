var numberOfModules = 0;
var pieceWidth = 20, pieceHeight = 20;

function AddModule(snakeId) {
    InsertModule(snakeId, 'AddNew');
}

function InsertModule(snakeId, moduleId) {
    var position = $("#Modules div.span4").index($("#" + moduleId));
    var newModuleId = guidGenerator();

    if (SetModuleSuccess(snakeId, newModuleId, position)) {
        CreateModuleElement(snakeId, newModuleId, moduleId);
        DrawModule(snakeId, newModuleId);
    };
}

function SetModuleSuccess(snakeId, moduleId, position) {
    var response;

    $.ajax({
        url: 'Edit/InsertModule',
        type: 'POST',
        dataType: 'json',
        data: '{snakeId:"' + snakeId +
           '", moduleId:"' + moduleId +
           '", position:' + position + '}',
        contentType: 'application/json',
        success: function (data) { response = data; },
        async: false
    });

    if (response.Status === "OK") {
        numberOfModules++;
        return true;
    }
    return false;
}

function CreateModuleElement(snakeId, newModuleId, moduleId) {
    var html = '<div class="span4" id="' + newModuleId + '">' +
            '<div class="span2"></div>' +
            '<div class="span1">' +
                '<div class="btn btn-mini" onclick="DeleteModule(\'' + snakeId + '\', \'' + newModuleId + '\');"><span>Delete</span></div>' +
                '<div class="btn btn-mini" onclick="InsertModule(\'' + snakeId + '\', \'' + newModuleId + '\');"><span>Insert</span></div>' +
            '</div>' +
           '</div>';
    $('#Modules #' + moduleId).before(html);
}

function DeleteModule(snakeId, moduleId) {
    var response;
    $.ajax({
        url: 'Edit/DeleteModule',
        type: 'POST',
        dataType: 'json',
        data: '{snakeId: "' + snakeId + '", moduleId:"' + moduleId + '"}',
        contentType: 'application/json',
        success: function (data) { response = data; },
        async: false
    });

    if (response.Status === "OK") {
        numberOfModules--;
        $("#" + moduleId).remove();
    }
}


function DrawModule(snakeId, moduleId) {
    var response;
    $.ajax({
        url: 'Edit/GetModule',
        type: 'POST',
        dataType: 'json',
        data: '{snakeId: "' + snakeId + '", moduleId:"' + moduleId + '"}',
        contentType: 'application/json',
        success: function (data) { response = data; },
        async: false
    });

    if (response.Status === "OK") {
        numberOfModules++;

        var canvas = CreateCanvas(moduleId, CanvaClick);
        FillModule(canvas, response.Module);
    }
}

function CreateCanvas(moduleId, canvaClick) {
    var elementId = "canva_" + moduleId;
    var html = '<canvas height="140" width="140" class="table-bordered" id="' + elementId + '" ></canvas>';
    $("#" + moduleId + " .span2").prepend(html);
    var canvas = document.getElementById(elementId);
    canvas.onclick = canvaClick;
    canvas.onselectstart = function () { return false; };
    return canvas;
}


function CanvaClick(e) {
    var cell = getCellUnderClick(e);

    var id = e.currentTarget.id;
    var canvas = document.getElementById(id);
    var context = canvas.getContext("2d");

    context.fillStyle = "rgb(200,153,255)";
    context.fillRect(cell.x * pieceWidth, cell.y * pieceHeight, pieceWidth, pieceHeight);
}

function FillModule(canvas, module) {
    var context = canvas.getContext("2d");
    for (var i = 0; i < 7; i++) {
        for (var j = 0; j < 7; j++) {
            context.fillStyle = "rgb(0,170,0)";
            context.fillRect(i * pieceWidth, j * pieceHeight, pieceWidth, pieceHeight);
        }
    }
}



function getCellUnderClick(e) {
    var id = e.currentTarget.id;
    var element = document.getElementById(id);
    var offsetX = 0, offsetY = 0;

    if (element.offsetParent) {
        do {
            offsetX += element.offsetLeft;
            offsetY += element.offsetTop;
        } while ((element = element.offsetParent));
    }

    var mx, my; // mouse coordinates
    mx = e.pageX - offsetX;
    my = e.pageY - offsetY;
    return { x: Math.floor(mx / pieceWidth), y: Math.floor(my / pieceHeight) };
}

function guidGenerator() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}