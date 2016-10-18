/// <reference path="jquery-1.10.2.js" />
/// <reference path="~/Scripts/bootstrap.js" />

function bookTabEvents() {
        $("table[id=\"bookTab\"] tr").click(function () {
            if ($("td:first-child", this).html() != undefined)
                UpdateBookView($("td:first-child", this).html());
        });
        $("table[id=\"bookTab\"] tbody tr").click(function () {
            $("table[id=\"bookTab\"] tbody tr").removeClass();
            $(this).addClass("selected");
        });
        $("table[id=\"bookTab\"] tbody tr").mouseover(function () {
            $("table[id=\"bookTab\"] tbody tr").removeClass();
            $(this).addClass("selected");
        });
        $("table[id=\"bookTab\"] tbody tr").mouseleave(function () {
            $("table[id=\"bookTab\"] tbody tr").removeClass();
        });
    }
function authorTabEvents() {
    $("table[id=\"authorTab\"] tr").click(function () {
        if ($("td:first-child", this).html() != undefined)
            UpdateAuthorView($("td:first-child", this).html());
    });
    $("table[id=\"authorTab\"] tbody tr").click(function () {
        $("table[id=\"authorTab\"] tbody tr").removeClass();
        $(this).addClass("selected");
    });
    $("table[id=\"authorTab\"] tbody tr").mouseover(function () {
        $("table[id=\"authorTab\"] tbody tr").removeClass();
        $(this).addClass("selected");
    });
    $("table[id=\"authorTab\"] tbody tr").mouseleave(function () {
        $("table[id=\"authorTab\"] tbody tr").removeClass();
    });
}

function hideBookIdCol() {
    $('table[id="bookTab"] th:first-child').css("display", "none");
    $('table[id="bookTab"] td:first-child').css("display", "none");
}
function hideAuthorIdCol() {
    $('table[id="authorTab"] th:first-child').css("display", "none");
    $('table[id="authorTab"] td:first-child').css("display", "none");
}

function createThead() {
    var thead = document.createElement("thead");

    if (arguments.length > 0) {
        var tr = document.createElement("tr");

        for (var i = 0; i < arguments.length; ++i) {
            var th = document.createElement("th");
            th.appendChild(document.createTextNode(arguments[i]));
            tr.appendChild(th);
        }
        thead.appendChild(tr);
    }
    return thead;
}
function createBooksTableBody(data) {
    function createCol(row, data) {
        var td = document.createElement("td");
        td.appendChild(document.createTextNode(data));
        row.appendChild(td);
    }

    var tbdy = document.createElement("tbody");

    for (var i = 0; i < data.ContentModels.length; ++i) {

        var tr = document.createElement("tr");
        tr.setAttribute("id", data.ContentModels[i].BookId);

        createCol(tr, data.ContentModels[i].BookId);
        createCol(tr, data.ContentModels[i].BookName);
        createCol(tr, data.ContentModels[i].Year);
        createCol(tr, data.ContentModels[i].AuthorName);
            
        tbdy.appendChild(tr);
    }
    return tbdy;
}
function createAuthorsTableBody(data) {
    function createCol(row, data) {
        var td = document.createElement("td");
        td.appendChild(document.createTextNode(data));
        row.appendChild(td);
    }

    var tbdy = document.createElement("tbody");

    for (var i = 0; i < data.ContentModels.length; ++i) {

        var tr = document.createElement("tr");
        tr.setAttribute("id", data.ContentModels[i].BookId);

        createCol(tr, data.ContentModels[i].AuthorId);
        createCol(tr, data.ContentModels[i].AuthorName);
        createCol(tr, data.ContentModels[i].Count);

        tbdy.appendChild(tr);
    }
    return tbdy;
}

function displayBooks(data) {

    var div = $("#Books");
    div.empty();

    var tbl = document.createElement("table");
    tbl.setAttribute("class", "table color_table");
    tbl.setAttribute("id", "bookTab");

    var thead = createThead("ID", "Name", "Year", "Author");
        
    tbl.appendChild(thead);

    var tbdy = createBooksTableBody(data);
        
    tbl.appendChild(tbdy);

    $("#Books").append(tbl);

    bookTabEvents();

    hideBookIdCol();
}
function displayAuthors(data) {
    var div = $("#Authors");
    div.empty();

    var tbl = document.createElement("table");
    tbl.setAttribute("class", "table color_table");
    tbl.setAttribute("id", "authorTab");

    var thead = createThead("ID", "Name", "Books");

    tbl.appendChild(thead);

    var tbdy = createAuthorsTableBody(data);

    tbl.appendChild(tbdy);
        
    $("#Authors").append(tbl);

    authorTabEvents();

    hideAuthorIdCol();
}

