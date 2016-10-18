
var currentBtns = []; // display list certain keys this modal window

var content = {}; // window content storage

// content reading
function readContent() {
    var bookId = $('#bookId').val();
    if (bookId != '') bookId = parseInt($('#bookId').val());
    else bookId = -1;

    content.bookId = bookId;
    content.bookName = $('#bookName').val();

    var authorNameSelect = $('#authorNameSelect').val();
    var authorNameInput = $('#authorNameInput').val().replace(new RegExp('\n', 'g'), '');
    var authorId = -1; //new author
    var authorName = authorNameInput;
    if (authorNameInput == authorNameSelect) {
        authorName = authorNameInput;
        authorId = -2; // present author
    }
    content.authorId = authorId;
    content.authorName = authorName;
    content.year = $('#bookYear').val();
}

// book creation - POST
function CreateBook() {

    readContent();

    var viewModel = {
        BookId: content.bookId,
        BookName: content.bookName,
        AuthorId: content.authorId,
        AuthorName: content.authorName,
        Year: content.year
    }

    $.ajax({
        url: '/api/content/PostBook',
        type: 'POST',
        data: viewModel,
        success: function(model) {
            window.UpdateIndexContent(model, '#modBookDialog');
        },
        error: function(data) {
            $('#bookResult').css("display", "block");
            $('#bookResult').append(data.responseText);
        }
    });
}

// modification of book - PUT
function UpdateBook() {

    readContent();

    var viewModel = {
        BookId: content.bookId,
        BookName: content.bookName,
        AuthorId: content.authorId,
        AuthorName: content.authorName,
        Year: content.year
    }

    $.ajax({
        url: '/api/content/PutBook',
        type: 'PUT',
        data: viewModel,
        success: function(model) {
            window.UpdateIndexContent(model, '#modBookDialog');
        },
        error: function(data) {
            $('#bookResult').css("display", "block");
            $('#bookResult').append(data.responseText);
        }
    });
}

// deleting of book - DELETE
function DeleteBook() {

    readContent();

    $.ajax({
        url: '/api/book/' + content.bookId,
        type: 'DELETE',
        success: function(model) {
            window.UpdateIndexContent(model, '#modBookDialog');
        },
        error: function(data) {
            $('#bookResult').css("display", "block");
            $('#bookResult').append(data.responseText);
        }
    });
}

// button on modal view. Shows when all form elements are filled
function displayButton() {
    var book = $('#bookName').val().replace(new RegExp('\n', 'g'), '').replace(new RegExp(' ', 'g'), '');
    var year = $('#bookYear').val().replace(new RegExp('\n', 'g'), '').replace(new RegExp(' ', 'g'), '');
    var authorNameInput = $('#authorNameInput').val().replace(new RegExp('\n', 'g'), '').replace(new RegExp(' ', 'g'), '');
    if (book !== '' && year !== '' && authorNameInput !== '') {
        for (var i = 0; i < currentBtns.length; ++i) {
            $('#bookResult').text('');
            $('#bookResult').css("display", "none");
            $(currentBtns[i]).css("display", "block");
        }
    } else {
        for (var i = 0; i < currentBtns.length; ++i) {
            $(currentBtns[i]).css("display", "none");
        }
    }
}

// request for info about the authors - GET
function getAuthors() {
    $.ajax({
        url: '/api/author',
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            var sel = document.getElementById('authorNameSelect');
            var opt = null;

            for (var i = -1; i < data.length; ++i) {
                opt = document.createElement('option');
                if (i == -1) {
                    opt.value = '';
                    opt.id = i;
                    opt.innerHTML = '';
                } else {
                    opt.value = data[i].Name;
                    opt.id = data[i].Id;
                    opt.innerHTML = data[i].Name;
                }
                sel.appendChild(opt);
            }
        },
        error: function(data) {
            $('#bookResult').css("display", "block");
            $('#bookResult').append(data.responseText);
        }
    });
}

// request for book list - GET
function getBook(model) {
    $.ajax({
        url: '/api/content/getbooks',
        type: 'GET',
        data: { index: model },
        dataType: 'json',
        success: function(data) {
            getAuthors();

            $('#bookId').val(data.ContentModels[0].BookId);
            $('#bookName').val(data.ContentModels[0].BookName);
            $('#bookYear').val(data.ContentModels[0].Year);
            $('#authorNameInput').val(data.ContentModels[0].AuthorName);
            $('#authorNameSelect').val(data.ContentModels[0].AuthorName);
            currentBtns = ["#updateBookBtn", "#deleteBookBtn"];
            displayButton();
        },
        error: function(data) {
            $('#bookResult').css("display", "block");
            $('#bookResult').append(data.responseText);
        }
    });
}

// AuthorName TextBox filling of selected in the ComboBox
function setAuthorNameTextBoxFromComboBox() {
    var select = document.getElementById("authorNameSelect");
    select.addEventListener("click",function () {
        $('#authorNameInput').val($('#authorNameSelect').val());
        displayButton();
    }, false);
}

// subscription to change Author Name TextBox
function changeAuthorNameTextBox() {
    var input = document.getElementById("authorNameInput");
    input.addEventListener("change", function () {
        displayButton();
    }, false);
}

// subscription to change the content of a modal window
function keyUpDisplBtnForModalViewContent() {
    $('#content').keyup(function() { displayButton(); });
}

// subscription buttons to events
function btnEvents() {
    $("#createBookBtn").bind("click", CreateBook);
    $("#updateBookBtn").bind("click", UpdateBook);
    $("#deleteBookBtn").bind("click", DeleteBook);
}