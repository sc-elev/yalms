
//

$(document).ready(function () {


    $("#UserSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: '/Admin/AutocompleteUser',
                dataType: 'json',
                data: { "term": request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Value, value: item.Key };
                    }))
                }
            });
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            //fill selected customer details on form
            event.preventDefault();
            $("#updateUserPartial").load('/Admin/GetUserPartial?userID=' + ui.item.value)
        }
    })

    $("#studentsTree").jstree({
        "plugins": ["themes", "html_data", "checkbox"],
        "checkbox" : {
            "three_state" : false,
        },
    })

    $("#usersTree").jstree({
        "plugins": ["themes", "html_data"],
    })
    .on('select_node.jstree', function (e, node) {
        if (node.node.children.length == 0) {
            var uid = node.node.id;
            $("#updateUserPartial").load('/Admin/GetUserPartial?userID=' + uid)
        }
    })

    if (location.hash) {
        $('a[href=' + location.hash + ']').tab('show');
    }
    $(document.body).on("click", "a[data-toggle]", function (event) {
        location.hash = this.getAttribute("href");
    })
    $("#ClassDropDown").change(function () {
        var classId = $("#ClassDropDown").val();
        $("#ClassListResults").load(
            '/Admin/GetClassList?SelectedClass=' + classId);
    });
});

$(window).on('popstate', function () {
    var anchor = location.hash || $("a[data-toggle=tab]").first().attr("href");
    $('a[href=' + anchor + ']').tab('show');
});

$(window).on('popstate', function () {
    var anchor = location.hash || $("a[data-toggle=tab]").first().attr("href");
    $('a[href=' + anchor + ']').tab('show');
});

function getTreeStudents() {
    var users = $("#studentsTree").jstree('get_checked').join();
    var class_ = $("#ClassDropDown").val();
    var url = '/Admin/AddClassStudents?SelectedUsers=' + users + '&SelectedClass=' + class_
    location.href = url
    return false
}