//Request to server
function Update(id) {
        
        //GetBooks
        if (id == null) {
            $.ajax({
                url: "/api/content/GetBooks",
                type: "GET",
                dataType: "json",
                success: function (data) {
                    displayBooks(data);
                    var div = $("#bookPageLinks");
                    div.empty();
                    if (data.PageInfo.TotalItems > data.PageInfo.PageSize)
                        CreateBookPageLinks(data.PageInfo);
                    pageLinksStyle();
                },
                error: function(data) {
                    var div = $("#Books");
                    div.empty();
                    var substring = "Database is empty";
                    if (data.responseText.search(substring) != -1)
                        div.append(substring);
                }
            });
        } else {
            $.ajax({
                url: "/api/content/GetBooks",
                type: "GET",
                dataType: "json",
                data: { index: id },
                success: function (data) {
                    displayBooks(data);
                },
                error: function (data) {
                    var div = $("#Books");
                    div.empty();
                    var substring = "Database is empty";
                    if (data.responseText.search(substring) != -1)
                        div.append(substring);
                }
            });
        } 

        //GetAuthors
        $.ajax({
            url: "/api/content/GetAuthors",
            type: "GET",
            dataType: "json",
            success: function (data) {
                displayAuthors(data);
                var div = $("#authorPageLinks");
                div.empty();
                if (data.PageInfo.TotalItems > data.PageInfo.PageSize)
                    CreateAuthorPageLinks(data.PageInfo);
                pageLinksStyle();
            },
            error: function(data) {
                var div = $("#Authors");
                div.empty();
                var substring = "Database is empty";
                if (data.responseText.search(substring) != -1)
                    div.append(substring);
            }
        });
}
   
//Hide Modal view -> update content
function UpdateIndexContent(model, modDialogId) {

    if (modDialogId != null) {
        $(modDialogId).modal("hide");
    } else {
        $("#modDialog").modal("hide");
    }

    try {
        $(function() {
            Update();
        });
    } catch (e) {
        console.log(e);
    }
}

function NewBookView() {
    $(function () {
        $.ajax({
            url: '/home/NewBookView',
            type: "POST",
            success: function (data) {
                $("#dialogBookContent").html(data);
                $("#modBookDialog").modal("show");
            }
        });
    });
}
function UpdateBookView(id) {
    $(function () {
        $.ajax({
            url: '/home/NewBookView',
            type: "POST",
            data: { index: id },
            success: function (data) {
                $("#dialogBookContent").html(data);
                $("#modBookDialog").modal("show");
            }
        });
    });
}
function NewAuthorView() {
    $(function () {
        $.ajax({
            url: '/home/NewAuthorView',
            type: "POST",
            success: function (data) {
                $("#dialogAuthorContent").html(data);
                $("#modAuthorDialog").modal("show");
            }
        });
    });
}
function UpdateAuthorView(id) {
    $(function () {
        $.ajax({
            url: '/home/NewAuthorView',
            type: "POST",
            data: { index: id },
            success: function (data) {
                $("#dialogAuthorContent").html(data);
                $("#modAuthorDialog").modal("show");
            }
        });
    });
}

// subscription buttons to events
function btnEvents() {
    $("#createBook").bind("click", NewBookView);
    $("#createAuthor").bind("click", NewAuthorView);
}

function CreateBookPageLinks(data) {
    var viewModel = { totalPages: data.TotalPages, pageNumber: data.PageNumber }

    $.ajax({
        url: "/home/PageLinksForBookTab",
        type: "GET",
        data: viewModel,
        success: function(data) {
            $("#bookPageLinks").html(data);
            pageLinksStyle();
        },
        error: function(data) {
            console.log(data);
        }
    });
}
function CreateAuthorPageLinks(data) {
    var viewModel = { totalPages: data.TotalPages, pageNumber: data.PageNumber }

    $.ajax({
        url: "/home/PageLinksForAuthorTab",
        type: "GET",
        data: viewModel,
        success: function (data) {
            $("#authorPageLinks").html(data);
            pageLinksStyle();
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function UpdateBooksContentWithPage(page) {
    $(function () {
        var pageNumber;
        if (page == null) {
            pageNumber = $(".btn-primary").html();
        } else {
            pageNumber = page;
        }

        var viewModel = {
            page: pageNumber
        }
        $.ajax({
            url: "/api/content/GetBooksByPage",
            type: "GET",
            dataType: "json",
            data: viewModel,
            success: function (data) {
                displayBooks(data);
            },
            error: function (data) {
                var div = $("#Books");
                div.empty();
                var substring = "Database is empty";
                if (data.responseText.search(substring) != -1)
                    div.append(substring);
            }
        });
    });
}
function UpdateAuthorsContentWithPage(page) {
    $(function () {
        var pageNumber;
        if (page == null) {
            pageNumber = $(".btn-primary").html();
        } else {
            pageNumber = page;
        }

        var viewModel = {
            page: pageNumber
        }
        $.ajax({
            url: "/api/content/GetAuthorsByPage",
            type: "GET",
            dataType: "json",
            data: viewModel,
            success: function (data) {
                displayAuthors(data);
            },
            error: function (data) {
                var div = $("#Authors");
                div.empty();
                var substring = "Database is empty";
                if (data.responseText.search(substring) != -1)
                    div.append(substring);
            }
        });
    });
}

function pageLinksStyle() {
    $("#bookPageLinks a:first-child").css("background-color", "#428bca");
    $("#authorPageLinks a:first-child").css("background-color", "#428bca");

    $("#bookPageLinks a").click(function () {
        $("#bookPageLinks a").removeClass();
        $("#bookPageLinks a").addClass("btn btn-default");
        $("#bookPageLinks a").css("background-color", "white");
        $(this).addClass("selected");
        $(this).addClass("btn-primary");
        $(this).css("background-color", "#428bca");
    });
    $("#authorPageLinks a").click(function () {
        $("#authorPageLinks a").removeClass();
        $("#authorPageLinks a").addClass("btn btn-default");
        $("#authorPageLinks a").css("background-color", "white");
        $(this).addClass("selected");
        $(this).addClass("btn-primary");
        $(this).css("background-color", "#428bca");
    });
}