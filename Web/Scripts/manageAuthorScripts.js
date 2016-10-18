
var currentBtns = []; // display list certain keys this modal window

var content = {}; // window content storage

// content reading
function readContent() {
    var authorId = $('#authorId').val();
    if (authorId != '') authorId = parseInt($('#authorId').val());
    else authorId = -1;

    content.authorId = authorId;
    content.authorName = $('#authorName').val().replace(new RegExp('\n', 'g'), '');
}

// author creation - POST
function CreateAuthor() {
        
    readContent();
        
    var viewModel = {
        AuthorId: content.authorId,
        AuthorName: content.authorName
    }

    $.ajax({
        url: '/api/content/PostAuthor',
        type: 'POST',
        data: viewModel,
        success: function(model) {
            window.UpdateIndexContent(model, '#modAuthorDialog');
        },
        error: function(data) {
            $('#authorResult').css("display", "block");
            $('#authorResult').append(data.responseText);
        }
    });
}

// modification of author - PUT
function UpdateAuthor() {

    readContent();

    var viewModel = {
        AuthorId: content.authorId,
        AuthorName: content.authorName
    }

    $.ajax({
        url: '/api/content/PutAuthor',
        type: 'PUT',
        data: viewModel,
        success: function(model) {
            window.UpdateIndexContent(model, '#modAuthorDialog');
        },
        error: function(data) {
            $('#authorResult').css("display", "block");
            $('#authorResult').append(data.responseText);
        }
    });
}

// deleting of author - DELETE
function DeleteAuthor() {
        
    readContent();

    $.ajax({
        url: '/api/author/' + content.authorId,
        type: 'DELETE',
        success: function(model) {
            window.UpdateIndexContent(model, '#modAuthorDialog');
        },
        error: function(data) {
            $('#authorResult').css("display", "block");
            $('#authorResult').append(data.responseText);
        }
    });
}

// button on modal view. Shows when all form elements are filled
function displayButton() {
    var author = $('#authorName').val().replace(new RegExp('\n', 'g'), '').replace(new RegExp(' ', 'g'), '');
    if (author !== '') {
        for (var i = 0; i < currentBtns.length; ++i) {
            //$('#authorResult').css("display", "none");
            $(currentBtns[i]).css("display", "block");
        }
    } else {
        for (var i = 0; i < currentBtns.length; ++i) {
            $(currentBtns[i]).css("display", "none");
            $('#authorResult').css("display", "block");
        }
    }
}

// request for info about the author - GET
function getAuthor(model) {
    $.ajax({
        url: '/api/content/GetAuthors',
        type: 'GET',
        data: { index: model },
        dataType: 'json',
        success: function(data) {
            $('#authorId').val(data.ContentModels[0].AuthorId);
            $('#authorName').val(data.ContentModels[0].AuthorName);
            if(data.ContentModels[0].Count > 0){
                currentBtns = ["#updateAuthorBtn"];
                $('#authorResult').css("display", "block");
                $('#authorResult').append("Delete button will be enabled when Book's Count will be zero");
            } else {
                currentBtns = ["#updateAuthorBtn", "#deleteAuthorBtn"];
            }
            displayButton();
        },
        error: function(data) {
            $('#authorResult').css("display", "block");
            $('#authorResult').append(data.responseText);
        }
    });
}

// subscription to change Author Name TextBox
function changeAuthorNameTextBox() {
    var input = document.getElementById("authorName");
    input.addEventListener("change", function () {
        displayButton();
    }, false);
}

// subscription to change the content of a modal window
function keyUpDisplBtnForModalViewContent() {
    $('#content').keyup(function() { displayButton(); });
    $('#authorName').keyup(function() { displayButton(); });
}

// subscription buttons to events
function btnEvents() {
    $("#createAuthorBtn").bind("click", CreateAuthor);
    $("#updateAuthorBtn").bind("click", UpdateAuthor);
    $("#deleteAuthorBtn").bind("click", DeleteAuthor);
}

